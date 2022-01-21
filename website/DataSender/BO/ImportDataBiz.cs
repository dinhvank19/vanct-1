using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using DataSender.Properties;
using DataSender.Requester;
using Hulk.Shared;
using Hulk.Shared.Log;
using Newtonsoft.Json;
using Vanct.Dal.BO;

namespace DataSender.BO
{
    public class ImportDataBiz
    {
        #region Properties

        public Property Property;
        private readonly string _connectionString;
        private OleDbConnection _connection;
        private readonly string _workingPath;
        private readonly string _worksPath;

        #endregion

        /// <summary>
        /// Gets the properties.
        /// </summary>
        /// <exception cref="System.Exception"></exception>
        public ImportDataBiz(string workingPath, string worksPath, string dataFilename, string dataPassword)
        {
            if (!File.Exists(dataFilename))
                throw new Exception("Could not found file " + dataFilename);

            _workingPath = workingPath;
            _worksPath = worksPath;
            Property = new Property
            {
                FilePassword = dataPassword,
                FileName = dataFilename,
                Key = Settings.Default.Key
            };

            _connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Property.FileName
                            + ";Jet OLEDB:Database Password=" + Property.FilePassword + ";";
        }

        /// <summary>
        /// Gets the user.
        /// </summary>
        public void SyncWorking()
        {
            #region pickup old data from json-file
            ReportUser sentReport;

            // old json-file already exist
            if (File.Exists(_workingPath))
            {
                var propertyJson = _workingPath.ReadFile();
                sentReport = JsonConvert.DeserializeObject<ReportUser>(propertyJson);
            }

            else // not exist, that mean first time sync data to server
                sentReport = new ReportUser {AccessToken = Property.Key};

            // initial report data
            var reportData = new ReportUser {AccessToken = Property.Key};

            #endregion

            #region get data from mdb

            using (_connection = new OleDbConnection(_connectionString))
            {
                _connection.Open();
                using (var command = _connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;

                    #region get all tables

                    command.CommandText = "select MABAN, COKHACH, CODOI, INBILL, STT, GIOVAO, GIORA, GIAM, PHUCVU from [DANH MUC BAN] where [DONG] = 0 order by STT;";
                    using (var dataReader = command.ExecuteReader())
                    {
                        if (dataReader == null)
                            throw new Exception("Can not ExecuteReader from table [DANH MUC BAN] - get all tables");

                        reportData.Tables.Clear();

                        while (dataReader.Read())
                        {
                            reportData.Tables.MergeItem(new ReportTable
                            {
                                TableNo = dataReader["MABAN"] as string,
                                IsBusy = (bool)dataReader["COKHACH"],
                                HasChanged = (bool)dataReader["CODOI"],
                                IsPrinted = (bool)dataReader["INBILL"],
                                No = (int)dataReader["STT"],
                                InDate = dataReader["GIOVAO"] != DBNull.Value
                                    ? (DateTime)dataReader["GIOVAO"]
                                    : (DateTime?)null,
                                OutDate = dataReader["GIORA"] != DBNull.Value
                                    ? (DateTime)dataReader["GIORA"]
                                    : (DateTime?)null,
                                Discount = dataReader["GIAM"] != DBNull.Value
                                    ? (int)dataReader["GIAM"]
                                    : 0,
                                Servicer = dataReader["PHUCVU"] as string
                            });
                        }
                    }

                    #endregion

                    #region push lines to tables

                    command.CommandText = "select SOBAN, MAHG, TENHANG, SOLUONG, DONGIA, DVT, NHOM, DABAO, DateValue(NGAY) + TimeValue(GIO) as NGAYGIO from [BAN];";
                    using (var dataReader = command.ExecuteReader())
                    {
                        if (dataReader == null)
                            throw new Exception("Can not ExecuteReader from table [BAN] - push lines to tables");

                        while (dataReader.Read())
                        {
                            var tableNo = dataReader["SOBAN"] as string;
                            var table = reportData.Tables.Get(tableNo);
                            if (table == null) continue;

                            table.Lines.MergeItem(new ReportTableline
                            {
                                TableNo = tableNo,
                                ProductName = dataReader["TENHANG"] as string,
                                ProductNo = dataReader["MAHG"] as string,
                                Amout = (double) dataReader["SOLUONG"],
                                Price = (int) dataReader["DONGIA"],
                                Om = dataReader["DVT"] as string,
                                ProductGroup = dataReader["NHOM"] as string,
                                InDate = dataReader["NGAYGIO"] != DBNull.Value
                                    ? (DateTime) dataReader["NGAYGIO"]
                                    : (DateTime?) null,
                                DaBao = dataReader["DABAO"] != DBNull.Value && (bool) dataReader["DABAO"]
                            });
                        }
                    }

                    #endregion

                    #region count total received of current work

                    //command.CommandText = "select sum(SOLUONG*DONGIA) as TOTAL, MANV, XUAT from [BAN] where SOBAN is null group by MANV, XUAT";
                    command.CommandText = "select sum(SOLUONG*DONGIA) as TOTAL, XUAT from [BAN] where SOBAN is null group by XUAT";
                    using (var dataReader = command.ExecuteReader())
                    {
                        if (dataReader == null)
                            throw new Exception("Can not ExecuteReader from table [BAN] - count total received of current work");

                        if (dataReader.Read())
                        {
                            reportData.Working = new ReportWork
                            {
                                Date = DateTime.Now,
                                Total = (double)dataReader["TOTAL"],
                                //Username = dataReader["MANV"] as string,
                                Username = String.Empty,
                                WorkTime = dataReader["XUAT"] as string
                            };
                        }
                    }

                    #endregion
                }
            }

            #endregion

            #region prepare and check different data

            var readyForSend = new ReportUser { AccessToken = Property.Key };

            // first time
            if (!sentReport.Tables.Any())
            {
                readyForSend = reportData;
            }

            else // update times
            {
                foreach (var table in reportData.Tables)
                {
                    var oldTable = sentReport.Tables.Get(table.TableNo);
                    if (oldTable == null)
                    {
                        LoggingFactory.GetLogger().Log("[InsertData]" + table.TableNo);
                        readyForSend.Tables.MergeItem(table);
                        continue;
                    }

                    var oldJson = JsonConvert.SerializeObject(oldTable);
                    var newJson = JsonConvert.SerializeObject(table);
                    if (oldJson.Equals(newJson)) continue;

                    LoggingFactory.GetLogger().Log("[UpdateData]" + table.TableNo);
                    readyForSend.Tables.MergeItem(table);
                }

                readyForSend.Working = reportData.Working;
            }

            #endregion

            #region sync data to server

            try
            {
                var rfsJson = JsonConvert.SerializeObject(readyForSend);
                using (var client = new VanctClient())
                {
                    client.SyncData(Property.Key, rfsJson);
                }
            }
            catch (Exception ex)
            {
                LoggingFactory.GetLogger().Log("[sync data to server]" + ex);
            }

            #endregion

            #region save data to file after sent

            _workingPath.DeleteFile();
            var saveJson = JsonConvert.SerializeObject(reportData);
            _workingPath.WriteFile(saveJson);

            #endregion
        }

        /// <summary>
        /// Totals the received per day.
        /// </summary>
        /// <exception cref="System.Exception">Tập tin dữ liệu không tồn tại</exception>
        public void SyncWorks()
        {
            #region pickup old data from json-file
            ReportUser sentReport;

            // old json-file already exist
            if (File.Exists(_worksPath))
            {
                var propertyJson = _worksPath.ReadFile();
                sentReport = JsonConvert.DeserializeObject<ReportUser>(propertyJson);
            }

            else // not exist, that mean first time sync data to server
                sentReport = new ReportUser { AccessToken = Property.Key };

            // initial report data
            var reportData = new ReportUser { AccessToken = Property.Key };

            #endregion

            #region prepare and check different data

            var maximumDate = "20000101";
            if (sentReport.Works.Any())
                maximumDate = sentReport.Works.Max(i => i.Date).Date.ToString("yyyyMMdd");

            var list = GetWorks(maximumDate);
            if (!list.Any()) return;
            reportData.Works = list.Where(i => !sentReport.Works.Contains(i)).ToList();

            #endregion

            #region save data to file before send

            _worksPath.DeleteFile();
            var saveJson = JsonConvert.SerializeObject(reportData);
            _worksPath.WriteFile(saveJson);

            #endregion

            #region sync data to server

            try
            {
                foreach (var work in reportData.Works)
                {
                    var sendWork = work;
                    var sendReport = new ReportUser { AccessToken = Property.Key };

                    // get work lines
                    sendWork.Lines = GetWorklines(sendWork);
                    sendReport.Works.Add(sendWork);

                    // sync data to server
                    var rfsJson = JsonConvert.SerializeObject(sendReport);
                    using (var client = new VanctClient())
                        client.SyncDailyData(Property.Key, rfsJson);
                }
            }
            catch (Exception ex)
            {
                LoggingFactory.GetLogger().Log("[sync data to server]" + ex);
            }

            #endregion
        }

        /// <summary>
        /// Checks the online.
        /// </summary>
        /// <returns></returns>
        public bool CheckOnline()
        {
            try
            {
                using (var client = new VanctClient())
                {
                    return client.CheckOnlineStatus(Property.Key, false);
                }
            }
            catch (Exception ex)
            {
                LoggingFactory.GetLogger().Log("[CheckOnline]" + ex);
                return false;
            }
        }

        /// <summary>
        /// Finisheds the work.
        /// </summary>
        public bool CheckFinishedWorking()
        {
            int totalTableline;
            int totalTable1;
            int totalTable;

            using (var conn = new OleDbConnection(_connectionString))
            {
                conn.Open();

                using (var command = conn.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = "select count(MAQL) as totalLineCount from BAN";
                    totalTableline = (int)command.ExecuteScalar();
                }

                using (var command = conn.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = "select count(MABAN) as totalTable from [DANH MUC BAN] where DONG = 0 and GIOVAO is null and GIORA is null";
                    totalTable1 = (int)command.ExecuteScalar();
                }

                using (var command = conn.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = "select count(MABAN) as totalTable from [DANH MUC BAN] where DONG = 0";
                    totalTable = (int)command.ExecuteScalar();
                }
            }

            return totalTableline == 0 && totalTable == totalTable1;
        }

        /// <summary>
        /// Gets the total receiveds.
        /// </summary>
        /// <returns></returns>
        private IList<ReportWork> GetWorks(string dateString)
        {
            var list = new List<ReportWork>();

            using (var conn = new OleDbConnection(_connectionString))
            {
                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = "select CA, NHANVIEN, NGAY, sum(SOLUONG*DONGIA) as TOTAL " +
                                          "from [LUU KET QUA BAN HANG] " +
                                          "where Val(Format (NGAY, \"yyyymmdd\")) > " + dateString + " " +
                                          "group by CA, NGAY, NHANVIEN";
                    using (var dataReader = command.ExecuteReader())
                    {
                        if (dataReader == null) return new List<ReportWork>();
                        while (dataReader.Read())
                        {
                            list.Add(new ReportWork
                            {
                                Date = (DateTime)dataReader["NGAY"],
                                Total = (double)dataReader["TOTAL"],
                                WorkTime = dataReader["CA"] as string,
                                Username = dataReader["NHANVIEN"] as string,
                            });
                        }
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// Gets the worklines.
        /// </summary>
        /// <param name="work">The work.</param>
        /// <returns></returns>
        private IList<ReportWorkline> GetWorklines(ReportWork work)
        {
            var list = new List<ReportWorkline>();
            using (var conn = new OleDbConnection(_connectionString))
            {
                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = "select MAHG, TENHANG, NHOM, SUM(SOLUONG) as Amout, DONGIA, DVT " +
                                          "from [LUU KET QUA BAN HANG] " +
                                          "where CA = \"" + work.WorkTime + "\" and Val(Format (NGAY, \"yyyymmdd\")) = " +
                                          work.Date.ToString("yyyyMMdd") + " " +
                                          "group by MAHG, TENHANG, NHOM, DONGIA, DVT";

                    using (var dataReader = command.ExecuteReader())
                    {
                        if (dataReader == null) return new List<ReportWorkline>();
                        while (dataReader.Read())
                        {
                            var o = new ReportWorkline
                            {
                                Om = dataReader["DVT"] as string,
                                ProductGroup = dataReader["NHOM"] as string,
                                ProductName = dataReader["TENHANG"] as string,
                                ProductNo = dataReader["MAHG"] as string,
                                Amout = (double) dataReader["Amout"],
                            };

                            var price = dataReader["DONGIA"].ToString().ToInt64();
                            o.Price = price;
                            list.Add(o);
                        }

                        return list;
                    }
                }
            }
        }
    }
}
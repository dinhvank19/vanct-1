using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using Hulk.Shared.Log;
using Vanct.Dal.BO;
using Vanct.Dal.Dao;

namespace Vanct.Report
{
    public class ImportDataBiz
    {
        #region Properties

        private readonly ReportUser _user;
        private readonly string _filePath;
        private readonly GoogleDriverHelper _googleDriver;
        private readonly ReportDao _reportDao;
        private OleDbConnection _connection;
        private readonly string _connectionString;
        private readonly object _lock = new object();

        #endregion

        public ImportDataBiz(ReportUser user, string uploadFolderPath)
        {
            _user = user;
            _filePath = Path.Combine(uploadFolderPath, _user.Filename);
            _googleDriver = new GoogleDriverHelper();
            _reportDao = new ReportDao();
            _connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + _filePath
                                + ";Jet OLEDB:Database Password=" + _user.FilePassword + ";";
        }

        public bool Import()
        {
            lock (_lock)
            {
                try
                {
                    var files = _googleDriver.GetFiles();
                    var file = files.SingleOrDefault(i => i.OriginalFilename.ToLower().Equals(_user.Filename.ToLower()));
                    if (file == null) return false;

                    if (File.Exists(_filePath)
                        && file.ModifiedDate != null
                        && file.ModifiedDate.Value.CompareTo(_user.ModifiedDate) == 0)
                        return false;

                    var ok = _googleDriver.DownloadFile(file, _filePath);
                    if (ok)
                    {
                        _reportDao.UpdateModifiedDate(_user.Id, file.ModifiedDate != null ? file.ModifiedDate.Value : DateTime.Now);
                        _user.ModifiedDate = file.ModifiedDate != null ? file.ModifiedDate.Value : DateTime.Now;
                    }
                    else
                    {
                        LoggingFactory.GetLogger().Log("GET FILE FAILED !!! ");
                        return false;
                    }
                }
                catch (Exception exception)
                {
                    LoggingFactory.GetLogger().Log("GET FILE FAILED !!! " + exception);
                    return false;
                }

                return true;
            }
        }

        #region extract data

        /// <summary>
        /// Extracts the data.
        /// </summary>
        public void ExtractData()
        {
            if (!File.Exists(_filePath)) throw new Exception("Tập tin dữ liệu không tồn tại");

            using (_connection = new OleDbConnection(_connectionString))
            {
                _connection.Open();
                using (var command = _connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;

                    // danh muc ban
                    GetTable(command);

                    // danh muc ban' hand
                    GetTableline(command);

                    TotalCurrentReceived(command);
                }
            }
        }

        public void TotalReceived(OleDbCommand command)
        {
            command.CommandText = "select NGAY, sum(SOLUONG*DONGIA) as TOTAL from [LUU KET QUA BAN HANG] group by NGAY";
            using (var dataReader = command.ExecuteReader())
            {
                if (dataReader == null) return;
                _user.TotalReceiveds.Clear();
                while (dataReader.Read())
                {
                    _user.TotalReceiveds.Add(new ReportTotalReceived
                    {
                        Date = (DateTime) dataReader["NGAY"],
                        Total = (double) dataReader["TOTAL"],
                    });
                }
            }
        }

        public void TotalCurrentReceived(OleDbCommand command)
        {
            command.CommandText = "select sum(SOLUONG*DONGIA) as TOTAL from [BAN] where SOBAN is null";
            using (var dataReader = command.ExecuteReader())
            {
                if (dataReader == null) return;
                if (dataReader.Read())
                {
                    _user.CurrentReceived = new ReportTotalReceived
                    {
                        Date = DateTime.Now,
                        Total = (double)dataReader["TOTAL"],
                    };
                }
            }
        }

        public void TotalReceived()
        {
            if (!File.Exists(_filePath)) throw new Exception("Tập tin dữ liệu không tồn tại");

            using (var conn = new OleDbConnection(_connectionString))
            {
                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    //var sql = "select CA, NGAY, MAHG, TENHANG, NHOM, SOLUONG, DONGIA, DVT, QUAY, NHANVIEN from [LUU KET QUA BAN HANG]";
                    command.CommandText = "select NGAY, sum(SOLUONG*DONGIA) as TOTAL from [LUU KET QUA BAN HANG] group by NGAY";
                    using (var dataReader = command.ExecuteReader())
                    {
                        if (dataReader == null) return;
                        _user.TotalReceiveds.Clear();
                        while (dataReader.Read())
                        {
                            _user.TotalReceiveds.Add(new ReportTotalReceived
                            {
                                Date = (DateTime) dataReader["NGAY"],
                                Total = (double) dataReader["TOTAL"],
                            });
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the tableline.
        /// </summary>
        /// <param name="command">The command.</param>
        private void GetTableline(OleDbCommand command)
        {
            command.CommandText = "select SOBAN, MAHG, TENHANG, SOLUONG, DONGIA, DVT, NHOM from [BAN];";
            using (var dataReader = command.ExecuteReader())
            {
                if (dataReader == null) return;
                while (dataReader.Read())
                {
                    var tableNo = dataReader["SOBAN"] as string;
                    var table = _user.Tables.Get(tableNo);
                    if(table==null) continue;

                    table.Lines.AddNew(new ReportTableline
                    {
                        TableNo = tableNo,
                        ProductName = dataReader["TENHANG"] as string,
                        ProductNo = dataReader["MAHG"] as string,
                        Amout = (double)dataReader["SOLUONG"],
                        Price = (int)dataReader["DONGIA"],
                        Om = dataReader["DVT"] as string,
                        ProductGroup = dataReader["NHOM"] as string,
                    });
                }
            }
        }

        /// <summary>
        /// Gets the table.
        /// </summary>
        /// <param name="command">The command.</param>
        private void GetTable(OleDbCommand command)
        {
            command.CommandText = "select MABAN, COKHACH, CODOI, INBILL, STT, GIOVAO, GIORA, GIAM from [DANH MUC BAN] order by STT;";
            using (var dataReader = command.ExecuteReader())
            {
                if (dataReader == null) return;
                _user.Tables.Clear();

                while (dataReader.Read())
                {
                    _user.Tables.Add(new ReportTable
                    {
                        TableNo = dataReader["MABAN"] as string,
                        IsBusy = (bool) dataReader["COKHACH"],
                        HasChanged = (bool) dataReader["CODOI"],
                        IsPrinted = (bool) dataReader["INBILL"],
                        No = (int) dataReader["STT"],
                        InDate = dataReader["GIOVAO"] != DBNull.Value
                            ? (DateTime) dataReader["GIOVAO"]
                            : (DateTime?) null,
                        OutDate = dataReader["GIORA"] != DBNull.Value
                            ? (DateTime) dataReader["GIORA"]
                            : (DateTime?) null,
                        Discount = dataReader["GIAM"] != DBNull.Value
                            ? (int) dataReader["GIAM"]
                            : 0,
                    });
                }
            }

            command.CommandText = "select distinct MANV, XUAT from [BAN];";
            using (var dataReader = command.ExecuteReader())
            {
                if (dataReader == null) return;
                while (dataReader.Read())
                {
                    foreach (var table in _user.Tables)
                    {
                        table.EmployeeName = dataReader["MANV"] as string;
                        table.WorkingTime = dataReader["XUAT"] as string;
                    }
                }
            }
        }

        #endregion

        
    }
}
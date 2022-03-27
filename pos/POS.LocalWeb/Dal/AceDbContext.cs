using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using POS.LocalWeb.Properties;
using POS.Shared.Logging;

namespace POS.LocalWeb.Dal
{
    public class AceDbContext
    {
        public AceDbContext()
        {
            var dataFilename = Settings.Default.FileDatashare;
            if (!File.Exists(dataFilename))
                throw new Exception("Could not found file " + dataFilename);

            _connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + dataFilename
                                + ";Jet OLEDB:Database Password=" + Settings.Default.Password + ";";
        }

        public void Refresh()
        {
            CacheContext.Cacher.Remove(CacheContext.CacheTables);
            CacheContext.Cacher.Remove(CacheContext.CacheProducts);
            CacheContext.Cacher.Remove(CacheContext.CacheGroups);
            CacheContext.Cacher.Remove(CacheContext.CacheExGroups);
        }

        /// <summary>
        ///     Gets the tables. get data from mdb
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.Exception">
        ///     Can not ExecuteReader from table [DANH MUC BAN] - get all tables
        ///     or
        ///     Can not ExecuteReader from table [BAN] - push lines to tables
        /// </exception>
        public IList<ReportTable> GetTables()
        {
            //// read cache
            //if (Cacher.IsSet(CacheTables))
            //    return (IList<ReportTable>) Cacher.Get(CacheTables);

            var tables = new List<ReportTable>();
            using (_connection = new OleDbConnection(_connectionString))
            {
                _connection.Open();
                using (var command = _connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;

                    #region get all tables

                    command.CommandText =
                        "select MABAN, COKHACH, CODOI, INBILL, STT, GIOVAO, GIORA, GIAM, PHUCVU from [DANH MUC BAN] where [DONG] = 0 order by STT;";
                    using (var dataReader = command.ExecuteReader())
                    {
                        if (dataReader == null)
                            return new List<ReportTable>();

                        while (dataReader.Read())
                        {
                            tables.MergeItem(new ReportTable
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

                    command.CommandText =
                        "select MAQL, SOBAN, MAHG, TENHANG, SOLUONG, DONGIA, DVT, NHOM, DABAO, INCHUA, GHICHU, DateValue(NGAY) + TimeValue(GIO) as NGAYGIO from [BAN] where SOBAN is not null;";
                    using (var dataReader = command.ExecuteReader())
                    {
                        if (dataReader == null) return tables;
                        while (dataReader.Read())
                        {
                            var tableNo = dataReader["SOBAN"] as string;
                            var table = tables.Get(tableNo);
                            if (table == null) continue;

                            table.Lines.MergeItem(new ReportTableline
                            {
                                Id = dataReader["MAQL"] as string,
                                TableNo = tableNo,
                                ProductName = dataReader["TENHANG"] as string,
                                ProductNo = dataReader["MAHG"] as string,
                                Amout = (double)dataReader["SOLUONG"],
                                Price = (int)dataReader["DONGIA"],
                                Om = dataReader["DVT"] as string,
                                ProductGroup = dataReader["NHOM"] as string,
                                DaBao = (bool)dataReader["DABAO"],
                                InDate = dataReader["NGAYGIO"] != DBNull.Value
                                    ? (DateTime)dataReader["NGAYGIO"]
                                    : (DateTime?)null,
                                IsPrinted = (byte)dataReader["INCHUA"] == 0,
                                GhiChu = dataReader["GHICHU"] as string
                            });
                        }
                    }

                    #endregion

                    // save cache
                    //Cacher.Set(CacheTables, tables);
                    return tables;
                }
            }
        }

        public bool IsRefundable()
        {
            using (_connection = new OleDbConnection(_connectionString))
            {
                _connection.Open();
                using (var command = _connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    var isRefundable = false;
                    command.CommandText = "select CHO_PHEP_TRA from [TUY CHON];";
                    using (var dataReader = command.ExecuteReader())
                        if (dataReader.Read())
                            isRefundable = (bool)dataReader["CHO_PHEP_TRA"];

                    return isRefundable;
                }
            }
        }

        /// <summary>
        ///     Gets the table.
        /// </summary>
        /// <param name="tableno">The tableno.</param>
        /// <returns></returns>
        public ReportTable GetTable(string tableno)
        {
            ReportTable table = null;
            using (_connection = new OleDbConnection(_connectionString))
            {
                _connection.Open();
                using (var command = _connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;

                    #region get all tables

                    command.CommandText =
                        "select MABAN, COKHACH, CODOI, INBILL, STT, GIOVAO, GIORA, GIAM, PHUCVU from [DANH MUC BAN] where [DONG] = 0 and MABAN = '" +
                        tableno + "';";
                    using (var dataReader = command.ExecuteReader())
                    {
                        if (dataReader == null)
                            return null;

                        if (dataReader.Read())
                        {
                            table = new ReportTable
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
                            };
                        }
                    }

                    #endregion

                    #region push lines to tables

                    if (table == null)
                        return null;

                    command.CommandText =
                        "select MAQL, SOBAN, MAHG, TENHANG, SOLUONG, DONGIA, DVT, NHOM, DABAO, INCHUA, GHICHU, DateValue(NGAY) + TimeValue(GIO) as NGAYGIO from [BAN] where SOBAN is not null and SOBAN = '" +
                        tableno + "';";
                    using (var dataReader = command.ExecuteReader())
                    {
                        if (dataReader == null) return table;

                        while (dataReader.Read())
                        {
                            table.Lines.MergeItem(new ReportTableline
                            {
                                Id = dataReader["MAQL"] as string,
                                TableNo = tableno,
                                ProductName = dataReader["TENHANG"] as string,
                                ProductNo = dataReader["MAHG"] as string,
                                Amout = (double)dataReader["SOLUONG"],
                                Price = (int)dataReader["DONGIA"],
                                Om = dataReader["DVT"] as string,
                                ProductGroup = dataReader["NHOM"] as string,
                                DaBao = (bool)dataReader["DABAO"],
                                InDate = dataReader["NGAYGIO"] != DBNull.Value
                                    ? (DateTime)dataReader["NGAYGIO"]
                                    : (DateTime?)null,
                                IsPrinted = (byte)dataReader["INCHUA"] == 0,
                                GhiChu = dataReader["GHICHU"] as string
                            });
                        }
                    }

                    #endregion

                    // save cache
                    //Cacher.Set(CacheTables, tables);
                    return table;
                }
            }
        }

        /// <summary>
        /// Moves the order lines to new table.
        /// </summary>
        /// <param name="orderLineSelectedIDs">The order line selected i ds.</param>
        /// <param name="newTableId">The new table identifier.</param>
        public void MoveOrderLinesToNewTable(string orderLineSelectedIDs, string newTableId)
        {
            try
            {
                using (_connection = new OleDbConnection(_connectionString))
                {
                    _connection.Open();
                    using (var command = _connection.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText = string.Format("UPDATE [BAN] SET SOBAN = '{0}' WHERE MAQL IN({1})", newTableId, orderLineSelectedIDs);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingFactory.GetLogger().Debug(ex.ToString());
            }
        }

        /// <summary>
        /// Busies the table.
        /// </summary>
        /// <param name="tableNo">The table no.</param>
        public void BusyTable(string tableNo)
        {
            try
            {
                using (_connection = new OleDbConnection(_connectionString))
                {
                    _connection.Open();
                    using (var command = _connection.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText = string.Format("UPDATE [DANH MUC BAN] SET COKHACH=1, GIOVAO=now WHERE MABAN = '{0}' AND COKHACH=0 AND GIOVAO is null AND (select count(MAQL) from [BAN] where SOBAN = '{0}') > 0", tableNo);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingFactory.GetLogger().Debug(ex.ToString());
            }
        }

        /// <summary>
        /// Releases the table.
        /// </summary>
        /// <param name="tableNo">The table no.</param>
        public void ReleaseTable(string tableNo)
        {
            try
            {
                using (_connection = new OleDbConnection(_connectionString))
                {
                    _connection.Open();
                    using (var command = _connection.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText = string.Format("UPDATE [DANH MUC BAN] SET COKHACH=0, GIOVAO=null WHERE MABAN = '{0}' AND (select count(MAQL) from [BAN] where SOBAN = '{0}') = 0", tableNo);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingFactory.GetLogger().Debug(ex.ToString());
            }
        }

        /// <summary>
        ///     Deletes the orderline.
        /// </summary>
        /// <param name="lineId">The line identifier.</param>
        public void DeleteOrderline(string lineId)
        {
            try
            {
                using (_connection = new OleDbConnection(_connectionString))
                {
                    _connection.Open();
                    using (var command = _connection.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText = "delete from [BAN] where MAQL = '" + lineId + "'";
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingFactory.GetLogger().Debug(ex.ToString());
            }
        }

        /// <summary>
        ///     Gets the products.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.Exception">Không có danh mục bàn</exception>
        public IList<ReportProduct> GetProducts()
        {
            // read cache
            if (CacheContext.Cacher.IsSet(CacheContext.CacheProducts))
                return (IList<ReportProduct>)CacheContext.Cacher.Get(CacheContext.CacheProducts);

            var products = new List<ReportProduct>();
            using (_connection = new OleDbConnection(_connectionString))
            {
                _connection.Open();
                using (var command = _connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;

                    var defaultPriceColumnName = "DONGIA";
                    command.CommandText = "select DON_GIA_MAC_DINH from [TUY CHON];";
                    using (var dataReader = command.ExecuteReader())
                        if (dataReader.Read())
                            defaultPriceColumnName = dataReader["DON_GIA_MAC_DINH"] as string;

                    command.CommandText = $"select TENHANG, MAHG, NHOM, MUC, {defaultPriceColumnName}, DVT, HINH from [DANH MUC HANG];";
                    using (var dataReader = command.ExecuteReader())
                    {
                        if (dataReader == null)
                            throw new Exception("Không có danh mục hàng");

                        while (dataReader.Read())
                        {
                            products.MergeItem(new ReportProduct
                            {
                                Id = dataReader["MAHG"] as string,
                                Name = dataReader["TENHANG"] as string,
                                Group = dataReader["NHOM"] as string,
                                Price = (double)dataReader[defaultPriceColumnName],
                                Dvt = dataReader["DVT"] as string,
                                Muc = dataReader["MUC"] as string,
                                ImagePath = dataReader["HINH"] as string
                            });
                        }
                    }

                    products = products.OrderBy(i => i.Name).ToList();

                    // save cache
                    CacheContext.Cacher.Set(CacheContext.CacheProducts, products);
                    return products;
                }
            }
        }

        /// <summary>
        ///     Gets the product groups.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.Exception">Không có danh mục hàng</exception>
        public IList<ReportGroup> GetProductGroups()
        {
            // read cache
            if (CacheContext.Cacher.IsSet(CacheContext.CacheGroups))
                return (IList<ReportGroup>)CacheContext.Cacher.Get(CacheContext.CacheGroups);

            var groups = new List<ReportGroup>();
            using (_connection = new OleDbConnection(_connectionString))
            {
                _connection.Open();
                using (var command = _connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = "select NHOM, [IN] from [NHOM];";
                    using (var dataReader = command.ExecuteReader())
                    {
                        if (dataReader == null)
                            return new List<ReportGroup>();

                        while (dataReader.Read())
                        {
                            groups.MergeItem(new ReportGroup
                            {
                                Id = dataReader["NHOM"] as string,
                                Name = dataReader["NHOM"] as string,
                                IsPrint = (bool)dataReader["IN"]
                                //Printer = dataReader["MAYIN"] as string
                            });
                        }
                    }

                    // save cache
                    CacheContext.Cacher.Set(CacheContext.CacheGroups, groups);
                    return groups;
                }
            }
        }

        /// <summary>
        ///     Gets the product ex groups.
        /// </summary>
        /// <returns></returns>
        public IList<ReportExGroup> GetProductExGroups()
        {
            // read cache
            if (CacheContext.Cacher.IsSet(CacheContext.CacheExGroups))
                return (IList<ReportExGroup>)CacheContext.Cacher.Get(CacheContext.CacheExGroups);

            var groups = new List<ReportExGroup>();
            using (_connection = new OleDbConnection(_connectionString))
            {
                _connection.Open();
                using (var command = _connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = "select MUC, [IN], MAYIN, MAY_IN_TAM_TINH from [MUC];";
                    using (var dataReader = command.ExecuteReader())
                    {
                        if (dataReader == null)
                            return new List<ReportExGroup>();

                        while (dataReader.Read())
                        {
                            groups.Add(new ReportExGroup
                            {
                                Name = dataReader["MUC"] as string,
                                IsPrint = (bool)dataReader["IN"],
                                Printer = dataReader["MAYIN"] as string,
                                IsTemporaryPrint = (bool)dataReader["MAY_IN_TAM_TINH"]
                            });
                        }
                    }

                    // save cache
                    CacheContext.Cacher.Set(CacheContext.CacheExGroups, groups);
                    return groups;
                }
            }
        }

        /// <summary>
        ///     Inserts the orderline.
        /// </summary>
        /// <param name="line">The line.</param>
        /// <param name="tableNo">The table no.</param>
        public void InsertOrderline(ReportOrderline line, string tableNo)
        {
            try
            {
                using (_connection = new OleDbConnection(_connectionString))
                {
                    _connection.Open();
                    using (var command = _connection.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText =
                            "insert into [BAN] (MAQL, XUAT, SOBAN, MANV, NHOM, QUAY, MAHG, TENHANG, DVT, DONGIA, GIAGOC, SOLUONG, GHICHU, INCHUA, NGAY, GIO, DABAO, CON, SLC) " +
                            "values(@MAQL, 1, @SOBAN, @MANV, @NHOM, @QUAY, @MAHG, @TENHANG, @DVT, @DONGIA, 0, @SOLUONG, @GHICHU, 1, DateValue(now), TimeValue(now), 0, 0, 0)";

                        command.Parameters.Add("@MAQL", OleDbType.VarChar).Value = line.Maql;
                        command.Parameters.Add("@SOBAN", OleDbType.VarChar).Value = line.Soban;
                        command.Parameters.Add("@MANV", OleDbType.VarChar).Value = line.Manv;
                        command.Parameters.Add("@NHOM", OleDbType.VarChar).Value = line.Nhom;
                        command.Parameters.Add("@QUAY", OleDbType.VarChar).Value = line.Quay;
                        command.Parameters.Add("@MAHG", OleDbType.VarChar).Value = line.Mahg;
                        command.Parameters.Add("@TENHANG", OleDbType.VarChar).Value = line.Tenhang;
                        command.Parameters.Add("@DVT", OleDbType.VarChar).Value = line.Dvt;
                        command.Parameters.Add("@DONGIA", OleDbType.Numeric).Value = line.Dongia;
                        command.Parameters.Add("@SOLUONG", OleDbType.Numeric).Value = line.SoLuong;
                        command.Parameters.Add("@GHICHU", OleDbType.VarChar).Value = line.GhiChu;
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingFactory.GetLogger().Debug(ex.ToString());
            }
        }

        /// <summary>
        ///     Updates the order printed.
        /// </summary>
        /// <param name="lineIDs">The line i ds.</param>
        public void UpdateOrderPrinted(IList<string> lineIDs)
        {
            try
            {
                using (_connection = new OleDbConnection(_connectionString))
                {
                    _connection.Open();
                    using (var command = _connection.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText = "update [BAN] set INCHUA = 0 where MAQL in (" + string.Join(",", lineIDs) +
                                              ")";
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingFactory.GetLogger().Debug(ex.ToString());
            }
        }

        /// <summary>
        ///     Logins the specified username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Sai tài khoản hoặc mật khẩu</exception>
        public ReportUser Login(string username, string password)
        {
            using (_connection = new OleDbConnection(_connectionString))
            {
                _connection.Open();
                using (var command = _connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText =
                        string.Format("select NHANVIEN, MK, CHUCVU from [NHAN VIEN] where NHANVIEN = '{0}' and MK = '{1}'",
                            username, password);
                    using (var dataReader = command.ExecuteReader())
                    {
                        if (dataReader == null || !dataReader.Read())
                            throw new Exception("Sai tài khoản hoặc mật khẩu");

                        return new ReportUser
                        {
                            Username = dataReader["NHANVIEN"] as string,
                            ChucVu = dataReader["CHUCVU"] as string
                        };
                    }
                }
            }
        }

        /// <summary>
        ///     Sums the total received. count total received of current work
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.Exception">không có dữ liệu</exception>
        public double SumTotalReceived()
        {
            using (_connection = new OleDbConnection(_connectionString))
            {
                _connection.Open();
                using (var command = _connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText =
                        "select sum(SOLUONG*DONGIA) as TOTAL, XUAT from [BAN] where SOBAN is null group by XUAT";
                    using (var dataReader = command.ExecuteReader())
                    {
                        if (dataReader == null || !dataReader.Read())
                            return 0;

                        return (double)dataReader["TOTAL"];
                    }
                }
            }
        }

        #region Properties

        private readonly string _connectionString;
        private OleDbConnection _connection;

        #endregion
    }
}
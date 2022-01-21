using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using Hulk.Shared;

namespace NamvietSender
{
    public class AccessReader
    {
        private const string FilePassword = "26331";
        private const string PostionFilePath = "Logs/khongxoa.txt";
        private readonly string _connectionString;
        private OleDbConnection _connection;

        public AccessReader()
        {
            _connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + GetFilePath()
                                + ";Jet OLEDB:Database Password=" + FilePassword + ";";
        }

        public int GetPosition()
        {
            return File.ReadAllText(PostionFilePath).ToInt32();
        }

        public string GetFilePath()
        {
            return File.ReadAllText("Logs/DuongDan.txt");
        }

        public void SetPosition(int position)
        {
            File.WriteAllText(PostionFilePath, position.ToString());
        }

        public IList<DoanhSo> GetDoanhThu()
        {
            var startPosition = GetPosition();
            var list = new List<DoanhSo>();
            using (_connection = new OleDbConnection(_connectionString))
            {
                _connection.Open();
                using (var command = _connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText =
                        "SELECT ID, NGAY, MAHG, TENHANG, SOLUONG, DONGIA FROM [DOANH SO] WHERE ID > " + startPosition +
                        " ORDER BY ID";
                    using (var dataReader = command.ExecuteReader())
                    {
                        if (dataReader == null) return list;
                        while (dataReader.Read())
                        {
                            list.Add(new DoanhSo
                            {
                                Amount = (double) dataReader["SOLUONG"],
                                Code = dataReader["MAHG"] as string,
                                Name = dataReader["TENHANG"] as string,
                                CreatedAt = dataReader["NGAY"] != DBNull.Value
                                    ? (DateTime) dataReader["NGAY"]
                                    : (DateTime?) null,
                                ExternalId = (int) dataReader["ID"],
                                Price = (double) dataReader["DONGIA"]
                            });
                        }
                    }
                }
            }

            return list;
        }
    }
}
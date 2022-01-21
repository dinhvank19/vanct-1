using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using Hulk.Shared;
using Vanct.Dal.BO;
using Vanct.Dal.Entities;

namespace Vanct.Dal.Dao
{
    public class ReportDao : BaseDao<RUser, VanctEntities>
    {
        #region For GUI

        /// <summary>
        /// Inserts the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public RUser Insert(RUser user)
        {
            using (var db = new VanctEntities())
            {
                if (db.RUsers.Count(i => i.Username.Equals(user.Username)) > 0)
                    throw new Exception(string.Format("Tên hoặc tài khoản '{0}' đã được sử dụng trước đó", user.Username));

                return Create(user);
            }
        }

        /// <summary>
        /// Updates the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public bool Update(RUser user)
        {
            return Edit(user, i => i.Id == user.Id);
        }

        /// <summary>
        /// Logins the specified username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">
        /// Tài khoản hoặc mật khẩu không đúng
        /// or
        /// Tài khoản của bạn đã bị khóa.
        /// or
        /// </exception>
        public ReportUser Login(string username, string password)
        {
            //password = password.ToMd5();
            using (var db = new VanctEntities())
            {
                var user = db.RUsers.SingleOrDefault(i => i.Username.Equals(username) && i.Password.Equals(password));
                if (user == null)
                    throw new Exception("Tài khoản hoặc mật khẩu không đúng");

                if (!user.IsActive)
                    throw new Exception("Tài khoản của bạn đã bị khóa.");

                if (DateTime.Now.CompareTo(user.ExpiredDate) > 0)
                    throw new Exception(string.Format("Tài khoản của bạn đã hết hạn ngày {0}",
                        user.ExpiredDate.ToString("dd-MM-yyyy")));

                return user.CopyTo(new ReportUser());
            }
        }

        /// <summary>
        /// Changes the password.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="newPass">The new pass.</param>
        /// <exception cref="System.Exception">Mật khẩu hiện tại không đúng</exception>
        public void ChangePassword(string username, string password, string newPass)
        {
            using (var db = new VanctEntities())
            {
                var user = db.RUsers.SingleOrDefault(i => i.Username.Equals(username) && i.Password.Equals(password));
                if (user == null)
                    throw new Exception("Mật khẩu hiện tại không đúng");

                user.Password = newPass;
                db.SaveChanges();
            }
        }

        #endregion

        #region For Work

        public void AddWork(ReportUser data)
        {
            using (var db = new VanctEntities())
            {
                var user = db.RUsers.SingleOrDefault(i => i.AccessToken.Equals(data.AccessToken));
                if(user == null) return;

                foreach (var work in data.Works)
                {
                    var daily = db.ReportDailies.Add(new ReportDaily
                    {
                        Username = work.Username,
                        WorkTime = work.WorkTime.Equals("S")
                            ? "Sáng"
                            : work.WorkTime.Equals("C")
                                ? "Chiều"
                                : work.WorkTime.Equals("T") ? "Tối" : "None",
                        Date = work.Date,
                        RUserId = user.Id,
                        Total = work.Total,
                    });
                    db.SaveChanges();

                    foreach (var line in work.Lines)
                    {
                        db.ReportDailylines.Add(new ReportDailyline
                        {
                            Amout = line.Amout,
                            Om = line.Om,
                            Price = line.Price,
                            ProductGroup = line.ProductGroup,
                            ProductName = line.ProductName,
                            ProductNo = line.ProductNo,
                            DailyId = daily.Id,
                        });
                        db.SaveChanges();
                    }
                }
            }
        }

        public IList<ReportWork> GetWorks(ReportFilterParamter filter)
        {
            using (var db = new VanctEntities())
            {
                using (DbConnection connection = db.Database.Connection)
                {
                    connection.Open();
                    using (DbCommand command = db.Database.Connection.CreateCommand())
                    {
                        command.CommandText = "[dbo].[sp_Report_GetWorks]";
                        command.CommandType = CommandType.StoredProcedure;
                        AddParameter(command, DbType.Int32, "@userId", filter.UserId);
                        AddParameter(command, DbType.Date, "@dateFrom", filter.DateFrom.ToDate("dd-MM-yyyy"));
                        AddParameter(command, DbType.Date, "@dateTo", filter.DateTo.ToDate("dd-MM-yyyy"));
                        using (var dataReader = command.ExecuteReader())
                        {
                            var list = new List<ReportWork>();
                            while (dataReader.Read())
                                list.Add(new ReportWork
                                {
                                    Id = (int) dataReader["Id"],
                                    WorkTime = dataReader["WorkTime"] as string,
                                    Username = dataReader["Username"] as string,
                                    Total = (double) dataReader["Total"],
                                    Date = (DateTime) dataReader["Date"],
                                    RUserId = (int) dataReader["RUserId"],
                                });

                            return list;
                        }
                    }
                }
            }
        }

        public IList<ReportWorkline> GetWorklines(ReportFilterParamter filter)
        {
            using (var db = new VanctEntities())
            {
                using (DbConnection connection = db.Database.Connection)
                {
                    connection.Open();
                    using (DbCommand command = db.Database.Connection.CreateCommand())
                    {
                        command.CommandText = "[dbo].[sp_Report_GetWorkDetails]";
                        command.CommandType = CommandType.StoredProcedure;
                        AddParameter(command, DbType.Int32, "@userId", filter.UserId);
                        AddParameter(command, DbType.Date, "@dateFrom", filter.DateFrom.ToDate("dd-MM-yyyy"));
                        AddParameter(command, DbType.Date, "@dateTo", filter.DateTo.ToDate("dd-MM-yyyy"));
                        using (var dataReader = command.ExecuteReader())
                        {
                            var list = new List<ReportWorkline>();
                            while (dataReader.Read())
                                list.Add(new ReportWorkline
                                {
                                    Om = dataReader["Om"] as string,
                                    ProductGroup = dataReader["ProductGroup"] as string,
                                    ProductName = dataReader["ProductName"] as string,
                                    ProductNo = dataReader["ProductNo"] as string,
                                    Amout = (double) dataReader["Amout"],
                                    Price = (double) dataReader["Price"],
                                    Total = (double)dataReader["Total"],
                                });

                            return list;
                        }
                    }
                }
            }
        }

        #endregion


    }
}

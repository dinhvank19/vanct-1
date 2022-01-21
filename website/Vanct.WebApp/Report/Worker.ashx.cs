using System;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using Hulk.Shared;
using Hulk.Shared.Log;
using Newtonsoft.Json;
using Vanct.Dal.BO;
using Vanct.WebApp.AppCode;

namespace Vanct.WebApp.Report
{
    public class Ban
    {
        public int No { get; set; }
        public string TableNo { get; set; }
        public double Total { get; set; }
        public bool IsPrinted { get; set; }
        public bool IsBusy { get; set; }
        public string Moment { set; get; }
        public string Servicer { set; get; }
        public bool Processed { set; get; }
    }

    /// <summary>
    /// Summary description for Worker
    /// </summary>
    public class Worker : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            var action = context.Request.QueryString["action"];
            if (action.Equals("LoadReport"))
                LoadReport(context);
            if (action.Equals("Login"))
                Login(context);
            if (action.Equals("GetTableline"))
                GetTableline(context);
            if (action.Equals("Logout"))
                Logout(context);
            if (action.Equals("ReportFilter"))
                ReportFilter(context);
            if (action.Equals("ChangePassword"))
                ChangePassword(context);
            
        }

        protected void ChangePassword(HttpContext context)
        {
            var user = VanctContext.ReportUser;
            if (user == null)
            {
                Error(context, "Vui lòng đăng nhập lại");
                return;
            }

            try
            {
                var password = context.Request.Form["Pass"];
                var newPass = context.Request.Form["NewPass"];
                VanctContext.ReportDao.ChangePassword(user.Username, password, newPass);
                context.Response.Write(JsonConvert.SerializeObject(new { Result = false, Message = "Đã đổi mật khẩu thành công" }));
            }
            catch (Exception exception)
            {
                Error(context, exception.Message);
            }
        }

        protected void ReportFilter(HttpContext context)
        {
            var user = VanctContext.ReportUser;
            if (user == null)
            {
                Error(context, "Vui lòng đăng nhập lại");
                return;
            }

            var filterParamter = context.Request.Form["filterParamter"];
            var parameter = JsonConvert.DeserializeObject<ReportFilterParamter>(filterParamter);
            parameter.UserId = user.Id;

            if (parameter.WorkMode)
            {
                var works = VanctContext.ReportDao.GetWorks(parameter);
                if (works.Count == 0)
                {
                    Error(context, "Không có dữ liệu");
                    return;
                }
                if (works.Count > 120)
                {
                    Error(context, "Quá nhiều dữ liệu, bạn vui lòng chọn ngày hợp lý");
                    return;
                }
                var dates = works.Select(i => i.Date.ToString("dd-MM-yyyy")).Distinct();
                var sum = works.Sum(i => i.Total);
                context.Response.Write(JsonConvert.SerializeObject(
                    new
                    {
                        Result = true,
                        List = dates.Select(date => new
                        {
                            D = date,
                            T = works.Where(i => i.Date.ToString("dd-MM-yyyy").Equals(date))
                                .Sum(i => i.Total)
                                .ToString(CultureInfo.InvariantCulture)
                                .ToMoneyString(),
                            L = works.Where(i => i.Date.ToString("dd-MM-yyyy").Equals(date)).Select(i => new
                            {
                                i.Id,
                                U = i.Username,
                                W = i.WorkTime,
                                T = i.Total.ToString(CultureInfo.InvariantCulture).ToMoneyString()
                            })
                        }),
                        Total = sum.ToString(CultureInfo.InvariantCulture).ToMoneyString()
                    }));
            }
            else
            {
                var lines = VanctContext.ReportDao.GetWorklines(parameter);
                if (lines.Count == 0)
                {
                    Error(context, "Không có dữ liệu");
                    return;
                }
                var sum = lines.Sum(i => i.Total);
                var groups = lines.Select(i => i.ProductGroup).Distinct();
                context.Response.Write(JsonConvert.SerializeObject(
                    new
                    {
                        Result = true,
                        List = groups.Select(group => new
                        {
                            G = group,
                            L = lines.Where(i => i.ProductGroup.Equals(group)).Select(i => new
                            {
                                N = i.ProductName,
                                P = i.Price.ToString(CultureInfo.InvariantCulture).ToMoneyString(),
                                A = i.Amout,
                                T = i.Total.ToString(CultureInfo.InvariantCulture).ToMoneyString()
                            }),
                            T = lines.Where(i => i.ProductGroup.Equals(group))
                                .Sum(i => i.Total)
                                .ToString(CultureInfo.InvariantCulture)
                                .ToMoneyString()
                        }),
                        Total = sum.ToString(CultureInfo.InvariantCulture).ToMoneyString()
                    }));
            }

        }

        /// <summary>
        /// Loads the report.
        /// </summary>
        /// <param name="context">The context.</param>
        protected void LoadReport(HttpContext context)
        {
            var user = VanctContext.ReportUser;
            if (user == null)
            {
                Error(context, "Vui lòng đăng nhập lại");
                return;
            }

            try
            {
                user = ReportHelper.Get(user.AccessToken);
                if (user == null)
                {
                    VanctContext.ReportUser = null;
                    Error(context, "Vui lòng đăng nhập lại");
                    return;
                }

                var list = (from i in user.Tables
                    let processed = i.Lines.Count == i.Lines.Count(l => l.DaBao) && i.Lines.Count > 0
                    select new Ban
                    {
                        No = i.No,
                        TableNo = i.TableNo,
                        Total = i.Total,
                        IsPrinted = i.IsPrinted,
                        IsBusy = i.IsBusy,
                        Servicer = i.Servicer,
                        Processed = processed,
                        Moment = i.InDate == null ? string.Empty : i.InDate.Value.TimeEscalationToString()
                    }).ToList();

                var totalSoldOut = user.Tables.Sum(i => i.Total);
                var countTableBusy = list.Count(i => i.IsBusy);
                var countTableInProgress = list.Count(i => i.IsBusy && !i.Processed && i.Total > 0);

                context.Response.Write(JsonConvert.SerializeObject(
                    new
                    {
                        Result = true,
                        Tables = list,
                        user.Working.Total,
                        user.Name,
                        TotalSoldOut = totalSoldOut,
                        CountTableBusy = countTableBusy,
                        CountTableInProgress = countTableInProgress
                    }));
            }
            catch (Exception exception)
            {
                VanctContext.ReportUser = null;
                Error(context, exception.Message);
                LoggingFactory.GetLogger().Log(exception.ToString());
            }
        }

        protected void GetTableline(HttpContext context)
        {
            try
            {
                var user = VanctContext.ReportUser;
                var tableNo = context.Request.Form["tableNo"];
                user = ReportHelper.Get(user.AccessToken);
                if (user == null)
                {
                    VanctContext.ReportUser = null;
                    Error(context, "Vui lòng đăng nhập lại");
                    return;
                }

                var table = user.Tables.Get(tableNo);
                table.Lines = table.Lines
                    .Where(i => i.Amout > 0)
                    .ToList();
                context.Response.Write(JsonConvert.SerializeObject(new { Result = true, Table = table }));
            }
            catch (Exception exception)
            {
                VanctContext.ReportUser = null;
                Error(context, exception.Message);
                LoggingFactory.GetLogger().Log(exception.ToString());
            }
        }

        protected void Login(HttpContext context)
        {
            var username = context.Request.Form["username"];
            var password = context.Request.Form["password"];

            if (username.Length == 0)
            {
                Error(context, "Vui lòng nhập tài khoản");
                return;
            }

            if (password.Length == 0)
            {
                Error(context, "Vui lòng nhập mật khẩu");
                return;
            }

            try
            {
                var user = VanctContext.ReportDao.Login(username, password);
                VanctContext.ReportUser = user;
                if (!ReportHelper.CheckOnlineStatus(user.AccessToken))
                    ReportHelper.AddOrUpdate(user);
                context.Response.Write(JsonConvert.SerializeObject(new { Result = true }));
            }
            catch (Exception exception)
            {
                Error(context, exception.Message);
                LoggingFactory.GetLogger().Log(exception.ToString());
            }
        }

        /// <summary>
        /// Logouts the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        protected void Logout(HttpContext context)
        {
            var user = VanctContext.ReportUser;
            ReportHelper.Remove(user.AccessToken);
            VanctContext.ReportUser = null;
            context.Response.Write(JsonConvert.SerializeObject(new { Result = true }));
        }

        /// <summary>
        /// Errors the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="message">The message.</param>
        protected void Error(HttpContext context, string message)
        {
            context.Response.Write(JsonConvert.SerializeObject(new { Result = false, Message = message }));
        }

        public bool IsReusable => false;
    }
}
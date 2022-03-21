using System.Web;
using System.Web.SessionState;
using POS.LocalWeb.Dal;
using POS.Shared;

namespace POS.LocalWeb.AppCode
{
    public static class PosContext
    {
        private static HttpSessionState Session => HttpContext.Current.Session;

        public static HttpServerUtility Server => HttpContext.Current.Server;

        public static string RequestTableNo => HttpContext.Current.Request["no"];

        public static bool RequestChangeTable => HttpContext.Current.Request["changeTable"] == "true";

        public static bool IsRefund => HttpContext.Current.Request["refund"] == "true";

        public static ReportUser User => HttpContext.Current.User.Identity.Name.JsonTextTo<ReportUser>();

        public static string UploadFolder => Server.MapPath("~/UploadManage");

        public static string IconOrList
        {
            set { Session["IconOrList"] = value; }
            get { return Session["IconOrList"] as string; }
        }
    }
}
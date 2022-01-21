using System.IO;
using System.Web;
using System.Web.SessionState;
using POS.BizRunner;
using POS.BizRunner.Interfaces;
using POS.Dal;
using POS.Dal.Enums;
using POS.Shared;

namespace POS.WebApp.AppCode
{
    public static class PosContext
    {

        #region Properties

        private static HttpSessionState Session => HttpContext.Current.Session;

        public static HttpServerUtility Server => HttpContext.Current.Server;

        #endregion

        #region Inprogress Sessions

        public static RecordUser User
        {
            get
            {
                if (Session["User"] != null)
                    return (RecordUser)Session["User"];

                var user = HttpContext.Current.User.Identity.Name.JsonTextTo<RecordUser>();

                // if is admin then no need get session
                if (user.UserType.ToEnum<UserType>() == UserType.Administrator)
                {
                    Session["User"] = user;
                    return (RecordUser)Session["User"];
                }

                // get session
                if (user.Session == null)
                    user.Session = BizSession.GetInprogress(user.Id);

                Session["User"] = user;
                return (RecordUser)Session["User"];
            }
        }

        #endregion

        #region URL Parameters

        public static int RequestId => HttpContext.Current.Request["id"] != null ? int.Parse(HttpContext.Current.Request["id"]) : 0;

        #endregion

        #region Application directories

        public static string UploadFolder => Server.MapPath("~/UploadManage");

        public static string ContentFolder => Server.MapPath("~/Content");

        #endregion

        #region Translate

        public static Translater Translater
        {
            get
            {
                if (Session["LetterTranslaterObject"] == null)
                    Session["LetterTranslaterObject"] = new Translater(Path.Combine(ContentFolder, Shared.Properties.Settings.Default.LanguageFilePath));
                return (Translater)Session["LetterTranslaterObject"];
            }
            set { Session["LetterTranslaterObject"] = value; }
        }
        public static string Translate(this string text)
        {
            return Translater.Translate(text);
        }

        #endregion

        #region Business Rules

        private static ISessionBiz _sessionBiz;
        public static ISessionBiz BizSession => _sessionBiz ?? (_sessionBiz = new SessionBiz());

        private static IOrderBiz _orderBiz;
        public static IOrderBiz BizOrder => _orderBiz ?? (_orderBiz = new OrderBiz());

        #endregion










    }
}
using System.IO;
using System.Web;
using System.Web.SessionState;
using Vanct.Dal.BO;
using Vanct.Dal.Entities;
using Hulk.Shared;
using Vanct.Dal.Dao;

namespace Vanct.WebApp.AppCode
{
    public class VanctContext
    {
        #region Properties

        public static HttpSessionState Session
        {
            get { return HttpContext.Current.Session; }
        }

        public static HttpRequest Request
        {
            get { return HttpContext.Current.Request; }
        }

        public static HttpResponse Response
        {
            get { return HttpContext.Current.Response; }
        }

        public static HttpServerUtility Server
        {
            get { return HttpContext.Current.Server; }
        }

        #endregion

        #region Login Session

        public static BaseUser LoginUser
        {
            get { return Session["LoginUser"] as BaseUser; }
            set { Session["LoginUser"] = value; }
        }

        public static ReportUser ReportUser
        {
            get { return Session["ReportUser"] as ReportUser; }
            set { Session["ReportUser"] = value; }
        }

        #endregion

        #region Translate

        public static Translater Translater
        {
            get
            {
                if (Session["LetterTranslaterObject"] == null)
                    Session["LetterTranslaterObject"] = new Translater(Path.Combine(AppPath.ApplicationFolder, Hulk.Shared.Properties.Settings.Default.LanguageFilePath));
                return (Translater)Session["LetterTranslaterObject"];
            }
            set { Session["LetterTranslaterObject"] = value; }
        }

        #endregion

        #region DAOs

        private static ReportDao _reportDao;
        public static ReportDao ReportDao
        {
            get { return _reportDao ?? (_reportDao = new ReportDao()); }
        }

        private static PosCompanyDao _posCompanyDao;
        public static PosCompanyDao PosCompanyDao
        {
            get { return _posCompanyDao ?? (_posCompanyDao = new PosCompanyDao()); }
        }

        private static PosUserDao _posUserDao;
        public static PosUserDao PosUserDao
        {
            get { return _posUserDao ?? (_posUserDao = new PosUserDao()); }
        }

        private static SupportOnlineDao _supportOnlineDao;
        public static SupportOnlineDao SupportOnlineDao
        {
            get { return _supportOnlineDao ?? (_supportOnlineDao = new SupportOnlineDao()); }
        }

        private static PostLinkDao _postLinkDao;
        public static PostLinkDao PostLinkDao
        {
            get { return _postLinkDao ?? (_postLinkDao = new PostLinkDao()); }
        }

        private static ProductDao _productDao;
        public static ProductDao ProductDao
        {
            get { return _productDao ?? (_productDao = new ProductDao()); }
        }

        private static ProductTypeDao _productTypeDao;
        public static ProductTypeDao ProductTypeDao
        {
            get { return _productTypeDao ?? (_productTypeDao = new ProductTypeDao()); }
        }

        private static ProductTypeGroupDao _productTypeGroupDao;
        public static ProductTypeGroupDao ProductTypeGroupDao
        {
            get { return _productTypeGroupDao ?? (_productTypeGroupDao = new ProductTypeGroupDao()); }
        }

        private static TopicDao _topicDao;
        public static TopicDao TopicDao
        {
            get { return _topicDao ?? (_topicDao = new TopicDao()); }
        }

        private static BaseUserDao _baseUserDao;
        public static BaseUserDao BaseUserDao
        {
            get { return _baseUserDao ?? (_baseUserDao = new BaseUserDao()); }
        }

        private static HomeGalleryDao _homeGalleryDao;
        public static HomeGalleryDao HomeGalleryDao
        {
            get { return _homeGalleryDao ?? (_homeGalleryDao = new HomeGalleryDao()); }
        }

        private static BaseFileDao _baseFileDao;
        public static BaseFileDao FileDao
        {
            get { return _baseFileDao ?? (_baseFileDao = new BaseFileDao()); }
        }

        #endregion

        #region Web Parameters

        public static int RequestId
        {
            get { return Request["Id"] != null ? int.Parse(Request["Id"]) : 0; }
        }

        public static int CompanyId
        {
            get { return Request["cid"] != null ? int.Parse(Request["cid"]) : 0; }
        }

        public static string RequestName
        {
            get { return HttpContext.Current.Request["n"]; }
        }

        #endregion


    }
}
using System.Web;
using System.Web.SessionState;

namespace Namviet.Helpers
{
    public class ApplicationHelper
    {
        #region Properties

        public static HttpSessionState Session => HttpContext.Current.Session;

        public static HttpRequest Request => HttpContext.Current.Request;

        public static HttpResponse Response => HttpContext.Current.Response;

        public static HttpServerUtility Server => HttpContext.Current.Server;

        #endregion 
    }
}
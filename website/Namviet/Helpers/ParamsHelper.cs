using System.Web;

namespace Namviet.Helpers
{
    public class ParamsHelper : ApplicationHelper
    {
        public static string FromDate => HttpContext.Current.Request["from"];
        public static string ToDate => HttpContext.Current.Request["to"];
    }
}
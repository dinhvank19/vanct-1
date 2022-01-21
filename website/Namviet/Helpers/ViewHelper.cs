using Hulk.Shared;

namespace Namviet.Helpers
{
    public class ViewHelper : ApplicationHelper
    {
        public static string DisplayMoney(object o)
        {
            if (o == null) return string.Empty;
            if (o.IsDecimal() && o.ToDecimal() == 0) return string.Empty;
            return $"{o:0,0}";
        }

        public static string DisplayDate(object o)
        {
            if (string.IsNullOrEmpty(o?.ToString())) return string.Empty;
            return $"{o:dd-MM-yyyy}";
        }

        public static string DisplayDateTime(object o)
        {
            if (string.IsNullOrEmpty(o?.ToString())) return string.Empty;
            return $"{o:dd-MM-yyyy HH:mm}";
        }
    }
}
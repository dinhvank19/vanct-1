using System;

namespace Hulk.Shared
{
    public static class DateUtil
    {
        public static string Display(DateTime datetime)
        {
            return datetime.ToString("dd-MM-yyyy");
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POS.LocalWeb.Dal
{
    public class ReportTotal
    {
        public DateTime Date { set; get; }
        public double Total { set; get; }
        public string WorkTime { set; get; }
    }
}
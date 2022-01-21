using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using POS.Shared;

namespace POS.LocalWeb.Dal
{
    public class ReportTableline
    {
        public string Id { get; set; }
        public string TableNo { get; set; }
        public string ProductNo { set; get; }
        public string GhiChu { set; get; }
        public string ProductName { set; get; }
        public string ProductGroup { set; get; }
        public double Amout { set; get; }
        public double Price { set; get; }
        public double Total => Amout*Price;
        public string Om { set; get; }
        public DateTime? InDate { get; set; }
        public bool DaBao { set; get; }
        public bool IsPrinted { set; get; }
        public string PriceText => Price.ToMoney();
        public string TotalText => Total.ToMoney();
        public string Moment => InDate == null
            ? string.Empty
            : InDate.Value.TimeEscalationToString();

        public string Css => DaBao ? "processed" : "";
        public string PrintCss => IsPrinted ? "printed" : "deleting";
    }
}
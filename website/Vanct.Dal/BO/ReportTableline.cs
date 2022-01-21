using System;
using Hulk.Shared;

namespace Vanct.Dal.BO
{
    public class ReportTableline
    {
        public string Id { get; set; }
        public string TableNo { get; set; }
        public string ProductNo { set; get; }
        public string ProductName { set; get; }
        public string ProductGroup { set; get; }
        public double Amout { set; get; }
        public double Price { set; get; }
        public string Om { set; get; }
        public DateTime? InDate { get; set; }
        public bool DaBao { set; get; }

        public string Moment
        {
            get
            {
                return InDate == null
                    ? string.Empty
                    : InDate.Value.TimeEscalationToString();
            }
        }
    }
}

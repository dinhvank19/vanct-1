using System;
using System.Collections.Generic;
using System.Linq;
using Hulk.Shared;

namespace Vanct.Dal.BO
{
    public class ReportTable
    {
        public int No { get; set; }
        public string TableNo { get; set; }
        public bool IsBusy { get; set; }
        public bool IsPrinted { get; set; }
        public bool HasChanged { get; set; }
        public DateTime? InDate { get; set; }
        public DateTime? OutDate { get; set; }
        public IList<ReportTableline> Lines { get; set; }
        public double Discount { set; get; }
        public double Total
        {
            get { return Lines.Sum(i => i.Amout*i.Price) - Discount; }
        }
        public string Moment
        {
            get
            {
                return InDate == null
                    ? string.Empty
                    : InDate.Value.TimeEscalationToString();
            }
        }
        public string Servicer { get; set; }

        public ReportTable()
        {
            Lines = new List<ReportTableline>();
        }
    }
}
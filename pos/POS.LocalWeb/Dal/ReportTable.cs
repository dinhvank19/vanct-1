using System;
using System.Collections.Generic;
using System.Linq;
using POS.Shared;

namespace POS.LocalWeb.Dal
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
            get { return Lines.Sum(i => i.Amout * i.Price) - Discount; }
        }
        public string TotalText => Total.ToMoney();
        public string Moment => InDate == null
            ? string.Empty
            : InDate.Value.TimeEscalationToString();

        public bool Processed
        {
            get { return Lines.Count == Lines.Count(l => l.DaBao) && Lines.Count > 0; }
        }

        public string Css => string.Format("{0} {1} {2}"
            , IsPrinted ? "printed " : ""
            , Total > 0 && Processed && !IsPrinted ? "processed " : ""
            , IsBusy ? "busy " : "");

        public string Servicer { get; set; }

        public ReportTable()
        {
            Lines = new List<ReportTableline>();
        }
    }
}
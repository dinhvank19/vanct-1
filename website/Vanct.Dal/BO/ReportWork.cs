using System;
using System.Collections.Generic;

namespace Vanct.Dal.BO
{
    public class ReportWork
    {
        public string WorkTime { set; get; }
        public string Username { set; get; }
        public DateTime Date { set; get; }
        public double Total { set; get; }

        public int Id { get; set; }
        public int RUserId { get; set; }

        public IList<ReportWorkline> Lines { set; get; }

        public ReportWork()
        {
            Lines = new List<ReportWorkline>();
        }
    }
}
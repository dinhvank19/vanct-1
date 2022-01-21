using System;
using System.Collections.Generic;

namespace Vanct.Dal.BO
{
    public class ReportUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime ExpiredDate { get; set; }
        public bool IsActive { get; set; }
        public string AccessToken { get; set; }
        public bool IsOnline { get; set; }

        public DateTime LastChanged { get; set; }

        public double CurrentTotal { get; set; }
        public IList<ReportTable> Tables { get; set; }
        public IList<ReportWork> Works { set; get; }
        public ReportWork Working { set; get; }

        public ReportUser()
        {
            Tables = new List<ReportTable>();
            Works = new List<ReportWork>();
            Working = new ReportWork
            {
                Date = DateTime.Now,
                Total = 0
            };
        }
    }
}
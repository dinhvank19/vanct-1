using System;
using System.Collections.Generic;
using System.Linq;
using Hulk.Shared;
using Vanct.Dal.BO;

namespace Vanct.Report
{
    public static class ReportHelper
    {
        public static void MergeItem(this IList<ReportTable> list, ReportTable item)
        {
            var update = list.SingleOrDefault(i => i.TableNo.Equals(item.TableNo));
            if (update == null)
            {
                list.Add(item);
            }
            else
            {
                update.EmployeeName = item.EmployeeName;
                update.HasChanged = item.HasChanged;
                update.InDate = item.InDate;
                update.IsBusy = item.IsBusy;
                update.IsPrinted = item.IsPrinted;
                update.OutDate = item.OutDate;
                update.WorkingTime = item.WorkingTime;
                //update.No = item.No;
            }
        }

        public static void AddNew(this IList<ReportTableline> list, ReportTableline item)
        {
            var o = list.SingleOrDefault(i => i.ProductName.ToLower().Equals(item.ProductName.ToLower()));
            if (o == null)
            {
                list.Add(item);
                return;
            }

            o.Amout = o.Amout + item.Amout;
        }

        public static ReportTable Get(this IList<ReportTable> list, string tableNo)
        {
            return list.SingleOrDefault(i => i.TableNo.Equals(tableNo));
        }

        public static ReportTotalReceived GetTotalOfToday(this IList<ReportTotalReceived> list)
        {
            var now = DateTime.Now.ToString("yyyy-MM-dd");
            return list.SingleOrDefault(i => i.Date.ToString("yyyy-MM-dd").Equals(now));
        }
    }
}
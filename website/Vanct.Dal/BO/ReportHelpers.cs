using System.Collections.Generic;
using System.Linq;

namespace Vanct.Dal.BO
{
    public static class ReportHelpers
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
                update.HasChanged = item.HasChanged;
                update.IsBusy = item.IsBusy;
                update.IsPrinted = item.IsPrinted;

                update.InDate = item.InDate;
                update.OutDate = item.OutDate;

                update.Discount = item.Discount;
                update.Lines = item.Lines;

                update.Servicer = item.Servicer;
            }
        }

        public static void MergeItem(this IList<ReportTableline> list, ReportTableline item)
        {
            //var o = list.SingleOrDefault(i => i.ProductName.ToLower().Equals(item.ProductName.ToLower()));
            //if (o == null)
            //{
            //    list.Add(item);
            //    return;
            //}
            //o.Amout = o.Amout + item.Amout;
            //o.DaBao = item.DaBao;
            list.Add(item);
        }

        public static ReportTable Get(this IList<ReportTable> list, string tableNo)
        {
            return list.SingleOrDefault(i => i.TableNo.Equals(tableNo));
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using POS.Shared;

namespace POS.LocalWeb.Dal
{
    public static class BoHelper
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

        public static ReportTable Get(this IList<ReportTable> list, string tableNo)
        {
            return list.SingleOrDefault(i => i.TableNo.Equals(tableNo));
        }

        public static void MergeItem(this IList<ReportTableline> list, ReportTableline item)
        {
            list.Add(item);
        }

        public static void MergeItem(this IList<ReportGroup> list, ReportGroup item)
        {
            list.Add(item);
        }

        public static ReportGroup Get(this IList<ReportGroup> list, string groupId)
        {
            return list.SingleOrDefault(i => i.Id.Equals(groupId));
        }

        public static void MergeItem(this IList<ReportProduct> list, ReportProduct item)
        {
            item.SearchName = (item.Name.ClearVietKey(" ").ToLower() + " " + item.Name + " " + item.Id).ToLower();
            list.Add(item);
        }

        public static IList<ReportProduct> ByGroup(this IList<ReportProduct> list, string groupId)
        {
            return list.Where(i => i.Group.Equals(groupId)).ToList();
        }

        public static ReportProduct Get(this IList<ReportProduct> list, string productId)
        {
            return list.SingleOrDefault(i => i.Id.Equals(productId));
        }
    }
}
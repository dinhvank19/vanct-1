using System.Collections.Generic;
using System.Linq;
using POS.Dal.Enums;
using POS.Shared;
using POS.Shared.Caching;

namespace POS.Dal
{
    public static class RecordManager
    {
        public static RecordProduct Get(this IList<RecordProduct> list, int id)
        {
            return list.SingleOrDefault(i => i.Id == id);
        }

        public static void Merge(this IList<RecordProduct> list, RecordProduct record)
        {
            var mergeItem = list.SingleOrDefault(i => i.Id == record.Id);
            if (mergeItem != null)
            {
                mergeItem.Name = record.Name;
                mergeItem.Description = record.Description;
                mergeItem.Discount = record.Discount;
                mergeItem.ValidStatus = record.ValidStatus;
                mergeItem.ChangedDate = record.ChangedDate;
                mergeItem.ChangedBy = record.ChangedBy;
                mergeItem.GroupId = record.GroupId;
                mergeItem.Price = record.Price;
                mergeItem.ProductOm = record.ProductOm;
            }
            else
            {
                list.Add(record);
            }
        }

        public static RecordTable Get(this IList<RecordTable> list, int id)
        {
            return list.SingleOrDefault(i => i.Id == id);
        }

        public static void Merge(this IList<RecordTable> list, RecordTable record)
        {
            var mergeItem = list.SingleOrDefault(i => i.Id == record.Id);
            if (mergeItem != null)
            {
                mergeItem.ValidStatus = record.ValidStatus;
                mergeItem.Name = record.Name;
                mergeItem.AreaId = record.AreaId;
            }
            else
            {
                list.Add(record);
            }
        }

        public static RecordOrderline GetPendingLine(this IList<RecordOrderline> list, int productId)
        {
            return list.SingleOrDefault(i => i.ProductId == productId && i.LineStatus.ToEnum<LineStatus>() == LineStatus.Pending);
        }

        public static void Remove(this IList<RecordOrderline> list, int productId)
        {
            var removing = list.SingleOrDefault(i => i.ProductId == productId);
            if (removing != null)
                list.Remove(removing);
        }

        #region Cacher

        private static ICacheProvider _cacheProvider;
        public static ICacheProvider Cacher => _cacheProvider ?? (_cacheProvider = new DefaultCacheProvider());

        #endregion
    }
}
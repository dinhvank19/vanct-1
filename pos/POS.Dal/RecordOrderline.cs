using System;
using System.Collections.Generic;
using System.Linq;
using POS.Dal.Entities;
using POS.Shared;

namespace POS.Dal
{
    public class RecordOrderline
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public double Price { get; set; }
        public double Amount { get; set; }
        public int Discount { get; set; }
        public string LineStatus { get; set; }
        public DateTime CreatedDate { get; set; }

        public RecordProduct Product { get; set; }

        public double Total => (Price - Price*Discount/100)*Amount;

        public string TotalText => string.Format("{0:0,0}", Total);
        public string PriceText => string.Format("{0:0,0}", Price);

        #region Statics

        /// <summary>
        /// Gets the orderlines.
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <returns></returns>
        public static IList<RecordOrderline> GetOrderlines(int orderId)
        {
            using (var db = new POSEntities())
            {
                var lines = db.Orderlines.Where(i => i.OrderId == orderId).ToList();

                var list = new List<RecordOrderline>();
                foreach (var item in lines.Select(line => line.Clone(new RecordOrderline())))
                {
                    item.Product = RecordProduct.Get(item.ProductId);
                    list.Add(item);
                }

                return list;
            }
        }

        #endregion

    }
}
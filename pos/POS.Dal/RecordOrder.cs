using System;
using System.Collections.Generic;
using System.Linq;
using POS.Dal.Entities;
using POS.Dal.Enums;
using POS.Shared;

namespace POS.Dal
{
    public class RecordOrder
    {
        public RecordOrder()
        {
            Lines = new List<RecordOrderline>();
        }

        public int Id { get; set; }
        public string OrderNr { get; set; }
        public string Description { get; set; }
        public int SessionId { get; set; }
        public int TableId { get; set; }
        public string OrderStatus { get; set; }
        public string OrderType { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime CreatedDate { get; set; }
        public double? Deposit { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public int Discount { get; set; }
        public bool IsPrinted { get; set; }
        public double ServiceCost { get; set; }

        public string StartTimeText => string.Format("Cách đây {0}'", StartTime.TimeEscalationToString());
        public double TotalOrder => TotalLines - (TotalLines * Discount / 100);
        public string TotalOrderText => string.Format("{0:0,0}", TotalOrder);
        public double TotalLines => Lines.Sum(i => i.Total);
        public string TotalLinesText => string.Format("{0:0,0}", TotalLines);

        public string DisplayCss
        {
            get
            {
                var processed = Lines.Count(i => i.LineStatus.Equals(LineStatus.Printed.ToString())) == Lines.Count;
                var status = OrderStatus.ToEnum<OrderStatus>();
                if (status == Enums.OrderStatus.Printed && processed)
                    return "printed-processed";

                if (status == Enums.OrderStatus.Printed && !processed)
                    return "printed";

                if (status == Enums.OrderStatus.Pending && processed)
                    return "processed";

                return "pending";
            }
        }


        public IList<RecordOrderline> Lines { get; set; }
        public RecordSession Session { get; set; }
        public RecordTable Table { get; set; }

        #region Statics

        public static RecordOrder Get(int orderId)
        {
            using (var db = new POSEntities())
            {
                var record = db.Orders.SingleOrDefault(i => i.Id == orderId);
                if (record == null)
                    return null;

                var order = record.Clone(new RecordOrder());
                order.Lines = RecordOrderline.GetOrderlines(order.Id);

                return order;
            }
        }

        #endregion

        #region Methods

        public void Save()
        {
            var isUpdate = Id > 0;

            using (var db = new POSEntities())
            {
                if (isUpdate)
                {
                    var record = db.Orders.Single(i => i.Id == Id);
                    record.Discount = Discount;
                    record.IsPrinted = IsPrinted;
                    record.OrderStatus = OrderStatus;
                    record.EndTime = EndTime;
                    record.TotalLines = TotalLines;
                    record.TotalOrder = TotalOrder;
                    db.SaveChanges();
                }
                else
                {
                    var record = new Order
                    {
                        CreatedDate = CreatedDate,
                        Discount = Discount,
                        IsPrinted = IsPrinted,
                        OrderStatus = OrderStatus,
                        OrderType = OrderType,
                        SessionId = SessionId,
                        StartTime = StartTime,
                        TableId = TableId,
                        TotalLines = TotalLines,
                        TotalOrder = TotalOrder,
                        OrderNr = "none"
                    };

                    db.Orders.Add(record);
                    db.SaveChanges();
                    Id = record.Id;

                    OrderNr = string.Format("{0,8:D8}", record.Id);
                    record.OrderNr = OrderNr;
                    db.SaveChanges();
                }

                foreach (var line in Lines)
                {
                    // insert new line
                    if (line.Id == 0)
                    {
                        line.OrderId = Id;
                        var inserted = line.Clone(new Orderline());
                        db.Orderlines.Add(inserted);
                        db.SaveChanges();
                        line.Id = inserted.Id;
                        continue;
                    }

                    // get existing line & update
                    var updated = db.Orderlines.Single(i => i.Id == line.Id);
                    updated.Price = line.Price;
                    updated.Amount = line.Amount;
                    updated.Discount = line.Discount;
                    updated.LineStatus = line.LineStatus;
                    db.SaveChanges();
                }
            }
        }

        public void Update()
        {

        }

        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using POS.Dal.Entities;
using POS.Shared;

namespace POS.Dal
{
    public class RecordSession
    {
        public RecordSession()
        {
            Orders = new List<RecordOrder>();
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public string WorkingTime { get; set; }
        public string SessionStatus { get; set; }
        public double Total { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ClosedDate { get; set; }

        public RecordUser User { get; set; }
        public IList<RecordOrder> Orders { get; set; }

        #region Statics

        /// <summary>
        ///     Gets the inproges session.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public static RecordSession GetInprogress(int userId)
        {
            using (var db = new POSEntities())
            {
                var record = db.Sessions.SingleOrDefault(i =>
                    i.UserId == userId
                    && !i.SessionStatus.Equals(Enums.SessionStatus.Completed.ToString()));

                return record == null ? null : record.Clone(new RecordSession());
            }
        }

        #endregion

        #region Methods

        public void Close()
        {
            using (var db = new POSEntities())
            {
                var record = db.Sessions.SingleOrDefault(i =>
                    i.UserId == UserId
                    && i.SessionStatus.Equals(Enums.SessionStatus.Inprogress.ToString())
                    && i.Id == Id);

                if (record == null) return;

                SessionStatus = Enums.SessionStatus.Completed.ToString();
                record.SessionStatus = SessionStatus;
                record.ClosedDate = DateTime.Now;

                if (db.Orders.Count(i =>
                    i.SessionId == record.Id
                    && i.OrderStatus.Equals(Enums.OrderStatus.Completed.ToString())) > 0)
                {
                    record.Total = db.Orders.Where(i =>
                    i.SessionId == record.Id
                    && i.OrderStatus.Equals(Enums.OrderStatus.Completed.ToString()))
                    .Sum(i => i.TotalOrder);
                }

                db.SaveChanges();
            }
        }

        public void Insert()
        {
            using (var db = new POSEntities())
            {
                var record = new Session
                {
                    CreatedDate = CreatedDate,
                    SessionStatus = SessionStatus,
                    UserId = UserId,
                    WorkingTime = WorkingTime
                };
                db.Sessions.Add(record);
                db.SaveChanges();

                Id = record.Id;
            }
        }

        #endregion
    }
}
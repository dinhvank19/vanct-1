using System;
using System.Collections.Generic;
using System.Linq;
using POS.Dal.Entities;
using POS.Dal.Enums;
using POS.Shared;

namespace POS.Dal
{
    public class RecordTable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AreaId { get; set; }
        public int? ActiveOrderId { get; set; }
        public bool IsBusy { get; set; }
        public string ValidStatus { get; set; }

        public string AreaName { get; set; }
        public RecordTableArea Area { get; set; }
        public RecordOrder Order { get; set; }

        #region Statics

        /// <summary>
        ///     Gets the specified record identifier.
        /// </summary>
        /// <param name="recordId">The record identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">NotFoundData</exception>
        public static RecordTable Get(int recordId)
        {
            using (var db = new POSEntities())
            {
                var record = db.TablePos.SingleOrDefault(i => i.Id == recordId);
                if (record == null)
                    throw new Exception("NotFoundData");

                return record.Clone(new RecordTable());
            }
        }

        /// <summary>
        ///     Alls this instance.
        /// </summary>
        /// <returns></returns>
        public static IList<RecordTable> All(ValidStatus valid = Enums.ValidStatus.None)
        {
            using (var db = new POSEntities())
            {
                return db.TablePos
                    .Where(i => valid == Enums.ValidStatus.None || i.ValidStatus.Equals(valid.ToString()))
                    .Select(i => new RecordTable
                    {
                        Name = i.Name,
                        Id = i.Id,
                        ValidStatus = i.ValidStatus,
                        AreaName = i.TableArea.Name,
                        AreaId = i.AreaId,
                        IsBusy = i.IsBusy,
                        ActiveOrderId = i.ActiveOrderId
                    })
                    .ToList();
            }
        }

        #endregion

        #region Methods

        public void Save()
        {
            using (var db = new POSEntities())
            {
                var record = db.TablePos.SingleOrDefault(i => i.Id == Id);
                if (record == null)
                    throw new Exception("NotFoundData");

                record.IsBusy = IsBusy;
                record.ActiveOrderId = ActiveOrderId;
                db.SaveChanges();
            }
        }

        public void Update()
        {
            using (var db = new POSEntities())
            {
                var record = db.TablePos.SingleOrDefault(i => i.Id == Id);
                if (record == null)
                    throw new Exception("NotFoundData");

                record.ValidStatus = ValidStatus;
                record.Name = Name;
                record.AreaId = AreaId;

                db.SaveChanges();
                MergeToCache(this);
            }
        }

        public void Insert()
        {
            using (var db = new POSEntities())
            {
                var record = new TablePos
                {
                    Name = Name,
                    ValidStatus = ValidStatus,
                    ActiveOrderId = null,
                    AreaId = AreaId,
                    IsBusy = false
                };
                db.TablePos.Add(record);
                db.SaveChanges();

                Id = record.Id;
                MergeToCache(this);
            }
        }

        #endregion

        private void MergeToCache(RecordTable record)
        {
            var cacheTables = "Tables";
            if (!RecordManager.Cacher.IsSet(cacheTables)) return;
            var data = (IList<RecordTable>)RecordManager.Cacher.Get(cacheTables);
            data.Merge(record);
        }
    }
}
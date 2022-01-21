using System;
using System.Collections.Generic;
using System.Linq;
using POS.Dal.Entities;
using POS.Dal.Enums;
using POS.Shared;

namespace POS.Dal
{
    public class RecordTableArea
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PrintersName { get; set; }
        public int Discount { get; set; }
        public string ValidStatus { get; set; }

        #region Statics

        /// <summary>
        ///     Gets the specified record identifier.
        /// </summary>
        /// <param name="recordId">The record identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">NotFoundData</exception>
        public static RecordTableArea Get(int recordId)
        {
            using (var db = new POSEntities())
            {
                var record = db.TableAreas.SingleOrDefault(i => i.Id == recordId);
                if (record == null)
                    throw new Exception("NotFoundData");

                return record.Clone(new RecordTableArea());
            }
        }

        /// <summary>
        ///     Alls this instance.
        /// </summary>
        /// <returns></returns>
        public static IList<RecordTableArea> All(ValidStatus valid = Enums.ValidStatus.None)
        {
            using (var db = new POSEntities())
            {
                return db.TableAreas
                    .Where(i => valid == Enums.ValidStatus.None || i.ValidStatus.Equals(valid.ToString()))
                    .Select(i => new RecordTableArea
                    {
                        Description = i.Description,
                        PrintersName = i.PrintersName,
                        Name = i.Name,
                        Id = i.Id,
                        Discount = i.Discount,
                        ValidStatus = i.ValidStatus
                    })
                    .ToList();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Updates this instance.
        /// </summary>
        /// <exception cref="System.Exception">NotFoundData</exception>
        public void Update()
        {
            using (var db = new POSEntities())
            {
                var record = db.TableAreas.SingleOrDefault(i => i.Id == Id);
                if (record == null)
                    throw new Exception("NotFoundData");

                record.ValidStatus = ValidStatus;
                record.Name = Name;
                record.Description = Description;
                record.Discount = Discount;
                record.PrintersName = PrintersName;

                db.SaveChanges();
                CleanCache();
            }
        }

        /// <summary>
        ///     Inserts this instance.
        /// </summary>
        /// <exception cref="System.Exception">NotFoundData</exception>
        public void Insert()
        {
            using (var db = new POSEntities())
            {
                var record = new TableArea
                {
                    Description = Description,
                    Discount = Discount,
                    Name = Name,
                    ValidStatus = ValidStatus,
                    PrintersName = PrintersName
                };
                db.TableAreas.Add(record);
                db.SaveChanges();

                Id = record.Id;
                CleanCache();
            }
        }

        #endregion

        private void CleanCache()
        {
            var cacheAreas = "CacheAreas";
            // refresh cache
            if (RecordManager.Cacher.IsSet(cacheAreas))
                RecordManager.Cacher.Remove(cacheAreas);
        }
    }
}
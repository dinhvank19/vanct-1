using System;
using System.Collections.Generic;
using System.Linq;
using POS.Dal.Entities;
using POS.Dal.Enums;
using POS.Shared;

namespace POS.Dal
{
    public class RecordProductGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PrintersName { get; set; }
        public string ValidStatus { get; set; }

        #region Statics

        /// <summary>
        ///     Gets the specified record identifier.
        /// </summary>
        /// <param name="recordId">The record identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">NotFoundData</exception>
        public static RecordProductGroup Get(int recordId)
        {
            using (var db = new POSEntities())
            {
                var record = db.ProductGroups.SingleOrDefault(i => i.Id == recordId);
                if (record == null)
                    throw new Exception("NotFoundData");

                return record.Clone(new RecordProductGroup());
            }
        }

        /// <summary>
        ///     Alls this instance.
        /// </summary>
        /// <returns></returns>
        public static IList<RecordProductGroup> All(ValidStatus valid = Enums.ValidStatus.None)
        {
            using (var db = new POSEntities())
            {
                return db.ProductGroups
                    .Where(i => valid == Enums.ValidStatus.None || i.ValidStatus.Equals(valid.ToString()))
                    .Select(i => new RecordProductGroup
                    {
                        Name = i.Name,
                        Id = i.Id,
                        ValidStatus = i.ValidStatus,
                        Description = i.Description,
                        PrintersName = i.PrintersName
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
                var record = db.ProductGroups.SingleOrDefault(i => i.Id == Id);
                if (record == null)
                    throw new Exception("NotFoundData");

                record.ValidStatus = ValidStatus;
                record.Name = Name;
                record.Description = Description;
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
                var record = new ProductGroup
                {
                    Name = Name,
                    ValidStatus = ValidStatus,
                    Description = Description,
                    PrintersName = PrintersName
                };
                db.ProductGroups.Add(record);
                db.SaveChanges();

                Id = record.Id;
                CleanCache();
            }
        }

        #endregion

        private void CleanCache()
        {
            var cacheAreas = "ProductGroups";
            // refresh cache
            if (RecordManager.Cacher.IsSet(cacheAreas))
                RecordManager.Cacher.Remove(cacheAreas);
        }
    }
}
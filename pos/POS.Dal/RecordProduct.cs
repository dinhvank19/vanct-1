using System;
using System.Collections.Generic;
using System.Linq;
using POS.Dal.Entities;
using POS.Dal.Enums;
using POS.Shared;

namespace POS.Dal
{
    public class RecordProduct
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int GroupId { get; set; }
        public string ProductOm { get; set; }
        public double Price { get; set; }
        public int Discount { get; set; }
        public string ImageUrl { get; set; }
        public string ValidStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ChangedDate { get; set; }
        public int? ChangedBy { get; set; }

        public string GroupName { get; set; }
        public string PriceText => string.Format("{0:0,0}", Price);
        public RecordProductGroup Group { get; set; }

        #region Statics

        /// <summary>
        ///     Gets the specified record identifier.
        /// </summary>
        /// <param name="recordId">The record identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">NotFoundData</exception>
        public static RecordProduct Get(int recordId)
        {
            using (var db = new POSEntities())
            {
                var record = db.Products.SingleOrDefault(i => i.Id == recordId);
                if (record == null)
                    throw new Exception("NotFoundData");

                return record.Clone(new RecordProduct());
            }
        }

        /// <summary>
        ///     Alls this instance.
        /// </summary>
        /// <returns></returns>
        public static IList<RecordProduct> All(ValidStatus valid = Enums.ValidStatus.None)
        {
            using (var db = new POSEntities())
            {
                return db.Products
                    .Where(i => valid == Enums.ValidStatus.None || i.ValidStatus.Equals(valid.ToString()))
                    .Select(i => new RecordProduct
                    {
                        Description = i.Description,
                        Name = i.Name,
                        Id = i.Id,
                        Discount = i.Discount,
                        ValidStatus = i.ValidStatus,
                        ProductOm = i.ProductOm,
                        Price = i.Price,
                        ChangedBy = i.ChangedBy,
                        ChangedDate = i.ChangedDate,
                        ImageUrl = i.ImageUrl,
                        GroupId = i.GroupId,
                        CreatedBy = i.CreatedBy,
                        CreatedDate = i.CreatedDate,
                        GroupName = i.ProductGroup.Name
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
                var record = db.Products.SingleOrDefault(i => i.Id == Id);
                if (record == null)
                    throw new Exception("NotFoundData");

                record.Name = Name;
                record.Description = Description;
                record.Discount = Discount;
                record.ValidStatus = ValidStatus;
                record.ChangedDate = DateTime.Now;
                record.ChangedBy = ChangedBy;
                record.GroupId = GroupId;
                record.Price = Price;
                record.ProductOm = ProductOm;

                db.SaveChanges();
                MergeToCache(this);
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
                var record = new Product
                {
                    Description = Description,
                    Discount = Discount,
                    Name = Name,
                    ValidStatus = ValidStatus,
                    CreatedBy = CreatedBy,
                    CreatedDate = DateTime.Now,
                    GroupId = GroupId,
                    Price = Price,
                    ProductOm = ProductOm,
                    ImageUrl = null,
                    ChangedBy = null,
                    ChangedDate = null
                };
                db.Products.Add(record);
                db.SaveChanges();

                Id = record.Id;
                MergeToCache(this);
            }
        }

        #endregion

        private void MergeToCache(RecordProduct record)
        {
            var cacheProducts = "Products";
            if (!RecordManager.Cacher.IsSet(cacheProducts)) return;
            var data = (IList<RecordProduct>)RecordManager.Cacher.Get(cacheProducts);
            data.Merge(record);
        }
    }
}
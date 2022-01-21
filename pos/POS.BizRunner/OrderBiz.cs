using System.Collections.Generic;
using POS.BizRunner.Interfaces;
using POS.Dal;
using POS.Dal.Enums;

namespace POS.BizRunner
{
    public class OrderBiz : IOrderBiz
    {
        #region Cacher

        const string CacheAreas = "CacheAreas";
        const string CacheTables = "Tables";
        const string CacheProductGroups = "ProductGroups";
        const string CacheProducts = "Products";

        #endregion

        #region Tables

        /// <summary>
        /// Gets the tables.
        /// </summary>
        /// <returns></returns>
        public IList<RecordTable> GetTables()
        {
            if (RecordManager.Cacher.IsSet(CacheTables))
                return (IList<RecordTable>)RecordManager.Cacher.Get(CacheTables);

            // get list of tables
            var list = RecordTable.All(ValidStatus.Active);

            // get active order on tables if isBusy
            foreach (var table in list)
            {
                if (table.ActiveOrderId != null)
                {
                    table.Order = RecordOrder.Get(table.ActiveOrderId.Value);
                }
            }

            // write cache
            RecordManager.Cacher.Set(CacheTables, list);

            return (IList<RecordTable>)RecordManager.Cacher.Get(CacheTables);

        }

        /// <summary>
        /// Gets the areas.
        /// </summary>
        /// <returns></returns>
        public IList<RecordTableArea> GetAreas()
        {
            if (RecordManager.Cacher.IsSet(CacheAreas))
                return (IList<RecordTableArea>)RecordManager.Cacher.Get(CacheAreas);

            var list = RecordTableArea.All(ValidStatus.Active);
            RecordManager.Cacher.Set(CacheAreas, list);

            return (IList<RecordTableArea>)RecordManager.Cacher.Get(CacheAreas);
        }

        #endregion


        #region Products

        /// <summary>
        /// Gets the products.
        /// </summary>
        /// <returns></returns>
        public IList<RecordProduct> GetProducts()
        {
            if (RecordManager.Cacher.IsSet(CacheProducts))
                return (IList<RecordProduct>)RecordManager.Cacher.Get(CacheProducts);

            var list = RecordProduct.All(ValidStatus.Active);
            RecordManager.Cacher.Set(CacheProducts, list);

            return (IList<RecordProduct>)RecordManager.Cacher.Get(CacheProducts);
        }

        /// <summary>
        /// Gets the product groups.
        /// </summary>
        /// <returns></returns>
        public IList<RecordProductGroup> GetProductGroups()
        {
            if (RecordManager.Cacher.IsSet(CacheProductGroups))
                return (IList<RecordProductGroup>)RecordManager.Cacher.Get(CacheProductGroups);

            var list = RecordProductGroup.All(ValidStatus.Active);
            RecordManager.Cacher.Set(CacheProductGroups, list);

            return (IList<RecordProductGroup>)RecordManager.Cacher.Get(CacheProductGroups);
        }

        #endregion


    }
}
using System.Collections.Generic;
using POS.Dal;

namespace POS.BizRunner.Interfaces
{
    public interface IOrderBiz
    {
        #region Tables

        IList<RecordTable> GetTables();

        IList<RecordTableArea> GetAreas();

        #endregion

        #region Products

        IList<RecordProduct> GetProducts();

        IList<RecordProductGroup> GetProductGroups();

        #endregion
    }
}
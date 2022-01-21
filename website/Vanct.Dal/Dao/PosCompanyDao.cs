using System.Linq;
using Vanct.Dal.Entities;

namespace Vanct.Dal.Dao
{
    public class PosCompanyDao : BaseDao<PosCompany, VanctEntities>
    {
        /// <summary>
        ///     Updates the specified record.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <returns></returns>
        public bool Update(PosCompany record)
        {
            using (var db = new VanctEntities())
            {
                var update = db.PosCompanies.SingleOrDefault(i => i.Id == record.Id);
                if (update == null)
                    return false;

                update.Name = record.Name;
                update.Description = record.Description;
                update.IsActive = record.IsActive;
                db.SaveChanges();
                return true;
            }
        }

        /// <summary>
        ///     Inserts the specified record.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <returns></returns>
        public PosCompany Insert(PosCompany record)
        {
            return Create(record);
        }
    }
}
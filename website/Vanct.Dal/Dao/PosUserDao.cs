using System;
using System.Linq;
using Hulk.Shared;
using Vanct.Dal.BO;
using Vanct.Dal.Entities;

namespace Vanct.Dal.Dao
{
    public class PosUserDao : BaseDao<PosUser, VanctEntities>
    {
        /// <summary>
        /// Inserts the specified record.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <returns></returns>
        public PosUser Insert(PosUser record)
        {
            return Create(record);
        }

        /// <summary>
        /// Logins the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="deviceUuid">The device UUID.</param>
        /// <param name="deviceName">Name of the device.</param>
        /// <returns></returns>
        public PosDevice Login(string key, string deviceUuid, string deviceName)
        {
            using (var db = new VanctEntities())
            {
                var update = db.PosUsers.SingleOrDefault(i => i.UniqueId.Equals(key));
                if (update == null || update.IsError)
                    return null;

                if (!string.IsNullOrEmpty(update.DeviceUuid) && !update.DeviceUuid.Equals(deviceUuid))
                {
                    update.IsError = true;
                    db.SaveChanges();
                    return null;
                }

                update.DeviceName = deviceName;
                update.DeviceUuid = deviceUuid;
                update.LastLoginDate = DateTime.Now;

                if (update.FirstLoginDate != null)
                    update.FirstLoginDate = update.LastLoginDate;

                db.SaveChanges();

                return update.CopyTo(new PosDevice());
            }
        }
    }
}
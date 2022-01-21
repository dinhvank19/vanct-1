using System;
using System.Runtime.Caching;
using Vanct.Dal.BO;

namespace Vanct.WebApp.Report
{
    public sealed class ReportHelper
    {
        #region Cache

        private static readonly ObjectCache Cache = MemoryCache.Default;

        private static readonly CacheItemPolicy Policy = new CacheItemPolicy
        {
            Priority = CacheItemPriority.Default,
            SlidingExpiration = new TimeSpan(0, 0, 60 * 10),
            RemovedCallback = ItemRemovedCallback,
        };

        private static void ItemRemovedCallback(CacheEntryRemovedArguments arguments)
        {

        }

        #endregion

        /// <summary>
        /// Adds the or update.
        /// </summary>
        /// <param name="user">The user.</param>
        public static void AddOrUpdate(ReportUser user)
        {
            var existedUser = Cache.Get(user.AccessToken) as ReportUser;
            if (existedUser == null)
            {
                user.LastChanged = DateTime.Now;
                Cache.Add(user.AccessToken, user, Policy);
            }
            else
            {
                existedUser.Working = user.Working;
                foreach (var table in user.Tables)
                {
                    existedUser.Tables.MergeItem(table);
                }

                //existedUser.Works = user.Works;
                existedUser.LastChanged = DateTime.Now;
            }
        }

        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static ReportUser Get(string key)
        {
            var existedUser = Cache.Get(key) as ReportUser;
            if (existedUser == null) return null;

            existedUser.LastChanged = DateTime.Now;
            return existedUser;
        }

        /// <summary>
        /// Removes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        public static void Remove(string key)
        {
            Cache.Remove(key);
        }

        /// <summary>
        /// Checks the online status.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static bool CheckOnlineStatus(string key)
        {
            var user = Cache.Get(key) as ReportUser;
            if (user == null) return false;
            return user.LastChanged.AddMinutes(5).CompareTo(DateTime.Now) > 0;
        }
    }
}
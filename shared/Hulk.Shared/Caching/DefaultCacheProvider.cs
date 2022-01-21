using System;
using System.Runtime.Caching;
using Hulk.Shared.Properties;

namespace Hulk.Shared.Caching
{
    public class DefaultCacheProvider : ICacheProvider
    {
        #region Properties

        private static ObjectCache Cache
        {
            get { return MemoryCache.Default; }
        }

        private static string _prefix = "POLYCLAIM";

        public DefaultCacheProvider(string prefix)
        {
            _prefix = prefix;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Gets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public object Get(string key)
        {
            return Cache[string.Format("{0}_{1}", _prefix, key)];
        }

        /// <summary>
        ///     Sets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="data">The data.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Set(string key, object data)
        {
            Remove(key);
            Set(key, data, Settings.Default.DefaultCacheTime);
        }

        /// <summary>
        ///     Sets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="data">The data.</param>
        /// <param name="cacheTime">The cache time (minutes).</param>
        public void Set(string key, object data, int cacheTime)
        {
            lock (key)
            {
                var policy = new CacheItemPolicy
                {
                    Priority = CacheItemPriority.Default,
                    SlidingExpiration = new TimeSpan(0, cacheTime, 0),
                };

                Cache.Add(new CacheItem(string.Format("{0}_{1}", _prefix, key), data), policy);
            }
        }

        /// <summary>
        ///     Sets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="data">The data.</param>
        /// <param name="expirationDate">The expiration date.</param>
        public void Set(string key, object data, DateTime expirationDate)
        {
            lock (key)
            {
                var policy = new CacheItemPolicy
                {
                    Priority = CacheItemPriority.Default,
                    AbsoluteExpiration = expirationDate,
                };
                Cache.Add(new CacheItem(string.Format("{0}_{1}", _prefix, key), data), policy);
            }
        }

        /// <summary>
        ///     Determines whether the specified key is set.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public bool IsSet(string key)
        {
            return (Cache[string.Format("{0}_{1}", _prefix, key)] != null);
        }

        /// <summary>
        ///     Invalidates the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        public void Remove(string key)
        {
            lock (key)
            {
                if (IsSet(key)) Cache.Remove(string.Format("{0}_{1}", _prefix, key));
            }
        }

        #endregion
    }
}
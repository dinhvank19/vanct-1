using System;

namespace Hulk.Shared.Caching
{
    public interface ICacheProvider
    {
        /// <summary>
        ///     Gets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        object Get(string key);

        /// <summary>
        ///     Sets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="data">The data.</param>
        void Set(string key, object data);

        /// <summary>
        ///     Sets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="data">The data.</param>
        /// <param name="cacheTime">The cache time (minutes).</param>
        void Set(string key, object data, int cacheTime);

        /// <summary>
        ///     Sets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="data">The data.</param>
        /// <param name="expirationDate">The expiration date.</param>
        void Set(string key, object data, DateTime expirationDate);

        /// <summary>
        ///     Determines whether the specified key is set.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        bool IsSet(string key);

        /// <summary>
        ///     Invalidates the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        void Remove(string key);
    }
}
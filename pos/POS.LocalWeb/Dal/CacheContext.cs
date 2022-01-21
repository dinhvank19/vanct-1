using POS.Shared.Caching;

namespace POS.LocalWeb.Dal
{
    public class CacheContext
    {
        private static ICacheProvider _cacheProvider;
        public static ICacheProvider Cacher => _cacheProvider ?? (_cacheProvider = new DefaultCacheProvider());
        public const string CacheTables = "Tables";
        public const string CacheProducts = "Products";
        public const string CacheGroups = "Groups";
        public const string CacheExGroups = "ExGroups";
        public const string ColumnOption = "ColumnOption";
        public const string DefaultColumnOption = "3";

        public static string GetColumnOption()
        {
            return IsSetColumnOption() ? Cacher.Get(ColumnOption).ToString() : DefaultColumnOption;
        }

        public static bool IsSetColumnOption()
        {
            return Cacher.IsSet(ColumnOption);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Caching;

namespace HobiHobi.Core.Caching
{
    /// <summary>
    /// Handle caching for syndication list river js production
    /// </summary>
    public static class SyndicationRiverJsCache
    {
        const string CACHE_PREFIX = "_RSS_";

        public static void Store(string name, object content, Cache cache)
        {
            var cacheKey = CACHE_PREFIX + name;
            cache.Add(cacheKey, content, null, Cache.NoAbsoluteExpiration, new TimeSpan(0, 10, 0), CacheItemPriority.Default, null);
        }

        public static string Get(string name, Cache cache)
        {
            var cacheKey = CACHE_PREFIX + name;
            return cache[cacheKey] as string;
        }

        public static void Flush(string name, Cache cache)
        {
            var cacheKey = CACHE_PREFIX + name;
            cache.Remove(cacheKey);
        }
    }
}

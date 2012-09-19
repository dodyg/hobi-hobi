using HobiHobi.Core.Feeds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Caching;

namespace HobiHobi.Core.Caching
{
    public static class SyndicationFeedCache
    {
        const string CACHE_PREFIX = "_SYND_FEED_";
   
        public static void Store(string feedUri, object content, Cache cache)
        {
            var cacheKey = CACHE_PREFIX + feedUri;
            cache.Add(cacheKey, content, null, Cache.NoAbsoluteExpiration, new TimeSpan(0, 10, 0), CacheItemPriority.Default, null);
        }

        public static FeedsRiver Get(string feedUri, Cache cache)
        {
            var cacheKey = CACHE_PREFIX + feedUri;
            return cache[cacheKey] as FeedsRiver;
        }

        public static void Flush(string feedUri, Cache cache)
        {
            var cacheKey = CACHE_PREFIX + feedUri;
            cache.Remove(cacheKey);
        }
 
    }
}

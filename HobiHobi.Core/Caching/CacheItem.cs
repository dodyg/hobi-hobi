using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HobiHobi.Core.Caching
{
    public class CacheItem<T>
    {
        public CacheItem (T val)
	    {
            Item = val;
	    }

        public T Item { get; set;}
        public Dictionary<string, string> ETags = new Dictionary<string, string>();
    }
}

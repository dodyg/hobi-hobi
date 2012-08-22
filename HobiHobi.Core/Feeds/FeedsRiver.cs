using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HobiHobi.Core.Feeds
{
    /// <summary>
    /// This is the root data structure for river json format
    /// </summary>
    public class FeedsRiver
    {
        public FeedsCollection UpdatedFeeds { get; set; }
    }
}

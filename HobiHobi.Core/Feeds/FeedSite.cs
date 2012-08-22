using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HobiHobi.Core.Feeds
{
    public class FeedSite
    {
        public string FeedUrl { get; set; }
        public string WebsiteUrl { get; set; }
        public string FeedTitle { get; set; }
        public string FeedDescription { get; set; }
        public string WhenLastUpdate { get; set; }
        public FeedItem[] Item { get; set; }
    }
}

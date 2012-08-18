using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HobiHobi.Core.Feeds
{
    public class FeedsRiver
    {
        public FeedsCollection UpdatedFeeds { get; set; }
    }
   
    public class FeedsCollection
    {
        public FeedSite[] UpdatedFeed { get; set; }
    }

    public class FeedSite
    {
        public string FeedUrl { get; set; }
        public string WebsiteUrl { get; set; }
        public string FeedTitle { get; set; }
        public string FeedDescription { get; set; }
        public string WhenLastUpdate { get; set; }
        public FeedItem[] Item { get; set; }       
    }

    public class FeedItem
    {
        public string Body { get; set; }
        public string PermaLink { get; set; }
        public string PubDate { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }
        public string Id { get; set; }
    }
}

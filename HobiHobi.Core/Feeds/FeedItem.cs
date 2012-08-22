using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HobiHobi.Core.Feeds
{
    public class FeedItem
    {
        public string Body { get; set; }
        public string PermaLink { get; set; }
        public string PubDate { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }
        public string Comments { get; set; }
        public string Id { get; set; }
    }
}

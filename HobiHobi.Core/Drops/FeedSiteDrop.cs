using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotLiquid;
using HobiHobi.Core.Feeds;

namespace HobiHobi.Core.Drops
{
    /// <summary>
    /// This is dotliquid object to hold
    /// FeedSite 
    /// </summary>
    public class FeedSiteDrop : Drop
    {
        public FeedSiteDrop(FeedSite site)
        {
            Title = site.FeedTitle;
            Description = site.FeedDescription;
            Url = site.FeedUrl;
            WebsiteUrl = site.WebsiteUrl;
            WhenLastUpdate = site.WhenLastUpdate;
            Items = site.Item.Select(x => new FeedItemDrop(x)).ToArray();
        }
        
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string WebsiteUrl { get; set; }
        public string WhenLastUpdate { get; set; }
        public FeedItemDrop[] Items { get; set; }
    }
}

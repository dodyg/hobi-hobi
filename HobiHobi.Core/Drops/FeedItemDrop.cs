using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotLiquid;
using HobiHobi.Core.Feeds;

namespace HobiHobi.Core.Drops
{
    public class FeedItemDrop : Drop
    {
        public FeedItemDrop(FeedItem item)
        {
            Id = item.Id;
            Title = item.Title;
            Body = item.Body;
            Link = item.Link;
            PermaLink = item.PermaLink;
            PubDate = item.PubDate;
            CommentsLink = item.Comments;
        }

        public string Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Link { get; set; }
        public string PermaLink { get; set; }
        public string PubDate { get; set; }
        public string CommentsLink { get; set; }
    }
}

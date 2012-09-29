using HobiHobi.Core.Syndications;
using HobiHobi.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;

namespace HobiHobi.Core.Feeds
{
    /// <summary>
    /// This is the root data structure for river json format
    /// </summary>
    public class FeedsRiver
    {
        public FeedsCollection UpdatedFeeds { get; set; }

        public static FeedsRiver FromSyndication(List<FeedAndSource> feeds)
        {
            var river = new FeedsRiver();
            river.UpdatedFeeds = new FeedsCollection();

            var f = from x in feeds
                    select new FeedSite
                    {
                        FeedTitle = x.Feed.Title.Text,
                        FeedDescription = x.Feed.Description.Text,
                        WhenLastUpdate = x.Feed.LastUpdatedTime.ToString("R"),
                        FeedUrl = x.Source.XmlUri != null ? x.Source.XmlUri.ToString() : string.Empty,
                        WebsiteUrl = x.Source.HtmlUri != null ? x.Source.HtmlUri.ToString() : string.Empty,
                        Item = (from y in x.Feed.Items
                                select new FeedItem
                                {
                                    Id = y.Id,
                                    Title = (y.Title != null) ? y.Title.Text : string.Empty,
                                    Body = Texts.LimitTextForRiverJs((y.Summary != null) ? y.Summary.Text : string.Empty),
                                    PubDate = y.PublishDate.ToString("R"),
                                    PermaLink = y.Id,
                                    Link = y.Id
                                }).ToArray()
                    };


            river.UpdatedFeeds.UpdatedFeed = f.ToArray();

            return river;
        }
    }
}

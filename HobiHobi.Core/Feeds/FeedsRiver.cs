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

        public static FeedsRiver FromSyndication(List<SyndicationFeed> feeds)
        {
            var river = new FeedsRiver();
            river.UpdatedFeeds = new FeedsCollection();

            var f = from x in feeds
                    select new FeedSite
                    {
                        FeedTitle = x.Title.Text,
                        FeedDescription = x.Description.Text,
                        WhenLastUpdate = x.LastUpdatedTime.ToString("R"),
                        Item = (from y in x.Items
                                select new FeedItem
                                {
                                    Id = y.Id,
                                    Title = y.Title.Text,
                                    Body = y.Summary.Text,
                                    PubDate = y.PublishDate.ToString("R"),
                                    PermaLink = y.Id
                                }).ToArray()
                    };


            river.UpdatedFeeds.UpdatedFeed = f.ToArray();

            return river;
        }
    }
}

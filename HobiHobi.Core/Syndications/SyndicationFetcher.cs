﻿using HobiHobi.Core.Framework;
using HobiHobi.Core.Subscriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Xml;
using System.Threading;
using System.Threading.Tasks;

namespace HobiHobi.Core.Syndications
{
    /// <summary>
    /// This class manage the downloading mechanism for RSS/ATOM feeds
    /// </summary>
    public class SyndicationFetcher
    {
        /// <summary>
        /// Download all the latest RSS feed
        /// </summary>
        /// <param name="subscription"></param>
        /// <returns></returns>
        public List<FeedAndSource> DownloadAll(RssSubscription subscription)
        {
            var feeds = new SynchronizedCollection<FeedAndSource>();

            var tasks = new List<Task>();

            foreach (var sub in subscription.Items)
            {
                var r = Task.Factory.StartNew(() =>
                    {
                        var res = Fetch(sub.XmlUri);
                        if (res.IsFound)
                            feeds.Add(new FeedAndSource(sub, res.Item));
                    });

                tasks.Add(r);
            }

            Task.WaitAll(tasks.ToArray());

            return feeds.ToList();
        }

        /// <summary>
        /// Fetch a single rss/atom feed
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public IQuerySetOne<SyndicationFeed> Fetch(Uri uri)
        {
            try
            {
                using (var reader = new SyndicationXmlReader(uri.ToString()))
                {
                    var feed = SyndicationFeed.Load(reader);

                    return new QuerySetOne<SyndicationFeed>(feed);
                }
            }
            catch (Exception e)
            {
                var qone = new QuerySetOne<SyndicationFeed>(null);
                qone.Error(e);
                return qone;
            }
        }
    }
}

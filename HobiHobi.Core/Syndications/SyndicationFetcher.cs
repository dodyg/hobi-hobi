using HobiHobi.Core.Framework;
using HobiHobi.Core.Subscriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Xml;

namespace HobiHobi.Core.Syndications
{
    public class SyndicationFetcher
    {
        RssSubscription _subscription;
        public SyndicationFetcher(RssSubscription subscription)
        {
            _subscription = subscription;
        }

        public int Download()
        {
            int totalCount = 0;
            foreach (var sub in _subscription.Items)
            {
                var res = Fetch(sub.XmlUri);
                if (res.IsFound)
                    totalCount = res.Item.Items.Count();
            }

            return totalCount;
        }

        /// <summary>
        /// Fetch a single rss/atom feed
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public IQuerySetOne<SyndicationFeed> Fetch(Uri uri)
        {
            using (var reader = XmlReader.Create(uri.ToString()))
            {
                var feed = SyndicationFeed.Load(reader);

                return new QuerySetOne<SyndicationFeed>(feed);
            }
        }
    }
}

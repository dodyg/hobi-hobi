using HobiHobi.Core.Subscriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;

namespace HobiHobi.Core.Syndications
{
    public class FeedAndSource
    {
        public SyndicationFeed Feed { get; set; }
        public RssSubscriptionItem Source { get; set; }

        public FeedAndSource(RssSubscriptionItem source, SyndicationFeed feed)
        {
            Source = source;
            Feed = feed;
        }
    }
}


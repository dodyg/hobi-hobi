using HobiHobi.Core.Subscriptions;
using HobiHobi.Core.Syndications;
using HobiHobi.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HobiHobi.Web.Controllers
{
    public class AggregatorController : Controller
    {
        public ActionResult Index(string url)
        {
            var uri = new Uri(url);
            var subFetcher = new SubscriptionFetcher();
            var xml = subFetcher.Download(Texts.FromUriHost(uri), uri.PathAndQuery);
            var opml = new Opml();
            opml.LoadFromXML(xml);
            var subscription = new RssSubscription(opml);

            var fetcher = new SyndicationFetcher(subscription);
            var feeds = fetcher.DownloadAll();

            return Content("Feeds " + feeds.Count);
        }
    }
}

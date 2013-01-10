﻿using HobiHobi.Core.Framework;
using HobiHobi.Core.Syndications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using System.Xml;

namespace HobiHobi.Web.Areas.API.Controllers
{
    public class RssController : Controller
    {
        public ActionResult Slim(string url, int maxSize)
        {
            try
            {
                SyndicationFeed feed = this.HttpContext.Cache[url] as SyndicationFeed;

                if (feed == null)
                {
                    var fetcher = new SyndicationFetcher();
                    var content = fetcher.Fetch(new Uri(url));

                    if (content.IsFound)
                    {
                        var syndicate = content.Item;
                        feed = Filter(syndicate, maxSize);
                        //cache source for 6 hours
                        HttpContext.Cache.Add(url, syndicate, null, Cache.NoAbsoluteExpiration, new TimeSpan(6, 0, 0), CacheItemPriority.Default, null);
                    }
                    else
                    {
                        return HttpNotFound();
                    }
                }
                else
                {
                    feed = Filter(feed, maxSize);
                }

                var rssOutput = new StringBuilder();
                using (var xml = XmlWriter.Create(rssOutput))
                {
                    feed.SaveAsRss20(xml);
                }

                //Read http://baleinoid.com/whaly/2009/07/xmlwriter-and-utf-8-encoding-without-signature/
                var payload = rssOutput.ToString().Replace("encoding=\"utf-16\"", ""); //remove the Processing Instruction encoding mark for the xml body = it's a hack I know

                this.Compress();
                return Content(payload, "application/rss+xml");
            }
            catch (Exception ex)
            {
                return HttpDoc<EmptyHttpReponse>.PreconditionFailed(ex.Message).ToJson();
            }
        }

        public SyndicationFeed Filter(SyndicationFeed input, int maxSize)
        {
            input.Items = input.Items.Take(maxSize);
            return input;
        }
    }
}

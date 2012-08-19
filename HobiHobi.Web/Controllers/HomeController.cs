using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using HobiHobi.Core.Feeds;
using HobiHobi.Core.Framework;

namespace HobiHobi.Web.Controllers
{
    public class HomeController : Controller
    {
        string _template = @"
{% for feed in feeds -%}
        {% for item in feed.items -%}
            <div class=""feed_item"" data-id=""{{ item.id }}"">
                <h2>{{ item.title }}<a href=""{{ item.link }}"">#</a></h2>
                {{ item.body }}
                <p class=""feed_date"">{{ item.pub_date }}</p>
                <div class=""feed_origin_website"">From: <a href=""{{ feed.website_url }}"">{{ feed.Title }}</a></div>
            </div><!-- feed_item -->
        {% endfor -%}
{% endfor -%}
";
        public ActionResult Index()
        {
            this.Compress();
            return View();
        }

        public ActionResult Feed(string feedName)
        {
            var feedTarget = GetFeed(feedName);

            if (!feedTarget.IsFound)
                return HttpNotFound();

            var hostTarget = "http://static.scripting.com";
            var pathTarget = feedTarget.Item;

            ViewBag.FeedUrl = hostTarget + pathTarget;

            var cache = this.HttpContext.Cache[ViewBag.FeedUrl];

            if (cache == null)
            {
                var fetch = new Fetcher();
                try
                {
                    var output = fetch.Download(hostTarget, pathTarget);
                    var river = fetch.Serialize(output);

                    HttpContext.Cache.Add(ViewBag.FeedUrl, river, null, Cache.NoAbsoluteExpiration, new TimeSpan(0, 10, 0), CacheItemPriority.Default, null);

                    ViewBag.CacheStatus = "No Cache";

                    var renderer = new FeedTemplateRenderer(river, _template);
                    this.Compress();
                    return Content(renderer.Render().ToString(), "text/html");
                }
                catch (Exception ex)
                {
                    return Content("Sorry, we really try to process " + ViewBag.FeedUrl + " but fate has decided on something else. " + ex.Message);
                }
            }
            else
            {
                ViewBag.CacheStatus = "Cached for 10 minutes";
                var renderer = new FeedTemplateRenderer(cache as FeedsRiver, _template);
                this.Compress();
                return Content(renderer.Render().ToString(), "text/html");
            }
        }

        IQuerySetOne<string> GetFeed(string feedName)
        {
            switch (feedName)
            {
                case "apple": return new QuerySetOne<string>("/houston/rivers/apple/apple.json");
                case "dave": return new QuerySetOne<string>("/houston/rivers/dave/dave.json");
                case "nyt": return new QuerySetOne<string>("/houston/rivers/nyt/nyt.json");
                default: return new QuerySetOne<string>(null);
            }
        }
    }
}

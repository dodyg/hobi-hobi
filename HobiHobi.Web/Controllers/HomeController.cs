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
    <div class=""feed"">
        <h2>{{ feed.Title }}</h2>
        <div class=""last_updated"">{{ feed.when_last_update }}</div>
        {% for item in feed.items -%}
            <div class=""feed_item"" data-id=""{{ item.id }}"">
                <h3>{{ item.title }}<a href=""{{ item.link }}"">#</a></h3>
                {{ item.body }}
                <p class=""feed_date"">{{ item.pub_date }}</p>
            </div><!-- feed_item -->
        {% endfor -%}
    </div><!-- end of feed -->
{% endfor -%}
";
        public ActionResult Index()
        {
            var hostTarget = "http://static.scripting.com";
            var pathTarget = "/houston/rivers/apple/apple.json";

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
                    ViewBag.Output = renderer.Render();
                    this.Compress();
                    return View(river);
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
                ViewBag.Output = renderer.Render();
                this.Compress();                    
                return View(cache);
            }
        }
    }
}

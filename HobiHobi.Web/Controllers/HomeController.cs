using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using HobiHobi.Core;
using HobiHobi.Core.Feeds;
using HobiHobi.Core.Framework;

namespace HobiHobi.Web.Controllers
{
    public class HomeController : RavenController
    {
        string _template = @"
{% for feed in feeds -%}
        {% for item in feed.items -%}
            <div class=""feed_item"" data-id=""{{ item.id }}"">
                <h2>{{ item.title }} <a href=""{{ item.link }}"">#</a></h2>
                {% if item.thumbnails -%}
                <div class=""feed_item_thumbnail"">
                    {% for thumb in item.thumbnails -%}
                        <img src=""{{ thumb.url }}"" width=""{{ thumb.width }}"" height=""{{ thumb.height }}"" />
                    {% endfor -%}
                </div><!-- end of feed_item_thumbnail -->
                {% endif -%}
                {{ item.body }}
                <p class=""feed_item_date"">{{ item.pub_date }}</p>
                {% if feed.title != """" -%}
                <div class=""feed_origin_website"">Source: <a href=""{{ feed.website_url }}"">{{ feed.title }}</a></div>
                {% else %}
                <div class=""feed_origin_website""><a href=""{{ feed.website_url }}"">Source</a></div>
                {% endif -%}
                {% if item.comments_link -%}
                <div class=""feed_item_comments""><a href=""{{ item.comments_link }}"">Comments</a></div> 
                {% endif -%}
            </div><!-- feed_item -->
        {% endfor -%}
{% endfor -%}
";
        public ActionResult Index()
        {
            this.RavenSession.Store(
                new
                {
                    Id = "user/dody",
                    Message = "Hello World"
                }
            );

            this.Compress();
            return View();
        }

        public ActionResult Hello()
        {
            var val = this.RavenSession.Load<dynamic>("user/dody");

            if (val != null)
                return Content("Hello " + val.Message);
            else
                return Content("No such key");
        }

        public ActionResult Feed(string feedName)
        {
            var feedTarget = GetFeed(feedName);

            if (!feedTarget.IsFound)
                return HttpNotFound();

            var hostTarget = feedTarget.Item.Host;
            var pathTarget = feedTarget.Item.Path;

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

        IQuerySetOne<HostAndPath> GetFeed(string feedName)
        {
            switch (feedName)
            {
                case "apple": return new QuerySetOne<HostAndPath>(new HostAndPath("http://static.scripting.com","/houston/rivers/apple/apple.json"));
                case "tech": return new QuerySetOne<HostAndPath>(new HostAndPath("http://static.scripting.com","/houston/rivers/techmeme/techmeme.json"));
                case "dave": return new QuerySetOne<HostAndPath>(new HostAndPath("http://static.scripting.com", "/houston/rivers/iowaRiver3.js"));
                case "nyt": return new QuerySetOne<HostAndPath>(new HostAndPath("http://static.scripting.com","/houston/rivers/nyt/nyt.json"));
                case "noAgenda": return new QuerySetOne<HostAndPath>(new HostAndPath("http://s3.amazonaws.com", "/river.curry.com/rivers/radio2/River3.js"));   
                default: return new QuerySetOne<HostAndPath>(null);
            }
        }
    }
}

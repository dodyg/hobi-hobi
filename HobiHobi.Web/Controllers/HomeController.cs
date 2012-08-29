using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using HobiHobi.Core;
using HobiHobi.Core.Feeds;
using HobiHobi.Core.Framework;
using HobiHobi.Core.Subscriptions;

namespace HobiHobi.Web.Controllers
{
    public class HomeController : RavenController
    {
        string _template = @"
{% for feed in feeds -%}
        {% for item in feed.items -%}
            <div class=""feed_item"" data-id=""{{ item.id }}"">
                <h2>{{ item.title }} <a href=""{{ item.link }}"" class=""item_link"">#</a></h2>
                <div class=""feed_item_body"">
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
                </div><!-- feed item body-->
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

        public IQuerySetOne<RiverSubscription> FetchDefaultRivers()
        {
            string cacheKey = "DEFAULT_RIVERS";
            var cache = HttpContext.Cache[cacheKey] as RiverSubscription;

            if (cache != null)
            {
                return new QuerySetOne<RiverSubscription>(cache);
            }
            else
            {
                var fetcher = new SubscriptionFetcher();
                var xml = fetcher.Download("http://hobihobi.apphb.com", "api/1/default/RiversSubscription");
                var opml = new Opml();
                var res = opml.LoadFromXML(xml);

                if (res.IsTrue)
                {
                    var subscriptionList = new RiverSubscription(opml);
                    HttpContext.Cache.Add(cacheKey, subscriptionList, null, Cache.NoAbsoluteExpiration, new TimeSpan(0, 10, 0), CacheItemPriority.Default, null);

                    return new QuerySetOne<RiverSubscription>(subscriptionList);
                }
                else
                {
                    return new QuerySetOne<RiverSubscription>(null);
                }
            }
        }

        public ActionResult Hello()
        {
            var river = FetchDefaultRivers();

            if (river.IsFound)
            {
                return Content(river.Item.Items.First().Title);
            }
            else
            {
                return Content("No rivers");
            }
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
                var fetch = new RiverFetcher();
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
            var list = FetchDefaultRivers();

            if (list.IsFound)
            {
                var rivers = list.Item.Items;

                var feed = rivers.Where(x => x.Name == feedName).FirstOrDefault();

                if (feed != null)
                {
                    var uri = feed.JSONUri;
                    return new QuerySetOne<HostAndPath>(new HostAndPath
                    (
                        host:"http://" + uri.DnsSafeHost,
                        path:uri.PathAndQuery
                    ));
                }
                else
                    return new QuerySetOne<HostAndPath>(null);
            }
            else
                return new QuerySetOne<HostAndPath>(null);
        }
    }
}

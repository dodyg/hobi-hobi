using HobiHobi.Core.Caching;
using HobiHobi.Core.Feeds;
using HobiHobi.Core.Framework;
using HobiHobi.Core.Identity;
using HobiHobi.Core.Syndications;
using HobiHobi.Core.Utils;
using System;
using System.Web;
using System.Web.Mvc;

namespace HobiHobi.Web.Controllers
{
    public class SyndicationController : RavenController
    {
        public ActionResult Index(string name)
        {
            var id = SyndicationList.NewId(name);

            var list = RavenSession.Load<SyndicationList>(id.Full());

            if (list != null)
            {
                ViewBag.Title = list.Title;
                ViewBag.Description = Server.HtmlEncode(list.Description);
                ViewBag.Keywords = Server.HtmlEncode(list.Keywords);
                ViewBag.Name = list.Name;

                var edit = CookieMonster.GetFromCookie<TransientAccount>(Request.Cookies[TransientAccount.COOKIE_NAME]);

                if (edit.IsFound && edit.Item.IsSyndicationListFound(list.Guid))
                {
                    ViewBag.EditLink = "/manage/syndication/sources/?guid=" + list.Guid;
                }
                else
                {
                    if (!edit.IsFound)
                    {
                        var init = new TransientAccount();
                        init.SyndicationGuids.Add(list.Guid);
                        Response.Cookies.Add(CookieMonster.SetCookie(init, TransientAccount.COOKIE_NAME));
                    }
                    else //
                    {
                        edit.Item.SyndicationGuids.Add(list.Guid);
                        Response.Cookies.Add(CookieMonster.SetCookie(edit.Item, TransientAccount.COOKIE_NAME));
                    }
                }

                return View();
            }
            else
                return HttpNotFound();
        }

        public ActionResult GetOpml(string name)
        {
            var id = SyndicationList.NewId(name);
            var list = RavenSession.Load<SyndicationList>(id.Full());

            if (list != null)
            {
                var opml = list.Sources.ToOpml();
                var xml = opml.ToXML();

                this.Compress();
                return Content(xml.ToString(), "text/xml");
            }
            else
                return HttpNotFound();
        }

        public ActionResult Feed(string name)
        {
            var hostTarget = Texts.FromUriHost(this.Request.Url);
            var pathTarget = "/s/riverjs/" + name;

            ViewBag.FeedUrl = hostTarget + pathTarget;
            
            FeedsRiver cache = SyndicationFeedCache.Get(ViewBag.FeedUrl as string, HttpContext.Cache);

            if (cache == null)
            {
                var fetch = new RiverFetcher();
                try
                {
                    var output = fetch.Download(hostTarget, pathTarget);
                    var river = fetch.Serialize(output);

                    SyndicationFeedCache.Store(ViewBag.Url as string, river, HttpContext.Cache);

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
                var renderer = new FeedTemplateRenderer(cache, _template);
                this.Compress();
                return Content(renderer.Render().ToString(), "text/html");
            }
        }

        public ActionResult GetRiverJs(string name)
        {
            var syndications = SyndicationRiverJsCache.Get(name, HttpContext.Cache);

            if (syndications == null)
            {
                var id = SyndicationList.NewId(name);
                var list = RavenSession.Load<SyndicationList>(id.Full());

                if (list != null)
                {
                    var subscription = list.Sources;
                    var fetcher = new SyndicationFetcher();
                    var feeds = fetcher.DownloadAll(subscription);

                    var river = FeedsRiver.FromSyndication(feeds);

                    var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(river, Newtonsoft.Json.Formatting.Indented);
                    var jsonp = "onGetRiverStream (" + jsonString + ")";

                    SyndicationRiverJsCache.Store(name, jsonp, HttpContext.Cache);
                    this.Compress();
                    return Content(jsonp, "application/json");
                }
                else
                    return HttpNotFound();

            }
            else
            {
                this.Compress();
                return Content(syndications, "application/json");
            }
        }

        public ActionResult RssJs()
        {
            return View();
        }

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

    }
}

﻿using HobiHobi.Core.Feeds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HobiHobi.Core.Framework;
using HobiHobi.Core;
using System.Web.Caching;

namespace HobiHobi.Web.Controllers
{
    public class RiverController : RavenController
    {
        public ActionResult Index(string name)
        {
            var id = RiverWall.NewId(name);

            var wall = RavenSession.Load<RiverWall>(id.Full());

            if (wall != null)
            {
                ViewBag.Title = wall.Title;
                ViewBag.Description = Server.HtmlEncode(wall.Description);
                ViewBag.Keywords = Server.HtmlEncode(wall.Keywords);
                ViewBag.BodyInline = wall.Template.HtmlBodyInline;
                ViewBag.HeaderInline = wall.Template.HtmlHeadInline;
                ViewBag.LinkedCss = string.Format("/r/css/{0}/{1}", id.Partial(), wall.Template.LessCss.ETag);
                if (!wall.Template.JavaScript.IsEmpty)
                    ViewBag.LinkedJs = string.Format("/r/js/{0}/{1}", id.Partial(), wall.Template.JavaScript.ETag);
                else
                    if (!wall.Template.CoffeeScript.IsEmpty)
                        ViewBag.LinkedJs = string.Format("/r/js/{0}/{1}", id.Partial(), wall.Template.CoffeeScript.ETag);

                return View();
            }
            else
                return HttpNotFound();
        }

        public ActionResult GetFeed(string name, string feedName)
        {
            var id = RiverWall.NewId(name);
            var wall = RavenSession.Load<RiverWall>(id.Full());

            var feedTarget = GetFeed(wall, feedName);

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
                    
                    var renderer = new FeedTemplateRenderer(river, wall.Template.FeedLiquidTemplate);
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
                var renderer = new FeedTemplateRenderer(cache as FeedsRiver, wall.Template.FeedLiquidTemplate);
                this.Compress();
                return Content(renderer.Render().ToString(), "text/html");
            }
        }

        IQuerySetOne<HostAndPath> GetFeed(RiverWall wall, string feedName)
        {
            if (wall != null)
            {
                var rivers = wall.Sources.Items;

                var feed = rivers.Where(x => x.Name == feedName).FirstOrDefault();

                if (feed != null)
                {
                    var uri = feed.JSONPUri;
                    return new QuerySetOne<HostAndPath>(new HostAndPath
                    (
                        host: "http://" + uri.DnsSafeHost,
                        path: uri.PathAndQuery
                    ));
                }
                else
                    return new QuerySetOne<HostAndPath>(null);
            }
            else
            {
                return new QuerySetOne<HostAndPath>(null);
            }
        }

        public ActionResult GetCss(string name)
        {
            var id = RiverWall.NewId(name);

            var wall = RavenSession.Load<RiverWall>(id.Full());

            if (wall != null)
            {
                var css = wall.Template.LessCss.GetText();
                this.Compress();
                return Content(css, "text/css");
            }
            else
                return HttpNotFound();
        }

        public ActionResult GetJs(string name)
        {
            var id = RiverWall.NewId(name);

            var wall = RavenSession.Load<RiverWall>(id.Full());

            if (wall != null)
            {
                this.Compress();
                if (!wall.Template.JavaScript.IsEmpty)
                {
                    var js = wall.Template.JavaScript.GetText();
                    return Content(js, "application/javascript");
                }
                else
                {
                    var js = wall.Template.CoffeeScript.GetText();
                    return Content(js, "application/javascript");
                }
            }
            else
                return HttpNotFound();
        }
    }
}
﻿using HobiHobi.Core.Blogging;
using HobiHobi.Core.Identity;
using HobiHobi.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;
using HobiHobi.Core;
using HobiHobi.Core.Framework;

namespace HobiHobi.Web.Controllers
{
    /// <summary>
    /// Deal with the public facing side of the blog
    /// </summary>
    public class BlogController : RavenController
    {
        public ActionResult Index(string name)
        {
            var blog = RavenSession.Query<Blog>().Where(x => x.Name == name).FirstOrDefault();

            if (blog == null)
                return HttpNotFound();

            var edit = CookieMonster.GetFromCookie<TransientAccount>(Request.Cookies[TransientAccount.COOKIE_NAME]);

            if (edit.IsFound && edit.Item.IsBlogFound(blog.Guid))
            {
                ViewBag.EditLink = "/manage/blog/?guid=" + blog.Guid;
            }

            ViewBag.Title = blog.Title;
            ViewBag.Description = blog.Description;
            ViewBag.Keywords = blog.Keywords;
            ViewBag.OpmlUrl = "/b/opml/" + blog.Name;

            var feeds = blog.GetFeeds(RavenSession);
            ViewBag.Feeds = feeds;

            return View();
        }

        public ActionResult FixTemporaryLogin(string name)
        {
            var blog = RavenSession.Query<Blog>().Where(x => x.Name == name).FirstOrDefault();

            if (blog == null)
                return HttpNotFound();

            var transient = CookieMonster.GetFromCookie<TransientAccount>(Request.Cookies[TransientAccount.COOKIE_NAME]);
            if (!transient.IsFound)
            {
                var init = new TransientAccount();
                init.BlogGuids.Add(blog.Guid);
                init.MarkUpdated();
                Response.Cookies.Add(CookieMonster.SetCookie(init, TransientAccount.COOKIE_NAME));
            }
            else
            {
                transient.Item.BlogGuids.Add(blog.Guid);
                transient.Item.MarkUpdated();
                Response.Cookies.Add(CookieMonster.SetCookie(transient.Item, TransientAccount.COOKIE_NAME));
            }

            return Content("The temporary login has been issued");
        }

        public ActionResult BlogOpmlSubscriptionList(string name)
        {
            var blog = RavenSession.Query<Blog>().Where(x => x.Name == name).FirstOrDefault();

            if (blog == null)
                return HttpNotFound();

            var feeds = blog.GetFeeds(RavenSession);
            var opml = blog.GetFeedsOpml(feeds, Request);
            var xml = opml.ToXML();

            this.Compress();
            return Content(xml.ToString(), "text/xml");
        }

        /// <summary>
        /// There are three types of cachedFeed. Render as HTML or RSS or RSSJS
        /// </summary>
        /// <param name="feedSlug"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public ActionResult Feed(string slug)
        {
            var feed = BlogFeed.FindByUrl(RavenSession, slug);

            if (!feed.IsFound)
                return HttpNotFound();

            ViewBag.RssJs = feed.Item.GetRssJsLink(Request);
            ViewBag.Rss = feed.Item.GetRssLink(Request);

            return View();
        }

        public ActionResult FeedRssJs(string slug, int page = SiteConstants.INITIAL_PAGE, int pageSize = SiteConstants.LARGE_PAGE_SIZE)
        {
            if (pageSize > SiteConstants.MAXIMUM_PAGE_SIZE)
                throw new ArgumentException("Page size maximum is " + SiteConstants.MAXIMUM_PAGE_SIZE);

            var feed = BlogFeed.FindByUrl(RavenSession, slug);

            if (!feed.IsFound)
                return HttpNotFound();

            feed.Item.LoadRss(RavenSession, page, pageSize);
            var rss = feed.Item.GetRssJs(HttpContext.Request.Url);

            var json = JsonConvert.SerializeObject(rss, JsonSettings.Get());
            var jsonP = "onGetRss(" + json + ")";

            return Content(jsonP, "application/json");
        }

        public ActionResult FeedRss(string slug, int page = SiteConstants.INITIAL_PAGE, int pageSize = SiteConstants.LARGE_PAGE_SIZE)
        {
            if (pageSize > SiteConstants.MAXIMUM_PAGE_SIZE)
                throw new ArgumentException("Page size maximum is " + SiteConstants.MAXIMUM_PAGE_SIZE);

            var feed = BlogFeed.FindByUrl(RavenSession, slug);

            if (!feed.IsFound)
                return HttpNotFound();

            feed.Item.LoadRss(RavenSession, page, pageSize);
            var rss = feed.Item.GetRss(HttpContext.Request.Url);
            var rssOutput = new StringBuilder();
            using (var xml = XmlWriter.Create(rssOutput))
            {
                rss.SaveAsRss20(xml);
            }

            //Read http://baleinoid.com/whaly/2009/07/xmlwriter-and-utf-8-encoding-without-signature/
            var payload = rssOutput.ToString().Replace("encoding=\"utf-16\"",""); //remove the Processing Instruction encoding mark for the xml body = it's a hack I know
            return Content(payload, "application/rss+xml"); 
        }


        public ActionResult FeedItem(string feedSlug, string postSlug)
        {
            var feed = BlogFeed.FindByUrl(RavenSession, feedSlug);

            if (!feed.IsFound)
                return HttpNotFound("Feed is not found");

            var blogPost = BlogPost.GetByUrl(RavenSession, postSlug);

            if (!blogPost.IsFound)
                return HttpNotFound("Post is not found");

            ViewBag.FeedTitle = feed.Item.Title;
            ViewBag.FeedHtmlLink = feed.Item.GetHtmlLink(Request);
            ViewBag.Post = blogPost.Item;

            return View();
        }


    }
}

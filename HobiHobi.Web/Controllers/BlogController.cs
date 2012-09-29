using HobiHobi.Core.Blogging;
using HobiHobi.Core.Identity;
using HobiHobi.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;

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

            return View();
        }

        /// <summary>
        /// There are three types of feed. Render as HTML or RSS or RSSJS
        /// </summary>
        /// <param name="slug"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public ActionResult Feed(string slug)
        {
            var feed = BlogFeed.FindByUrl(RavenSession, slug);

            if (!feed.IsFound)
                return HttpNotFound();

            return View();
        }

        public ActionResult FeedRssJs(string slug)
        {
            return View();
        }

        public ActionResult FeedRss(string slug)
        {
            var feed = BlogFeed.FindByUrl(RavenSession, slug);

            if (!feed.IsFound)
                return HttpNotFound();

            feed.Item.LoadRss(RavenSession);
            var rss = feed.Item.GetRss(HttpContext.Request.Url);
            var rssOutput = new StringBuilder();
            using (var xml = XmlWriter.Create(rssOutput))
            {
                rss.SaveAsRss20(xml);
            }

            return Content(rssOutput.ToString(), "application/rss+xml"); 
        }
    }
}

using HobiHobi.Core.Feeds;
using HobiHobi.Core.Subscriptions;
using HobiHobi.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HobiHobi.Core.Framework;
using HobiHobi.Core.Utils;
using HobiHobi.Core.Syndications;
using HobiHobi.Core.Blogging;

namespace HobiHobi.Web.Areas.API.Controllers
{
    public class SiteController : RavenController
    {
        [HttpGet]
        public ActionResult TabbedRivers()
        {
            var rivers = this.RavenSession.Query<RiverWall>()
                .Where(x => x.Status == RiverWallStatus.Published || x.Status == RiverWallStatus.Draft).ToList();
            var opml = new Opml();
            opml.Title = "Hobi Published Wall List";
            opml.OwnerName = "hobieu";
            opml.Outlines.AddRange(rivers.Select(x =>
            {
                var outline = new Outline();
                outline.Attributes["type"] = "link";
                outline.Attributes["text"] = x.Title;
                outline.Attributes["name"] = x.Name;
                outline.Attributes["url"] = Texts.FromUriHost(Request.Url) + "/r/" + x.Name;
                outline.Attributes["urlOpml"] = Texts.FromUriHost(Request.Url) + "/r/opml/" + x.Name;
                return outline;
            }));

            var xml = opml.ToXML();
            this.Compress();
            return Content(xml.ToString(), "text/xml");
        }

        [HttpGet]
        public ActionResult SyndicationLists()
        {
            var lists = this.RavenSession.Query<SyndicationList>()
                .ToList();
            var opml = new Opml();
            opml.Title = "Hobi Published Syndication List";
            opml.OwnerName = "hobieu";
            opml.Outlines.AddRange(lists.Select(x =>
            {
                var outline = new Outline();
                outline.Attributes["type"] = "link";
                outline.Attributes["text"] = x.Title;
                outline.Attributes["name"] = x.Name;
                outline.Attributes["url"] = Texts.FromUriHost(Request.Url) + "/s/" + x.Name;
                return outline;
            }));

            var xml = opml.ToXML();
            this.Compress();
            return Content(xml.ToString(), "text/xml");
        }

        [HttpGet]
        public ActionResult Blogs()
        {
            var blogs = this.RavenSession.Query<Blog>().ToList();
            var opml = new Opml()
            {
                Title = "Hobie Published Blogs",
                OwnerName = "hobieu"
            };
            opml.Outlines.AddRange(blogs.Select(x =>
                {
                    var outline = new Outline();
                    outline.Attributes["type"] = "link";
                    outline.Attributes["text"] = x.Title;
                    outline.Attributes["name"] = x.Name;
                    outline.Attributes["url"] = Texts.FromUriHost(Request.Url) + "/b/" + x.Name;
                    return outline;
                }));

            var xml = opml.ToXML();
            this.Compress();
            return Content(xml.ToString(), "text/xml");
        }
    }
}

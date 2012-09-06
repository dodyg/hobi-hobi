using HobiHobi.Core.Feeds;
using HobiHobi.Core.Identity;
using HobiHobi.Core.Syndications;
using HobiHobi.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using HobiHobi.Core.Framework;

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

                if (edit.IsFound && edit.Item.IsRiverFound(list.Guid))
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

        public ActionResult GetRiverJs(string name)
        {
            var cacheKey = "_RSS_" + name;
            var syndications = HttpContext.Cache[cacheKey] as string;

            if (syndications == null)
            {
                var id = SyndicationList.NewId(name);
                var list = RavenSession.Load<SyndicationList>(id.Full());

                if (list != null)
                {
                    var subscription = list.Sources;
                    var fetcher = new SyndicationFetcher(subscription);
                    var feeds = fetcher.DownloadAll();

                    var river = FeedsRiver.FromSyndication(feeds);

                    var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(river, Newtonsoft.Json.Formatting.Indented);

                    HttpContext.Cache.Add(cacheKey, jsonString, null, Cache.NoAbsoluteExpiration, new TimeSpan(0, 10, 0), CacheItemPriority.Default, null);
                    this.Compress();
                    return Content(jsonString, "application/json");
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
    }
}

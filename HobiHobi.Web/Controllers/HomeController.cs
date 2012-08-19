using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using HobiHobi.Core.Feeds;

namespace HobiHobi.Web.Controllers
{
    public class HomeController : Controller
    {
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
                return View(cache);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HobiHobi.Core.Feeds;

namespace HobiHobi.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var fetch = new Fetcher();
            var hostTarget = "http://static.scripting.com";
            var pathTarget = "/houston/rivers/apple/River3.js";

            var output = fetch.Download(hostTarget, pathTarget);

            var river = fetch.Serialize(output);
            return View(river);
        }
    }
}

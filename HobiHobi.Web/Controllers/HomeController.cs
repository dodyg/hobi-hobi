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
            var output = fetch.Download(null);

            try
            {
                var feeds = fetch.Serialize(output);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            ViewBag.Output = new HtmlString(output);
            
            return View();
        }
    }
}

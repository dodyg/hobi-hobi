using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HobiHobi.Core.Subscriptions;
using HobiHobi.Core.Framework;
namespace HobiHobi.Web.Areas.API.Controllers
{
    public class DefaultController : Controller
    {
        public ActionResult RiversSubscription()
        {
            var river = DefaultRiverSubscription.Get();
            var opml = river.ToOpml();
            var xml = opml.ToXML();

            return Content(xml.ToString(), "text/xml");
        }
    }
}

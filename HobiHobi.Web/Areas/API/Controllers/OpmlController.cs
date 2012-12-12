using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HobiHobi.Web.Areas.API.Controllers
{
    public class OpmlController : Controller
    {
        //
        // GET: /API/Opml/

        public ActionResult Root()
        {
            var location = Server.MapPath("/content/opml/root.opml");
            var result = new FilePathResult(location, "application/xml");
            return result;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HobiHobi.Web.Areas.API.Controllers
{
    public class SamplesController : Controller
    {
        public ActionResult RiverJsWithOPML()
        {
            var path = Server.MapPath("/content/samples/riverwithoutlines.js");

            var file = System.IO.File.ReadAllText(path);

            return Content(file, "application/javascript");
        }
    }
}

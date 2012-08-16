using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HobiHobi.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return new ContentResult { Content = "Hello World ", ContentType = "text/html", ContentEncoding = System.Text.UTF8Encoding.UTF8 };
        }
    }
}

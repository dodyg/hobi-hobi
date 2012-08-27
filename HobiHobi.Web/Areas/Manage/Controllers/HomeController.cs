using HobiHobi.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HobiHobi.Web.Areas.Manage.Controllers
{
    public class HomeController : RavenController
    {
        public ActionResult Index()
        {
            return Content("Hello World"); ;
        }
    }
}

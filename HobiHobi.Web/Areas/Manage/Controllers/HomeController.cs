using HobiHobi.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HobiHobi.Core.Identity;

namespace HobiHobi.Web.Areas.Manage.Controllers
{
    [Authorize(Roles = "Participant,Admin")]
    public class HomeController : RavenController
    {
        public ActionResult Index()
        {
            return Content("Hello World"); ;
        }
    }
}

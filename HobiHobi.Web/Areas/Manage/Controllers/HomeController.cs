using HobiHobi.Core.Framework;
using HobiHobi.Core.Identity;
using HobiHobi.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HobiHobi.Web.Areas.Manage.Controllers
{
    [Authorize(Roles="Participant")]
    public class HomeController : RavenController
    {
        public ActionResult Index()
        {
            var userInfo = this.GetCurrentUser();

            return View();
        }
    }
}

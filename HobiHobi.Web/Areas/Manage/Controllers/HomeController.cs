using HobiHobi.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HobiHobi.Core.Identity;
using HobiHobi.Core.Framework;

namespace HobiHobi.Web.Areas.Manage.Controllers
{
    [Authorize(Roles = "Participant")]
    public class HomeController : RavenController
    {
        public ActionResult Index()
        {
            var userInfo = this.GetCurrentUser();

            if (userInfo.IsFound)
                return Content(userInfo.Item.Id + " " + userInfo.Item.Email);
            else
                return Content("nothing");
        }
    }
}

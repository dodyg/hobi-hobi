using HobiHobi.Core;
using HobiHobi.Core.Feeds;
using HobiHobi.Core.ViewModels;
using HobiHobi.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HobiHobi.Web.Areas.Manage.Controllers
{
    public class RiverController : RavenController
    {
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public ActionResult EditTemplate(string guid)
        {
            var wall = RavenSession.Query<RiverWall>().Where(x => x.Guid == guid).FirstOrDefault();

            if (wall == null)
                return HttpNotFound();

            var vm = new RiverTemplateViewModel(wall);

            return View(vm);
        }

        [HttpPost]//, ValidateAntiForgeryToken(Salt = SiteConstants.ANTI_FORGERY_SALT)]
        public ActionResult EditTemplate(string guid, RiverTemplateViewModel vm)
        {
            var obj = new { Message = "Hello world" };
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(obj), "application/json");
        }
    }
}

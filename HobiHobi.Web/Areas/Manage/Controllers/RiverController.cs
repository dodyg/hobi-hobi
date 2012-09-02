using HobiHobi.Core;
using HobiHobi.Core.Feeds;
using HobiHobi.Core.ViewModels;
using HobiHobi.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HobiHobi.Core.Framework;
using System.Text;

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
            if (!ModelState.IsValid)
            {
                var errors = new StringBuilder();
                foreach (ModelState modelState in ViewData.ModelState.Values) {
                    foreach (ModelError error in modelState.Errors) {
                       errors.AppendLine(error.ErrorMessage);
                    }
                }
                return HttpDoc<EmptyHttpReponse>.PreconditionFailed(errors.ToString()).ToJson();
            }

            var wall = RavenSession.Query<RiverWall>().Where(x => x.Guid == vm.RiverGuid).FirstOrDefault();

            //template of such guid doesn't exist
            if (wall == null)
                return HttpDoc<EmptyHttpReponse>.NotFound("Tabbed Rivers with Guid " + vm.RiverGuid + " does not exist").ToJson();
            else
            {
                wall.LastModified = Stamp.Time();
                wall.Template.FeedLiquidTemplate = vm.FeedLiquidTemplate;
                wall.Template.WallLiquidTemplate = vm.WallLiquidTemplate;
                wall.Template.LessCss.SetText(vm.LessCss);
                wall.Template.JavaScript.SetText(vm.JavaScript);
                wall.Template.CoffeeScript.SetText(vm.CoffeeScript);

                try
                {
                    RavenSession.Store(wall);
                    return HttpDoc<dynamic>.OK(new { Message = "hello world 2" }).ToJson();
                }
                catch (Exception ex)
                {
                    return HttpDoc<EmptyHttpReponse>.InternalServerError(ex.Message).ToJson();    
                }
            }
        }
    }
}

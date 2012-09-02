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
            try
            {
                var feedTemplate = DotLiquid.Template.Parse(vm.WallLiquidTemplate);
            }
            catch (DotLiquid.Exceptions.SyntaxException se)
            {
                this.PropertyValidationMessage("WallLiquidTemplate", se.Message);
            }

            try
            {
                var feedTemplate = DotLiquid.Template.Parse(vm.FeedLiquidTemplate);
            }
            catch (DotLiquid.Exceptions.SyntaxException se)
            {
                this.PropertyValidationMessage("FeedLiquidTemplate", se.Message);
            }

            if (!ModelState.IsValid)
            {
                var errors = new ModelPropertyErrors();

                foreach (var key in ModelState.Keys) {
                    ModelState modelState = ModelState[key];
                    if (modelState.Errors.Any())
                    {
                        var r = new ModelPropertyError { Key = key };
                        foreach (ModelError error in modelState.Errors)
                        {
                            r.Errors.Add(error.ErrorMessage);
                        }
                        errors.Properties.Add(r);
                    }
                }

                return HttpDoc<EmptyHttpReponse>.PreconditionFailed(errors.ToJson()).ToJson();
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

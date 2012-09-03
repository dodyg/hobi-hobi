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
using HobiHobi.Core.Validators;
using FluentValidation.Results;
using FluentValidation.Mvc;

namespace HobiHobi.Web.Areas.Manage.Controllers
{

    public class RiverController : RavenController
    {
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(BasicRiverWallViewModel vm)
        {
            //this is for basic validation
            if (!ModelState.IsValid)
                return View(vm);

            var river = RavenSession.Query<RiverWall>().Where(x => x.Name == vm.Name).FirstOrDefault();

            if (river != null)
                this.PropertyValidationMessage("Name", string.Format("Please pick another name. {0} has already been taken.", vm.Name));

            //do another check after this 'advanced' validation
            if (!ModelState.IsValid)
                return View(vm);

            this.FlashInfo("Creation is successful man");
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

        private void ValidateRiverTemplate(RiverTemplateViewModel vm)
        {
            //validate and validate some more
            var validator = new RiverTemplateViewModelValidator();
            ValidationResult results = validator.Validate(vm);
            results.AddToModelState(ModelState, string.Empty);

            try
            {
                var feedTemplate = DotLiquid.Template.Parse(vm.WallTemplate);
            }
            catch (DotLiquid.Exceptions.SyntaxException se)
            {
                this.PropertyValidationMessage("WallLiquidTemplate", se.Message);
            }

            try
            {
                var feedTemplate = DotLiquid.Template.Parse(vm.FeedTemplate);
            }
            catch (DotLiquid.Exceptions.SyntaxException se)
            {
                this.PropertyValidationMessage("FeedLiquidTemplate", se.Message);
            }
        }

        private ModelPropertyErrors ProduceAJAXErrorMessage(ModelStateDictionary ms)
        {
            var errors = new ModelPropertyErrors();

            foreach (var key in ms.Keys)
            {
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

            return errors;
        }

        [HttpPost]//, ValidateAntiForgeryToken(Salt = SiteConstants.ANTI_FORGERY_SALT)]
        public ActionResult EditTemplate(string guid, RiverTemplateViewModel vm)
        {
            ValidateRiverTemplate(vm);

            if (!ModelState.IsValid)
            {
                var errors = ProduceAJAXErrorMessage(ModelState);
                return HttpDoc<EmptyHttpReponse>.PreconditionFailed(errors.ToJson()).ToJson();
            }

            // now that we are done with validation, get on with business

            var wall = RavenSession.Query<RiverWall>().Where(x => x.Guid == vm.RiverGuid).FirstOrDefault();

            //template of such guid doesn't exist
            if (wall == null)
                return HttpDoc<EmptyHttpReponse>.NotFound("Tabbed Rivers with Guid " + vm.RiverGuid + " does not exist").ToJson();
            else
            {
                wall.LastModified = Stamp.Time();
                wall.Template.FeedTemplate = vm.FeedTemplate;
                wall.Template.WallTemplate = vm.WallTemplate;
                wall.Template.LessCss.SetText(vm.LessCss);
                wall.Template.JavaScript.SetText(vm.JavaScript);
                wall.Template.CoffeeScript.SetText(vm.CoffeeScript);
                wall.Template.HtmlHeadInline = vm.HtmlHeadInline;
                wall.Template.HtmlBodyInline = vm.HtmlBodyInline;

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

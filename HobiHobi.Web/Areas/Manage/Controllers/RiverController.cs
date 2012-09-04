﻿using HobiHobi.Core;
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
using HobiHobi.Core.Subscriptions;
using HobiHobi.Core.Identity;
using HobiHobi.Core.Utils;

namespace HobiHobi.Web.Areas.Manage.Controllers
{
    public class RiverController : RavenController
    {
        [HttpGet]
        public ActionResult Sources(string guid)
        {
            var wall = RavenSession.Query<RiverWall>().Where(x => x.Guid == guid).FirstOrDefault();

            if (wall == null)
                return HttpNotFound();

            var sources = wall.Sources.Items;
            ViewBag.RiverGuid = wall.Guid;
            return View(sources);
        }

        [HttpPost]
        public ActionResult AddSource(string title, string uri)
        {
            if (string.IsNullOrWhiteSpace(title))
                this.PropertyValidationMessage("Title", "Title is required");

            if (string.IsNullOrWhiteSpace(uri))
                this.PropertyValidationMessage("Uri", "Uri is required");

            Uri url = null;
            try
            {
                url = new Uri(uri);
            }
            catch
            {
                this.PropertyValidationMessage("Uri", "Given Uri is invalid. Please do not forget to include http://");
            }

            if (!ModelState.IsValid)
            {
                var errors = ProduceAJAXErrorMessage(ModelState);
                return HttpDoc<EmptyHttpReponse>.PreconditionFailed(errors.ToJson()).ToJson();
            }

            return HttpDoc<dynamic>.OK(new { Message = "hello world" }).ToJson();
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken(Salt = SiteConstants.ANTI_FORGERY_SALT)]
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

            var wall = new RiverWall();
            wall.Id = RiverWall.NewId(vm.Name).Full();
            wall.Guid = Stamp.GUID().ToString();
            wall.Name = vm.Name;
            wall.Title = vm.Title;
            wall.Description = vm.Description;
            wall.Keywords = vm.Keywords;
            wall.Template = DefaultRiverTemplate.Get();
            wall.Sources = DefaultRiverSubscription.Get();

            RavenSession.Store(wall);

            //take care of the temporary account

            var transient = CookieMonster.GetFromCookie<TransientAccount>(Request.Cookies[TransientAccount.COOKIE_NAME]);
            if (!transient.IsFound)
            {
                var init = new TransientAccount();
                init.RiverGuids.Add(wall.Guid);
                Response.Cookies.Add(CookieMonster.SetCookie(init, TransientAccount.COOKIE_NAME));
            }
            else
            {
                transient.Item.RiverGuids.Add(wall.Guid);
                Response.Cookies.Add(CookieMonster.SetCookie(transient.Item, TransientAccount.COOKIE_NAME));
            }

            return Redirect("/r/" + vm.Name);
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

        [HttpPost]//, 
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

using FluentValidation.Mvc;
using FluentValidation.Results;
using HobiHobi.Core;
using HobiHobi.Core.Feeds;
using HobiHobi.Core.Framework;
using HobiHobi.Core.Identity;
using HobiHobi.Core.Subscriptions;
using HobiHobi.Core.Utils;
using HobiHobi.Core.Validators;
using HobiHobi.Core.ViewModels;
using HobiHobi.Web.Controllers;
using System;
using System.Linq;
using System.Web.Mvc;

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
            ViewBag.RiverName = wall.Name;
            return View(sources);
        }

        public string ConvertTitleToName(string title)
        {
            return title.Replace(" ", "_").Replace("#", "_").Replace("'", "_").Replace(".", "_")
                .Replace("/","_").Replace("\\","_");
        }

        [HttpPost]
        public ActionResult RemoveSource(string riverGuid, string name)
        {
            if (string.IsNullOrWhiteSpace(riverGuid))
                this.PropertyValidationMessage("RiverGuid", "A critical id is missing. Please refresh your page and try again");

            if (string.IsNullOrWhiteSpace(name))
                this.PropertyValidationMessage("Name", "Feed name is missing");

            if (!ModelState.IsValid)
            {
                var errors = ProduceAJAXErrorMessage(ModelState);
                return HttpDoc<EmptyHttpReponse>.PreconditionFailed(errors.ToJson()).ToJson();
            }

            var wall = RavenSession.Query<RiverWall>().Where(x => x.Guid == riverGuid).FirstOrDefault();

            if (wall == null)
                this.PropertyValidationMessage("RiverGuid", "The River Id is not valid. Please refresh your page.");
            else
            {
                //remove name
                wall.Sources.Items = wall.Sources.Items.Where(x => x.Name != name).ToList();

                RavenSession.Store(wall);
                this.SaveChangesAndTerminate();

                return HttpDoc<dynamic>.OK(new { Message = "Source remove", Name = name }).ToJson();
            }

            if (!ModelState.IsValid)
            {
                var errors = ProduceAJAXErrorMessage(ModelState);
                return HttpDoc<EmptyHttpReponse>.PreconditionFailed(errors.ToJson()).ToJson();
            }

            return HttpDoc<dynamic>.OK(new { Message = "Source removed", Name = name }).ToJson();
        }

        [HttpPost]
        public ActionResult AddSource(string riverGuid, string title, string uri)
        {
            if (string.IsNullOrWhiteSpace(riverGuid))
                this.PropertyValidationMessage("RiverGuid", "A critical id is missing. Please refresh your page and try again");

            if (string.IsNullOrWhiteSpace(title))
                this.PropertyValidationMessage("Title", "Title is required");

            if (string.IsNullOrWhiteSpace(uri))
                this.PropertyValidationMessage("Uri", "Uri is required");

            Uri jsonUrl = null;
            try
            {
                jsonUrl = new Uri(uri);
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

            var wall = RavenSession.Query<RiverWall>().Where(x => x.Guid == riverGuid).FirstOrDefault();

            if (wall == null)
                this.PropertyValidationMessage("RiverGuid", "The River Id is not valid. Please refresh your page.");
            else
            {
                try
                {
                    var fetcher = new RiverFetcher();
                    var content = fetcher.Download("http://" + jsonUrl.DnsSafeHost, jsonUrl.PathAndQuery);
                    var river = fetcher.Serialize(content);
                    var name = ConvertTitleToName(title);

                    wall.Sources.Items.Add(new RiverSubscriptionItem
                    {
                        Name = name,
                        Text = title,
                        JSONPUri = jsonUrl
                    });

                    RavenSession.Store(wall);
                    this.SaveChangesAndTerminate();

                    return HttpDoc<dynamic>.OK(new { Message = "Source added", Name = name }).ToJson();
                }
                catch
                {
                    this.PropertyValidationMessage("Uri", "Given Uri does not exist or is an invalid river format. Please try again.");
                }
            }

            if (!ModelState.IsValid)
            {
                var errors = ProduceAJAXErrorMessage(ModelState);
                return HttpDoc<EmptyHttpReponse>.PreconditionFailed(errors.ToJson()).ToJson();
            }

            return HttpDoc<dynamic>.OK(new { Message = "Source added" }).ToJson();
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

            this.SaveChangesAndTerminate();

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
                    this.SaveChangesAndTerminate();

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

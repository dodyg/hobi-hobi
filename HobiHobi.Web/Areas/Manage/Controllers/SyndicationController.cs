using HobiHobi.Core;
using HobiHobi.Core.Caching;
using HobiHobi.Core.Framework;
using HobiHobi.Core.Identity;
using HobiHobi.Core.Subscriptions;
using HobiHobi.Core.Syndications;
using HobiHobi.Core.Utils;
using HobiHobi.Core.ViewModels;
using HobiHobi.Web.Controllers;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HobiHobi.Web.Areas.Manage.Controllers
{
    public class SyndicationController : RavenController
    {
        [HttpGet]
        public ActionResult Sources(string guid)
        {
            var list = RavenSession.Query<SyndicationList>().Where(x => x.Guid == guid).FirstOrDefault();

            if (list == null)
                return HttpNotFound();

            var sources = list.Sources.Items;
            ViewBag.SyndicationGuid = list.Guid;
            ViewBag.SyndicationName = list.Name;
            return View(sources);
        }


        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken(Salt = SiteConstants.ANTI_FORGERY_SALT)]
        public ActionResult Create(BasicSyndicationListViewModel vm)
        {
            //this is for basic validation
            if (!ModelState.IsValid)
                return View(vm);

            var existingSyndication = RavenSession.Query<SyndicationList>().Where(x => x.Name == vm.Name).FirstOrDefault();

            if (existingSyndication != null)
                this.PropertyValidationMessage("Name", string.Format("Please pick another name. {0} has already been taken.", vm.Name));

            //do another check after this 'advanced' validation
            if (!ModelState.IsValid)
                return View(vm);

            var list = new SyndicationList();
            list.Id = SyndicationList.NewId(vm.Name).Full();
            list.Guid = Stamp.GUID().ToString();
            list.Name = vm.Name;
            list.Title = vm.Title;
            list.Description = vm.Description;
            list.Keywords = vm.Keywords;

            RavenSession.Store(list);

            //take care of the temporary account

            var transient = CookieMonster.GetFromCookie<TransientAccount>(Request.Cookies[TransientAccount.COOKIE_NAME]);
            if (!transient.IsFound)
            {
                var init = new TransientAccount();
                init.SyndicationGuids.Add(list.Guid);
                Response.Cookies.Add(CookieMonster.SetCookie(init, TransientAccount.COOKIE_NAME));
            }
            else
            {
                transient.Item.SyndicationGuids.Add(list.Guid);
                Response.Cookies.Add(CookieMonster.SetCookie(transient.Item, TransientAccount.COOKIE_NAME));
            }

            this.RavenSession.SaveChanges();

            return RedirectToAction("Sources", new { guid = list.Guid });
        }

        [HttpPost]
        public ActionResult AddSource(string syndicationGuid, string title, string uri)
        {
            if (string.IsNullOrWhiteSpace(syndicationGuid))
                this.PropertyValidationMessage("SyndicationGuid", "A critical id is missing. Please refresh your page and try again");

            if (string.IsNullOrWhiteSpace(title))
                this.PropertyValidationMessage("Title", "Title is required");

            if (string.IsNullOrWhiteSpace(uri))
                this.PropertyValidationMessage("Uri", "Uri is required");

            Uri xmlUrl = null;
            try
            {
                xmlUrl = new Uri(uri);
            }
            catch
            {
                this.PropertyValidationMessage("Uri", "Given Uri is invalid. Please do not forget to include http://");
            }

            if (!ModelState.IsValid)
            {
                var errors = this.ProduceAJAXErrorMessage(ModelState);
                return HttpDoc<EmptyHttpReponse>.PreconditionFailed(errors.ToJson()).ToJson();
            }

            var list = RavenSession.Query<SyndicationList>().Where(x => x.Guid == syndicationGuid).FirstOrDefault();

            if (list == null)
                this.PropertyValidationMessage("SyndicationGuid", "The Syndication List Id is not valid. Please refresh your page.");
            else
            {
                try
                {
                    var fetcher = new SyndicationFetcher();
                    var content = fetcher.Fetch(xmlUrl);

                    if (content.IsFound)
                    {
                        var name = Texts.ConvertTitleToName(title);

                        list.Sources.Items.Add(new RssSubscriptionItem
                        {
                            Name = name,
                            Text = title,
                            XmlUri = xmlUrl
                        });

                        RavenSession.Store(list);
                        this.RavenSession.SaveChanges();

                        SyndicationRiverJsCache.Flush(list.Name, HttpContext.Cache);

                        return HttpDoc<dynamic>.OK(new { Message = "Source added", Name = name }).ToJson();
                    }
                    else
                        this.PropertyValidationMessage("Uri", "Given Uri does not exist or is an invalid river format. Please try again.");
                }
                catch
                {
                    this.PropertyValidationMessage("Uri", "Given Uri does not exist or is an invalid river format. Please try again.");
                }
            }

            if (!ModelState.IsValid)
            {
                var errors = this.ProduceAJAXErrorMessage(ModelState);
                return HttpDoc<EmptyHttpReponse>.PreconditionFailed(errors.ToJson()).ToJson();
            }

            return HttpDoc<dynamic>.OK(new { Message = "Source added" }).ToJson();
        }

        [HttpPost]
        public ActionResult RemoveSource(string syndicationGuid, string name)
        {
            if (string.IsNullOrWhiteSpace(syndicationGuid))
                this.PropertyValidationMessage("SyndicationGuid", "A critical id is missing. Please refresh your page and try again");

            if (string.IsNullOrWhiteSpace(name))
                this.PropertyValidationMessage("Name", "Feed name is missing");

            if (!ModelState.IsValid)
            {
                var errors = this.ProduceAJAXErrorMessage(ModelState);
                return HttpDoc<EmptyHttpReponse>.PreconditionFailed(errors.ToJson()).ToJson();
            }

            var list = RavenSession.Query<SyndicationList>().Where(x => x.Guid == syndicationGuid).FirstOrDefault();

            if (list == null)
                this.PropertyValidationMessage("SyndicationGuid", "The River Id is not valid. Please refresh your page.");
            else
            {
                //remove name
                list.Sources.Items = list.Sources.Items.Where(x => x.Name != name).ToList();

                RavenSession.Store(list);
                this.RavenSession.SaveChanges();
                SyndicationRiverJsCache.Flush(list.Name, HttpContext.Cache);

                return HttpDoc<dynamic>.OK(new { Message = "Source remove", Name = name }).ToJson();
            }

            if (!ModelState.IsValid)
            {
                var errors = this.ProduceAJAXErrorMessage(ModelState);
                return HttpDoc<EmptyHttpReponse>.PreconditionFailed(errors.ToJson()).ToJson();
            }

            return HttpDoc<dynamic>.OK(new { Message = "Source removed", Name = name }).ToJson();
        }
    }
}

using HobiHobi.Core;
using HobiHobi.Core.Framework;
using HobiHobi.Core.Identity;
using HobiHobi.Core.Syndications;
using HobiHobi.Core.Utils;
using HobiHobi.Core.ViewModels;
using HobiHobi.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HobiHobi.Web.Areas.Manage.Controllers
{
    public class SyndicationController : RavenController
    {
        public ActionResult Index()
        {
            return View();
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

            this.SaveChangesAndTerminate();

            return RedirectToAction("Source", new { guid = list.Guid });
        }

    }
}

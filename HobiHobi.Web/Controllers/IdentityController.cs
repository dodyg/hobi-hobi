using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HobiHobi.Core.Framework;
using HobiHobi.Core.Identity;
using HobiHobi.Core.ViewModels;

namespace HobiHobi.Web.Controllers
{
    public class IdentityController : RavenController
    {
        public ActionResult Index()
        {
            return Content(HobiHobi.Core.Identity.User.HashPassword("hello world"));   
        }

        [HttpGet]
        public ActionResult Register()
        {
            
            return View();
        }

        [HttpPost]
        public ActionResult Register(UserViewModel user)
        {
            //make sure the email has not been used before
            var usr = this.RavenSession.Query<User>().Where(x => x.Email == user.Email).FirstOrDefault();

            if (usr != null) 
                this.PropertyValidationMessage("Email", HobiHobi.Core.Validators.Resources.UserViewModelValidator.RepeatEmail);

            if (!ModelState.IsValid)
            {
                return View(user);
            }

            var u = user.GetUserForCreation();

            RavenSession.Store(u);
            SaveChangesAndTerminate();
            this.FlashInfo(Resources.Messages.RegistrationSuccessful);
            return RedirectToAction("Index", "Home");
        }
    }
}

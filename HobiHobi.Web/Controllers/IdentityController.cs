using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HobiHobi.Core.Framework;
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
            return View();
        }
    }
}

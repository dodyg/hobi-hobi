using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HobiHobi.Core.Framework;

namespace HobiHobi.Web.Controllers
{
    public class IdentityController : Controller
    {
        public ActionResult Index()
        {
            return Content(HobiHobi.Core.Identity.User.HashPassword("hello world"));   
        }
    }
}

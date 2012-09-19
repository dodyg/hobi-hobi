using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HobiHobi.Web.Controllers
{
    public class ListController : Controller
    {
        //
        // GET: /List/

        public ActionResult Rivers()
        {
            return View();
        }

        public ActionResult Syndications()
        {
            return View();
        }
    }
}

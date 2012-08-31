using HobiHobi.Core.Feeds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HobiHobi.Web.Controllers
{
    public class RiverController : RavenController
    {
        public ActionResult Index(string name)
        {
            var id = RiverWall.NewId(name);

            var wall = RavenSession.Load<RiverWall>(id.Full());

            if (wall != null)
                return Content(name);
            else
                return Content("No wall " + name);
        }

        public ActionResult GetCss(string name)
        {
            var id = RiverWall.NewId(name);

            var wall = RavenSession.Load<RiverWall>(id.Full());

            if (wall != null)
            {
                return Content(name);
            }
            else
                return Content("No css " + name);
        }
    }
}

using HobiHobi.Core.Blogging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HobiHobi.Web.Controllers
{
    /// <summary>
    /// Deal with the public facing side of the blog
    /// </summary>
    public class BlogController : RavenController
    {
        //
        // GET: /Blog/

        public ActionResult Index(string name)
        {
            var blog = RavenSession.Query<Blog>().Where(x => x.Name == name).FirstOrDefault();

            if (blog == null)
                return HttpNotFound();

            return View();
        }

    }
}

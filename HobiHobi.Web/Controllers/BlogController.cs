using HobiHobi.Core.Blogging;
using HobiHobi.Core.Identity;
using HobiHobi.Core.Utils;
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
        public ActionResult Index(string name)
        {
            var blog = RavenSession.Query<Blog>().Where(x => x.Name == name).FirstOrDefault();

            if (blog == null)
                return HttpNotFound();

            var edit = CookieMonster.GetFromCookie<TransientAccount>(Request.Cookies[TransientAccount.COOKIE_NAME]);

            if (edit.IsFound && edit.Item.IsBlogFound(blog.Guid))
            {
                ViewBag.EditLink = "/manage/blog/?guid=" + blog.Guid;
            }

            return View();
        }

    }
}

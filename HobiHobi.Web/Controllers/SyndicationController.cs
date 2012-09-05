using HobiHobi.Core.Identity;
using HobiHobi.Core.Syndications;
using HobiHobi.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HobiHobi.Web.Controllers
{
    public class SyndicationController : RavenController
    {
        public ActionResult Index(string name)
        {
            var id = SyndicationList.NewId(name);

            var list = RavenSession.Load<SyndicationList>(id.Full());

            if (list != null)
            {
                ViewBag.Title = list.Title;
                ViewBag.Description = Server.HtmlEncode(list.Description);
                ViewBag.Keywords = Server.HtmlEncode(list.Keywords);
                ViewBag.Name = list.Name;

                var edit = CookieMonster.GetFromCookie<TransientAccount>(Request.Cookies[TransientAccount.COOKIE_NAME]);

                if (edit.IsFound && edit.Item.IsRiverFound(list.Guid))
                {
                    ViewBag.EditLink = "/manage/syndication/sources/?guid=" + list.Guid;
                }
                else
                {
                    if (!edit.IsFound)
                    {
                        var init = new TransientAccount();
                        init.SyndicationGuids.Add(list.Guid);
                        Response.Cookies.Add(CookieMonster.SetCookie(init, TransientAccount.COOKIE_NAME));
                    }
                    else //
                    {
                        edit.Item.SyndicationGuids.Add(list.Guid);
                        Response.Cookies.Add(CookieMonster.SetCookie(edit.Item, TransientAccount.COOKIE_NAME));
                    }
                }

                return View();
            }
            else
                return HttpNotFound();
        }

        public ActionResult GetOpml(string name)
        {
            var id = SyndicationList.NewId(name);
            var wall = RavenSession.Load<SyndicationList>(id.Full());

            if (wall != null)
            {
                var opml = wall.Sources.ToOpml();
                var xml = opml.ToXML();

                return Content(xml.ToString(), "text/xml");
            }
            else
                return HttpNotFound();
        }
    }
}

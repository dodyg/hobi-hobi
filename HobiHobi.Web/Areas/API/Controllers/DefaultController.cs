using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HobiHobi.Core.Subscriptions;
using HobiHobi.Core.Framework;
namespace HobiHobi.Web.Areas.API.Controllers
{
    public class DefaultController : Controller
    {
        public ActionResult RiversSubscription(string type)
        {
            var river = new RiverSubscription();
            river.Title = "Default Rivers";
            river.OwnerName = "Hobi Hobi";
            river.DateCreated = Stamp.Time();
            river.DateModified = Stamp.Time();

            river.Items.Add(new RiverSubscriptionItem
            {
               Title = "Apple River",
               Text = "Apple River",
               JSONUri = new Uri("http://static.scripting.com/houston/rivers/apple/apple.json")
            });

            river.Items.Add(new RiverSubscriptionItem
            {
                Title = "Dave River",
                Text = "Dave's River",
                JSONPUri = new Uri("http://static.scripting.com/houston/rivers/iowaRiver3.js")
            });

            river.Items.Add(new RiverSubscriptionItem
            {
                Title = "Tech River",
                Text = "Tech River (TechMeme)",
                JSONUri = new Uri("http://static.scripting.com/houston/rivers/techmeme/techmeme.json")
            });

            river.Items.Add(new RiverSubscriptionItem
            {
                Title = "NYT River",
                Text = "NYT River",
                JSONUri = new Uri("http://static.scripting.com/houston/rivers/nyt/nyt.json")
            });

            river.Items.Add(new RiverSubscriptionItem
            {
                Title = "No Agenda River",
                Text = "No Agenda River",
                JSONPUri = new Uri("http://s3.amazonaws.com/river.curry.com/rivers/radio2/River3.js")
            });

            if (type == "json")
                river.Items.RemoveAll(x => x.JSONPUri != null);
            else if (type == "jsonp")
                river.Items.RemoveAll(x => x.JSONUri != null);

            var opml = river.ToOpml();
            var xml = opml.ToXML();

            return Content(xml.ToString(), "text/xml");
        }
    }
}

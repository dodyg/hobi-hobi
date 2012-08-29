﻿using System;
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
        public ActionResult RiversSubscription()
        {
            var river = new RiverSubscription();
            river.Title = "Default Rivers";
            river.OwnerName = "Hobi Hobi";
            river.DateCreated = Stamp.Time();
            river.DateModified = Stamp.Time();

            river.Items.Add(new RiverSubscriptionItem
            {
               Text = "Apple River",
               Name = "apple",
               JSONPUri = new Uri("http://static.scripting.com/houston/rivers/apple/River3.js")
            });

            river.Items.Add(new RiverSubscriptionItem
            {
                Text = "Dave's River",
                Name = "dave",
                JSONPUri = new Uri("http://static.scripting.com/houston/rivers/iowaRiver3.js")
            });

            river.Items.Add(new RiverSubscriptionItem
            {
                Text = "Tech River (TechMeme)",
                Name = "tech",
                JSONPUri = new Uri("http://static.scripting.com/houston/rivers/techmeme/River3.js")
            });

            river.Items.Add(new RiverSubscriptionItem
            {
                Text = "NYT River",
                Name = "nyt",
                JSONPUri = new Uri("http://static.scripting.com/houston/rivers/nyt/River3.js")
            });

            river.Items.Add(new RiverSubscriptionItem
            {
                Text = "No Agenda River",
                Name = "noagenda",
                JSONPUri = new Uri("http://s3.amazonaws.com/river.curry.com/rivers/radio2/River3.js")
            });

            river.Items.Add(new RiverSubscriptionItem
            {
                Text = "East Village News",
                Name = "eastVillageNews",
                JSONPUri = new Uri("http://static.scripting.com/houston/rivers/eastVillageRiver3.js")
            });

            var opml = river.ToOpml();
            var xml = opml.ToXML();

            return Content(xml.ToString(), "text/xml");
        }
    }
}

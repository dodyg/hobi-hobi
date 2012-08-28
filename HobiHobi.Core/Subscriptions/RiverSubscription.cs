using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HobiHobi.Core.Subscriptions
{
    public class RiverSubscription
    {
        public string Title { get; set; }
        public string OwnerName { get; set; }
        public string OwnerEmail { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public List<RiverSubscriptionItem> Items { get; set; }

        public RiverSubscription()
        {
            Items = new List<RiverSubscriptionItem>();
            ParsingErrors = new List<string>();
        }

        /// <summary>
        /// List errors in parsing opml attributes
        /// </summary>
        public List<string> ParsingErrors
        {
            get;
            set;
        }

        public Opml ToOpml()
        {
            var opml = new Opml
            {
                Title = Title,
                OwnerName = OwnerName,
                OwnerEmail = OwnerEmail,
                DateCreated = DateCreated,
                DateModified = DateModified
            };

            foreach(var i in Items.Select(
                x =>
                {
                    var item = new Outline();
                    item.Attributes["text"] = x.Text;
                    item.Attributes["title"] = x.Title;
                    item.Attributes["type"] = "river";
                    if (x.JSONPUri != null)
                        item.Attributes["url"] = x.JSONPUri.ToString();
                    if (x.JSONUri != null)
                        item.Attributes["url"] = x.JSONUri.ToString();

                    return item;
                }))
            {
                    opml.Outlines.Add(i);
            }

            return opml;
        }
    }
}

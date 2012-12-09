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

        public RiverSubscription(Opml opml)
            : this()
        {
            Title = opml.Title;
            DateCreated = opml.DateCreated;
            DateModified = opml.DateModified;

            var line = 0;
            foreach (var x in opml.Outlines)
            {
                line++;
                var item = new RiverSubscriptionItem();
                foreach (var y in x.Attributes)
                {
                    try
                    {
                        if (y.Key == "text")
                            item.Text = y.Value;
                        else if (y.Key == "name")
                            item.Name = y.Value;
                        else if (y.Key == "url" && !string.IsNullOrWhiteSpace(y.Value))
                            item.JSONPUri = new Uri(y.Value);
                    }
                    catch (Exception ex)
                    {
                        ParsingErrors.Add("Error at line " + line + " in processing attribute " 
                            + y.Key + " with value " + y.Value + " " +  ex.Message);
                    }
                }

                Items.Add(item);
            }
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
                    item.Attributes["type"] = "river";
                    if (!string.IsNullOrWhiteSpace(x.Name))
                        item.Attributes["name"] = x.Name;
                    if (!string.IsNullOrWhiteSpace(x.Description))
                        item.Attributes["description"] = x.Description;
                    if (x.JSONPUri != null)
                        item.Attributes["url"] = x.JSONPUri.ToString();
                    if (!string.IsNullOrWhiteSpace(x.Language))
                        item.Attributes["language"] = x.Language;
                    return item;
                }))
            {
                    opml.Outlines.Add(i);
            }

            return opml;
        }
    }
}

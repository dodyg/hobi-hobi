using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HobiHobi.Core.Subscriptions
{
    /// <summary>
    /// Hold information of an RSS subscription list written in OPML format
    /// </summary>
    public class RssSubscription
    {
        public string Title { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public List<RssSubscriptionItem> Items { get; set; }

        public RssSubscription()
        {
            Items = new List<RssSubscriptionItem>();
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

        public RssSubscription(Opml opml): this()
        {
            Title = opml.Title;
            DateCreated = opml.DateCreated;
            DateModified = opml.DateModified;

            var line = 0;
            foreach (var x in opml.Outlines)
            {
                line++;
                var item = new RssSubscriptionItem();
                foreach (var y in x.Attributes)
                {
                    try
                    {
                        if (y.Key == "text")
                            item.Text = y.Value;
                        else if (y.Key == "description")
                            item.Text = y.Value;
                        else if (y.Key == "title")
                            item.Text = y.Value;
                        else if (y.Key == "name")
                            item.Name = y.Value;
                        else if (y.Key == "htmlUrl" && !string.IsNullOrWhiteSpace(y.Value))
                            item.HtmlUri = new Uri(y.Value);
                        else if (y.Key == "xmlUrl" && !string.IsNullOrWhiteSpace(y.Value))
                            item.XmlUri = new Uri(y.Value);
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
    }
}

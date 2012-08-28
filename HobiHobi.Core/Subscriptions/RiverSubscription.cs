using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HobiHobi.Core.Subscriptions
{
    public class RiverSubscription
    {
        public string Title { get; set; }
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

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HobiHobi.Core.Subscriptions
{
    public class RssSubscriptionItem
    {
        public string Text { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Uri HtmlUri { get; set; }
        public Uri XmlUri { get; set; }
    }
}

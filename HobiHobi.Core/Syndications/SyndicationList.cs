using HobiHobi.Core.Framework;
using HobiHobi.Core.Subscriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HobiHobi.Core.Syndications
{
    /// <summary>
    /// Hold the information of a syndication list
    /// </summary>
    public class SyndicationList
    {
        public static Key NewId(string value)
        {
            return Key.Generate("SyndicationList/", value);
        }

        public string Id { get; set; }
        public string Guid { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }

        public List<string> Participants { get; set; }
        public RssSubscription Sources { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime LastModified { get; set; }

        public SyndicationList()
        {
            Participants = new List<string>();
            Sources = new RssSubscription();
            DateCreated = Stamp.Time();
            LastModified = Stamp.Time();
        }
    }
}

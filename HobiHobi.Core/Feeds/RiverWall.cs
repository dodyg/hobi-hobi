using HobiHobi.Core.Framework;
using HobiHobi.Core.Subscriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HobiHobi.Core.Feeds
{
    /// <summary>
    /// Hold the information of the feedwall
    /// </summary>
    public class RiverWall
    {
        public static Key NewId(string value)
        {
            return Key.Generate("RiverWall/", value);
        }

        public string Id { get; set; }
        public string Guid { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }

        public List<string> Participants { get; set; }
        public RiverTemplate Template { get; set; }
        public RiverSubscription Sources { get; set; }
        public RiverWallVisibility Visibility { get; set; }
        public RiverWallStatus Status { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime LastModified { get; set; }

        public RiverWall()
        {
            Participants = new List<string>();
            Sources = new RiverSubscription();
            Template = new RiverTemplate();
            Visibility = RiverWallVisibility.Public;
            Status = RiverWallStatus.Draft;
            DateCreated = Stamp.Time();
            LastModified = Stamp.Time();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotLiquid;
using HobiHobi.Core.Subscriptions;

namespace HobiHobi.Core.Drops
{
    public class RiverSubscriptionItemDrop : Drop
    {
        public string Text { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string JsonP { get; set; }

        public RiverSubscriptionItemDrop(RiverSubscriptionItem item)
        {
            Text = item.Text;
            Name = item.Name;
            Description = item.Description;
            JsonP = item.JSONPUri.ToString();
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HobiHobi.Core.Subscriptions
{
    public class RiverSubscriptionItem
    {
        public string Text { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Language { get; set; }
        public Uri JSONPUri { get; set; }
    }
}

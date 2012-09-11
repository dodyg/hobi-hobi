using HobiHobi.Core.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HobiHobi.Core.Site
{
    public class BrokenLink
    {
        public static Key NewId(Uri uri)
        {
            var key = uri.ToString().Replace("/", "-").Replace("?", "-").Replace(".", "_");
            return Key.Generate("BrokenLink/", key);
        }

        public BrokenLink()
        {
            Attempts = new List<BrokenAttempt>();
        }

        public string Id { get; set; }
        public string Uri { get; set; }
        public BrokenLinkType Type { get; set; }
        public List<BrokenAttempt> Attempts { get; set; }
    }

}

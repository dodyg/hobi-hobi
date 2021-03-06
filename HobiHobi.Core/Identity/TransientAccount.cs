﻿using HobiHobi.Core.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Newtonsoft.Json;

namespace HobiHobi.Core.Identity
{
    [Serializable]
    public class TransientAccount
    {
        public const string COOKIE_NAME = "TransientAccountInfo";

        public List<string> RiverGuids { get; set; }
        public List<string> SyndicationGuids { get; set; }
        public List<string> BlogGuids { get; set; }
        public List<string> OpmlGuids { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime LastModified { get; set; }

        public TransientAccount()
        {
            DateCreated = Stamp.Time();
            LastModified = Stamp.Time();
            RiverGuids = new List<string>();
            SyndicationGuids = new List<string>();
            BlogGuids = new List<string>();
            OpmlGuids = new List<string>();
        }

        public bool IsRiverFound(string guid)
        {
            return RiverGuids.Any(x => x == guid);
        }

        public bool IsSyndicationListFound(string guid)
        {
            return SyndicationGuids.Any(x => x == guid);
        }

        public bool IsBlogFound(string guid)
        {
            return BlogGuids.Any(x => x == guid);
        }

        public bool IsOpmlFound(string guid)
        {
            return OpmlGuids.Any(x => x == guid);
        }

        public void MarkUpdated()
        {
            LastModified = DateTime.UtcNow;
        }
    }
}

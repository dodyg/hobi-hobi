using HobiHobi.Core.Framework;
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
        public DateTime DateCreated { get; set; }
        public DateTime LastModified { get; set; }

        public TransientAccount()
        {
            DateCreated = Stamp.Time();
            LastModified = Stamp.Time();
            RiverGuids = new List<string>();
        }

        public bool IsFound(string guid)
        {
            return RiverGuids.Any(x => x == guid);
        }
    }
}

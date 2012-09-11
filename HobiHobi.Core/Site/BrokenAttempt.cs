using HobiHobi.Core.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HobiHobi.Core.Site
{
    public class BrokenAttempt
    {
        public DateTime DateCreated { get; set; }
        public string ErrorMessage { get; set; }

        public BrokenAttempt()
        {
            DateCreated = Stamp.Time();
        }
    }
}

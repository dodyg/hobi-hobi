using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HobiHobi.Core.Feeds
{
    public class LessCssBundledText : BundledText
    {
        public LessCssBundledText()
            : base((txt) => dotless.Core.Less.Parse(txt))
        {
        }

    }
}

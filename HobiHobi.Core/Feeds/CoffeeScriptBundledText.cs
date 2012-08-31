using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HobiHobi.Core.Feeds
{
    public class CoffeeScriptBundledText : BundledText
    {
        public CoffeeScriptBundledText() : base((txt) => new CoffeeSharp.CoffeeScriptEngine().Compile(txt))
        {

        }
    }
}

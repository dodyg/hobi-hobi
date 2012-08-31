using HobiHobi.Core.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HobiHobi.Core.Feeds
{
    /// <summary>
    /// Hold template information for a wall
    /// </summary>
    public class RiverTemplate
    {
        public static Key NewId(string value = null)
        {
            if (value == null)
                value = Key.GenerateGuid();

            return Key.Generate("RiverTemplate/", value);
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string AuthorName { get; set; }
        public string Url { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime LastModified { get; set; }

        public string LiquidTemplate { get; set; }
        public BundledText JavaScript { get; set; }
        public BundledText CoffeeScript { get; set; }
        public BundledText LessCss { get; set; }
        public string HtmlHeadInline { get; set; }
        public string HtmlBodyInline { get; set; }
        
        public bool AllowClone { get; set; }

        public RiverTemplate()
        {
            JavaScript = new BundledText();
            CoffeeScript = new BundledText();
            LessCss = new BundledText();
            DateCreated = Stamp.Time();
            LastModified = Stamp.Time();
            AllowClone = true;
        }
    }
}

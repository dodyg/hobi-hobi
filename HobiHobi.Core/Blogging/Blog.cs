using HobiHobi.Core.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HobiHobi.Core.Blogging
{
    public class Blog
    {
        public static Key NewId(string value)
        {
            return Key.Generate("Blog/", value);
        }

        public string Id { get; set; }
        public string Guid { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }

        public DateTime DateCreated { get; set; }

        public Blog()
        {
            DateCreated = Stamp.Time();
        }
    }
}

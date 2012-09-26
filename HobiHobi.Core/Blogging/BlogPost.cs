using HobiHobi.Core.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HobiHobi.Core.Blogging
{
    public class BlogPost
    {
        public static Key NewId(string value = null)
        {
            return Key.Generate("Blog/Feed/Post/", value);
        }

        public string Id { get; set; }
        public string Slug { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }
        public string FeedId { get; set; }
        public string Link { get; set; }
        public string ShortLink { get; set; }
        public bool AllowComment { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<string> Tags { get; set; }

        public BlogPost()
        {
            DateCreated = Stamp.Time();
            Tags = new List<string>();
            AllowComment = true;
            IsDeleted = false;
        }
    }
}

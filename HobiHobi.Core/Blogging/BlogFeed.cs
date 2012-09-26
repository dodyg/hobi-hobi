using HobiHobi.Core.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HobiHobi.Core.Blogging
{
    public class BlogFeed
    {
        public static Key NewId(string blogName, string value)
        {
            return Key.Generate(string.Format("Blog/{0}/Feed/", blogName), value);
        }

        public string Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Guid { get; set; }
        public string BlogId { get; set; }
        public DateTime DateCreated { get; set; }
        public string Description { get; set; }

        public BlogFeed()
        {
            DateCreated = Stamp.Time();
        }

        public BlogPost NewPost(string content, string title = null, string link = null)
        {
            if (Id.IsNullOrWhiteSpace())
                throw new ApplicationException("Id must exist to perform this operation");

            var post = new BlogPost
            {
                Title = title,
                Content = content,
                Link = link
            };

            return post;
        }
    }
}

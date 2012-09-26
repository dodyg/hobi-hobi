using HobiHobi.Core.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HobiHobi.Core.Blogging
{
    public class BlogFeed
    {
        public static Key NewId(string value = null)
        {
            return Key.Generate("Blog/Feed/", value);
        }

        public string Id { get; set; }
        public string Guid { get; set; }
        public string BlogId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime LastModified { get; set; }
        
        public BlogFeed()
        {
            DateCreated = Stamp.Time();
            LastModified = Stamp.Time();
        }

        public BlogPost NewPost(string content, string title = null, string link = null)
        {
            if (Id.IsNullOrWhiteSpace())
                throw new ApplicationException("Id must exist to perform this operation");

            var post = new BlogPost
            {
                Id = BlogPost.NewId().Full(),
                FeedId = this.Id,
                Title = title,
                Content = content,
                Link = link
            };

            return post;
        }

        public static bool CheckIfNameExist(Raven.Client.IDocumentSession session, string url)
        {
            return session.Query<BlogFeed>().Where( x => x.Url == url.ToLower()).Any();
        }
    }
}

using HobiHobi.Core.Framework;
using HobiHobi.Core.Utils;
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
        public List<string> BlogFeedIds { get; set; }

        public DateTime DateCreated { get; set; }

        public Blog()
        {
            DateCreated = Stamp.Time();
            BlogFeedIds = new List<string>();
        }

        /// <summary>
        /// Create a new feed related to this blog
        /// </summary>
        /// <param name="name"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public BlogFeed CreateFeed(string name, string title, string description)
        {
            if (Id.IsNullOrWhiteSpace())
                throw new ApplicationException("This blog must have an id before performing a create feed operation");

            var feed = new BlogFeed
            {
                Id = GetDefaultFeedId(),
                Title = title,
                Description = description,
                Guid = Stamp.GUID().ToString(),
                BlogId = this.Id,
                Url = Texts.ConvertTitleToName(title)
            };

            BlogFeedIds.Add(feed.Id);

            return feed;
        } 

        /// <summary>
        /// Create a default feed for this blog
        /// </summary>
        /// <returns></returns>
        public BlogFeed CreateDefaultFeed()
        {
            return CreateFeed(this.Name, this.Title, this.Description);
        }

        public string GetDefaultFeedId()
        {
            return BlogFeed.NewId("Default-" + Name).Full();
        }
    }
}

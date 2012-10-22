using HobiHobi.Core.Framework;
using HobiHobi.Core.Subscriptions;
using HobiHobi.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

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


        /// <summary>
        /// Path to display this blog in HTML
        /// </summary>
        public string HtmlLink
        {
            get
            {
                return "/b/" + this.Name;
            }
        }


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
        public BlogFeed CreateFeed(string title, string description, string newId = null)
        {
            if (Id.IsNullOrWhiteSpace())
                throw new ApplicationException("This blog must have an id before performing a create feed operation");

            if (newId == null)
                newId = BlogFeed.NewId().Full();

            var feed = new BlogFeed
            {
                Id = newId,
                Guid = Stamp.GUID().ToString(),
                BlogId = this.Id,
                Title = title,
                Url = ConvertFeedTitleToUrl(title)
            };

            BlogFeedIds.Add(feed.Id);

            return feed;
        }

        public void RemoveFeed(string feedId)
        {
            BlogFeedIds.Remove(feedId);
        }

        public List<BlogFeed> GetFeeds(Raven.Client.IDocumentSession session)
        {
            if (Id.IsNullOrWhiteSpace())
                throw new ApplicationException("This blog must have an id before performing GetFeeds operations");

            var feeds = session.Query<BlogFeed>().Where(x => x.BlogId == this.Id).OrderBy(x => x.Title).ToList();

            return feeds;
        }

        public Opml GetFeedsOpml(List<BlogFeed> feeds, HttpRequestBase request)
        {
            if (Id.IsNullOrWhiteSpace())
                throw new ApplicationException("This blog must have an id before performing GetFeedsOpml operations");

            var opml = new Opml
            {
                Title = Title,
                DateCreated = DateCreated,
            };


            foreach (var i in feeds.Select(
                x =>
                {
                    var item = new Outline();
                    item.Attributes["text"] = x.Title;
                    item.Attributes["type"] = "rss";
                    item.Attributes["name"] = Texts.ConvertTitleToName(x.Title);
                    if (!string.IsNullOrWhiteSpace(x.Description))
                        item.Attributes["description"] = x.Description;
                    item.Attributes["htmlUrl"] = x.GetHtmlLink(request);
                    item.Attributes["xmlUrl"] = x.GetRssLink(request);

                    return item;
                }))
            {
                opml.Outlines.Add(i);
            }


            return opml;
        }

        /// <summary>
        /// Use this method to convert a given feed title to a url
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public string ConvertFeedTitleToUrl(string title)
        {
            return Texts.ConvertTitleToUrl(title);
        }

        /// <summary>
        /// Create a default feed for this blog
        /// </summary>
        /// <returns></returns>
        public BlogFeed CreateDefaultFeed()
        {
            return CreateFeed(this.Title, this.Description, GetDefaultFeedId());
        }

        public string GetDefaultFeedId()
        {
            return BlogFeed.NewId("Default-" + Name).Full();
        }
    }
}

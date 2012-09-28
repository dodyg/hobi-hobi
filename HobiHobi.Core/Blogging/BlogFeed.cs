using HobiHobi.Core.Framework;
using Raven.Client.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Xml;
using System.Xml.Linq;

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
        public string ETag { get; set; }

        List<BlogPost> _posts;

        public List<BlogPost> Posts
        {
            get
            {
                return _posts;
            }
        }

        public BlogFeed()
        {
            DateCreated = Stamp.Time();
            LastModified = Stamp.Time();
            _posts = new List<BlogPost>();
            MarkEtag();
        }

        public void MarkEtag()
        {
            ETag = Stamp.GUID().ToString();
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

        public static bool CheckIfUrlExist(Raven.Client.IDocumentSession session, string url)
        {
            return session.Query<BlogFeed>().Where( x => x.Url == url.ToLower()).Any();
        }

        public static IQuerySetOne<BlogFeed> FindByUrl(Raven.Client.IDocumentSession session, string url)
        {
            var feed = session.Query<BlogFeed>().Where(x => x.Url == url.ToLower()).FirstOrDefault();

            return new QuerySetOne<BlogFeed>(feed);
        }

        public void LoadRss(Raven.Client.IDocumentSession session, int page = 0, int pageSize = 30)
        {
            RavenQueryStatistics stats;

            _posts = session.Query<BlogPost>()
                .Statistics(out stats)
                .Where(x => x.FeedId == this.Id)
                .OrderByDescending(x => x.DatePublished)
                .Take(pageSize)
                .Skip((page - 1)*pageSize)
                .ToList();
        }

        public SyndicationFeed GetRss()
        {
            var feed = new SyndicationFeed();
            feed.Title = new TextSyndicationContent(Title);
            feed.Description = new TextSyndicationContent(Description);
            
            var microBlogNs = new XmlQualifiedName("microblog", "http://www.w3.org/2000/xmlns/");
            feed.AttributeExtensions.Add(microBlogNs, "http://microblog.reallysimple.org/");

            XNamespace micro = "http://microblog.reallysimple.org/";

            var items = new List<SyndicationItem>();
            foreach (var p in Posts)
            {
                var item = new SyndicationItem();
                item.Title = new TextSyndicationContent(p.Title);
                item.Content = new TextSyndicationContent(p.Content);
                item.PublishDate = p.DatePublished;
                item.LastUpdatedTime = p.LastModified;
                
                if (!p.Link.IsNullOrWhiteSpace())
                    item.Links.Add(new SyndicationLink(new Uri(p.Link)));

                item.ElementExtensions.Add(new XElement(micro + "linkFull", p.Link));
                items.Add(item);
            }
            
            feed.Items = items;

            return feed;
        }
    }
}

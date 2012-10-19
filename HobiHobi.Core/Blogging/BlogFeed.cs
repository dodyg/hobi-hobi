using HobiHobi.Core.Framework;
using HobiHobi.Core.Syndications;
using HobiHobi.Core.Utils;
using Raven.Client.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace HobiHobi.Core.Blogging
{
    /// <summary>
    /// This is the class that manage the concept of a blog's feed. A blog can contain multiple feed and each feed represent a single RSS channel.
    /// </summary>
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
        public string Author { get; set; }
        public string Copyright { get; set; }
        public string Language { get; set; }

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
            _posts = new List<BlogPost>();
            MarkAsUpdated();
        }

        public void MarkAsUpdated()
        {
            ETag = Stamp.GUID().ToString();
            this.LastModified = Stamp.Time();
        }

        /// <summary>
        /// Prepare a new post. Beware that this operation contains a call to Google url shorterner service
        /// </summary>
        /// <param name="content"></param>
        /// <param name="title"></param>
        /// <param name="link"></param>
        /// <returns></returns>
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
            };

            if (!link.IsNullOrWhiteSpace())
            {
                post.Link = link;
                var shortenerResult = UrlShortener.GetGoogle(link);
                if (shortenerResult.IsTrue)
                    post.ShortLink = shortenerResult.Value;
            }

            post.GenerateSlug();

            this.MarkAsUpdated();

            return post;
        }

        /// <summary>
        /// Check if an existing BlogFeed by a given slug already exists. This is necessary because feed slug is universal per installation of this software.
        /// We could have scoped this by using blog name but it will make the link too long and nasty e.g http://hobieu.apphb.com/b/my-blog-name/f/feed-name
        /// </summary>
        /// <param name="session"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool CheckIfUrlExist(Raven.Client.IDocumentSession session, string url)
        {
            return session.Query<BlogFeed>().Where(x => x.Url == url.ToLower()).Any();
        }

        /// <summary>
        /// Find a BlogFeed by its url slug e.g. in the form of /f/{slug} such as /f/my-best-news
        /// </summary>
        /// <param name="session"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static IQuerySetOne<BlogFeed> FindByUrl(Raven.Client.IDocumentSession session, string url)
        {
            var feed = session.Query<BlogFeed>().Where(x => x.Url == url.ToLower()).FirstOrDefault();

            return new QuerySetOne<BlogFeed>(feed);
        }

        /// <summary>
        /// Fetch the posts related to this blog post by using paging. The posts are automatically sorted descending by post date published.
        /// </summary>
        /// <param name="session"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        public void LoadRss(Raven.Client.IDocumentSession session, int page = 1, int pageSize = 30)
        {
            RavenQueryStatistics stats;

            _posts = session.Query<BlogPost>()
                .Statistics(out stats)
                .Where(x => x.FeedId == this.Id)
                .OrderByDescending(x => x.DatePublished)
                .Take(pageSize)
                .Skip((page - 1) * pageSize)
                .ToList();
        }

        const short RSS_TTL = 10;

        /// <summary>
        /// Build RSS 2.0 output based on the content of this BlogFeed. Make sure to call LoadRss to load all the items related to this blog feed otherwise 
        /// the output RSS will not contain any items.
        /// </summary>
        /// <param name="currentHost"></param>
        /// <returns></returns>
        public SyndicationFeed GetRss(Uri currentHost)
        {
            var host = Utils.Texts.FromUriHost(currentHost);
            var alternativeLink = host + "/f/" + this.Url;

            //prepare the channel information
            var feed = new SyndicationFeed(Title, Description, new Uri(alternativeLink));
            feed.LastUpdatedTime = this.LastModified;
            feed.Generator = "HobiHobi";

            if (!Author.IsNullOrWhiteSpace())
                feed.Authors.Add(new SyndicationPerson(Author));

            feed.ElementExtensions.Add("docs", "", "http://blogs.law.harvard.edu/tech/rss");
            feed.ElementExtensions.Add("ttl", "", RSS_TTL);
            //feed.ElementExtensions.Add("cloud", "", "oi");

            if (!Copyright.IsNullOrWhiteSpace())
                feed.Copyright = new TextSyndicationContent(Copyright);

            if (!Language.IsNullOrWhiteSpace())
                feed.Language = Language;

            //specify microblogging extension
            XNamespace micro = "http://microblog.reallysimple.org/";
            var microBlogNs = new XmlQualifiedName("microblog", "http://www.w3.org/2000/xmlns/");
            feed.AttributeExtensions.Add(microBlogNs, micro.ToString());

            //Do the feed item
            var items = new List<SyndicationItem>();
            foreach (var p in Posts)
            {
                var item = new SyndicationItem();
                if (!p.Title.IsNullOrWhiteSpace())
                    item.Title = new TextSyndicationContent(p.Title);
                item.Content = new TextSyndicationContent(p.Content);
                item.PublishDate = p.DatePublished;
                item.Id = host + "/f/" + this.Url + "/" + p.Slug;

                var author = p.GetAuthorForRss();
                if (author != null)
                    item.Authors.Add(author);

                //Do not use item.Links.Add since it uses ATOM namespace for multiple links
                if (!p.ShortLink.IsNullOrWhiteSpace())
                {
                    item.ElementExtensions.Add("link", "", p.ShortLink);
                    item.ElementExtensions.Add(new XElement(micro + "linkFull", p.Link));
                }
                else if (!p.Link.IsNullOrWhiteSpace())
                {
                    item.ElementExtensions.Add("link", "", p.Link);
                }


                items.Add(item);
            }

            feed.Items = items;
            return feed;
        }

        public RssJs GetRssJs(Uri currentHost)
        {
            var host = Utils.Texts.FromUriHost(currentHost);
            var alternativeLink = host + "/f/" + this.Url;

            var rss = new RssJs
            {
                Rss = new RssJs.RssRoot{
                    Channel = new RssJs.RssRoot.RssChannel
                    {
                        Title = Title,
                        Description = Description,
                        Link = alternativeLink,
                        LastBuildDate = LastModified.ToString("R"),
                        PubDate = LastModified.ToString("R"),
                        Ttl = RSS_TTL,
                        Generator = "Hobi Hobi",
                        Items = Posts.Select( x =>
                            {
                                var item = new RssJs.RssRoot.RssChannel.RssItem
                                {
                                    Description = x.Content,
                                    Guid = host + "/f/" + this.Url + "/" + x.Slug,
                                    PubDate = x.DatePublished.ToString("R")
                                };

                                if (!x.ShortLink.IsNullOrWhiteSpace())
                                    item.Link = x.ShortLink;
                                else
                                    item.Link = x.Link;

                                if (!Title.IsNullOrWhiteSpace())
                                    item.Title = x.Title;

                                return item;
                            }
                        ).ToList()
                    }
                }
            };


            return rss;
        }

        /// <summary>
        /// Get the RssJs version of this blog feed
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public string GetRssJsLink(HttpRequestBase request)
        {
            var link = Texts.FromUriHost(request.Url) + "/f/rssjs/" + this.Url;
            return link;
        }

        /// <summary>
        /// Get the Rss version of this blog feed
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public string GetRssLink(HttpRequestBase request)
        {
            var link = Texts.FromUriHost(request.Url) + "/f/rss/" + this.Url;
            return link;
        }

        /// <summary>
        /// Get the HTML rendition of this blog feed
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public string GetHtmlLink(HttpRequestBase request)
        {
            var link = Texts.FromUriHost(request.Url) + "/f/" + this.Url;
            return link;
        }
    }
}

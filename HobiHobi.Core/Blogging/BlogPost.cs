using HobiHobi.Core.Framework;
using HobiHobi.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
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
        public string Author { get; set; }
        public string AuthorEmail { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DatePublished { get; set; }
        public DateTime LastModified { get; set; }
        public string FeedId { get; set; }
        public string Link { get; set; }
        public string ShortLink { get; set; }
        public bool AllowComment { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<string> Tags { get; set; }

        public string PreferredLink
        {
            get
            {
                if (!ShortLink.IsNullOrWhiteSpace())
                    return ShortLink;
                else if (!Link.IsNullOrWhiteSpace())
                    return Link;
                else
                    return string.Empty;
            }
        }

        public BlogPost()
        {
            DateCreated = Stamp.Time();
            DatePublished = Stamp.Time();
            LastModified = Stamp.Time();
            Tags = new List<string>();
            AllowComment = true;
            IsDeleted = false;
        }

        public SyndicationPerson GetAuthorForRss()
        {
            if (Author.IsNullOrWhiteSpace() && AuthorEmail.IsNullOrWhiteSpace())
                return null;
            else if (Author.IsNullOrWhiteSpace())
                return new SyndicationPerson(AuthorEmail);
            else
                return new SyndicationPerson(AuthorEmail, Author, string.Empty);
        }

        /// <summary>
        /// Generate a slug based on the information available for this blog post (whether a title exists or not)
        /// </summary>
        public void GenerateSlug()
        {
            if (Title.IsNullOrWhiteSpace())
                GenerateNumberSlug();
            else
                GenerateTitleSlug();
        }

        /// <summary>
        /// Generate a slug based on the title of the post
        /// </summary>
        public void GenerateTitleSlug()
        {
            Slug = Texts.ConvertTitleToUrl(Title);
        }

        /// <summary>
        /// Generate number based slug
        /// </summary>
        public void GenerateNumberSlug()
        {
            var ticks = DateTime.UtcNow.Ticks.ToString();
            Slug = ticks;
        }

        public void AddTag(string tag)
        {
            Tags.Add(tag);
        }

        public void RemoveTag(string tag)
        {
            Tags = Tags.Where(x => x != tag).ToList();
        }

        public static IQuerySetOne<BlogPost> GetByUrl(Raven.Client.IDocumentSession session, string url)
        {
            var item = session.Query<BlogPost>().Where(x => x.Slug == url).FirstOrDefault();
            return new QuerySetOne<BlogPost>(item);
        }

        public static IQuerySetMany<BlogPost> GetByFeedId(Raven.Client.IDocumentSession session, string feedId, int page = 0, int pageSize = 30)
        {
            var items = session.Query<BlogPost>().Where(x => x.FeedId == feedId)
                .OrderByDescending(x => x.DatePublished)
                .Take(pageSize).Skip((page - 1) * pageSize).ToList();

            return new QuerySetMany<BlogPost>(items);
        }

        public static Result<None> Delete(Raven.Client.IDocumentSession session, string feedId, string blogPostId)
        {
            try
            {
                var feed = session.Load<BlogFeed>(feedId);

                if (feed == null)
                    return None.False(new ArgumentException("Feed Id is not found"));

                var post = session.Load<BlogPost>(blogPostId);

                if (post == null)
                    return None.False(new ArgumentException("Blog Id is not found"));

                session.Delete(post);
                feed.MarkAsUpdated();
                session.Store(feed);
                session.SaveChanges();

                return None.True();
            }
            catch (Exception ex)
            {
                return None.False(ex);
            }
        }
    }
}

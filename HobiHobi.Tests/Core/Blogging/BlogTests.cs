using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using HobiHobi.Core.Blogging;

namespace HobiHobi.Tests.Core.Blogging
{
    public class BlogTests
    {
        const string TEST_BLOG_GUID = "fa4c395d-e6da-4b52-992d-5d33809ce631";

        [Test, Explicit]
        public void LoadBlog()
        {
            using (var store = Raven.GetStoreFromServer())
            {
                using (var session = store.OpenSession(Raven.DATABASE_NAME))
                {
                    var blog = session.Query<Blog>().Where(x => x.Guid == TEST_BLOG_GUID).FirstOrDefault();

                    Assert.IsTrue(blog != null, "Blog cannot be found");
                    Assert.IsNotNullOrEmpty(blog.Name, "Name cannot be null");
                    Assert.IsNotNullOrEmpty(blog.Title, "Title cannot be null");
                    Assert.IsNotNullOrEmpty(blog.Description, "Descriptioin cannot be null");
                    Assert.IsNotNullOrEmpty(blog.Guid, "Guid cannot be null");
                    Assert.IsTrue(blog.BlogFeedIds.Count() > 0, "There must be at least 1 blog feed");
                }
            }
        }

        [Test, Explicit]
        public void ClearBlog()
        {
            using (var store = Raven.GetStoreFromServer())
            {
                using (var session = store.OpenSession(Raven.DATABASE_NAME))
                {
                    var blog = session.Query<Blog>().Where(x => x.Guid == TEST_BLOG_GUID).First();

                    foreach (var feed in blog.BlogFeedIds)
                    {
                        session.Advanced.DatabaseCommands.Delete(feed, null);
                    }

                    blog.BlogFeedIds.Clear();
                    session.Store(blog);
                    session.SaveChanges();
                }
            }
        }
        
        [Test, Explicit]
        public void CreateDefaultFeed()
        {
            using (var store = Raven.GetStoreFromServer())
            {
                using (var session = store.OpenSession(Raven.DATABASE_NAME))
                {
                    var blog = session.Query<Blog>().Where(x => x.Guid == TEST_BLOG_GUID).First();

                    var feed = blog.CreateDefaultFeed();

                    session.Store(blog);
                    session.Store(feed);

                    session.SaveChanges();
                }
            }
        }

        [Test, Explicit]
        public void LoadDefaultFeed()
        {
            using (var store = Raven.GetStoreFromServer())
            {
                using (var session = store.OpenSession(Raven.DATABASE_NAME))
                {
                    var blog = session.Query<Blog>().Where(x => x.Guid == TEST_BLOG_GUID).First();

                    var defaultFeedId = blog.GetDefaultFeedId();

                    var feed = session.Load<BlogFeed>(defaultFeedId);

                    Assert.IsTrue(feed != null, defaultFeedId + " must load");
                    Assert.IsNotNullOrEmpty(feed.Title, "Title cannot be null");
                    Assert.IsNotNullOrEmpty(feed.BlogId, "Blog id cannot be null");
                    Assert.IsNotNullOrEmpty(feed.Description, "Description cannot be null");
                    Assert.IsNotNullOrEmpty(feed.Url, "Url cannot be null");
                }
            }
        }

        [Test, Explicit]
        public void AddNewPost()
        {
            using (var store = Raven.GetStoreFromServer())
            {
                using (var session = store.OpenSession(Raven.DATABASE_NAME))
                {
                    var blog = session.Query<Blog>().Where(x => x.Guid == TEST_BLOG_GUID).First();

                    var defaultFeedId = blog.GetDefaultFeedId();

                    var feed = session.Load<BlogFeed>(defaultFeedId);

                    Assert.IsTrue(feed != null, defaultFeedId + " must load");

                    var post = feed.NewPost("Hello world");

                    session.Store(post);
                    session.SaveChanges();
                }
            }
        }
    }
}

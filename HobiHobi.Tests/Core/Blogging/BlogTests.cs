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
        public void AddNewFeed()
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
                }
            }
        }
    }
}

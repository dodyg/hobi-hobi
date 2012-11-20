using HobiHobi.Core;
using HobiHobi.Core.Blogging;
using HobiHobi.Core.ViewModels;
using HobiHobi.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HobiHobi.Core.Framework;
using HobiHobi.Core.Utils;
using HobiHobi.Core.Identity;

namespace HobiHobi.Web.Areas.Manage.Controllers
{
    public class BlogController : RavenController
    {

        [HttpGet]
        public ActionResult Index(string guid)
        {
            var blog = RavenSession.Query<Blog>().Where(x => x.Guid == guid).FirstOrDefault();

            if (blog == null)
                return HttpNotFound();

            var feeds = RavenSession.Query<BlogFeed>().Where(x => x.BlogId == blog.Id).OrderBy(x => x.Title).ToList();

            ViewBag.BlogId = blog.Id;
            ViewBag.BlogTitle = blog.Title;
            ViewBag.Feeds = feeds;
            ViewBag.BlogLink = blog.HtmlLink;
            ViewBag.IsSecureConnection = this.IfSecureConnectionOnAppHarbor();
            ViewBag.SecureUrl = Texts.ConvertToSecureVersion(Request.Url);
            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken(Salt = SiteConstants.ANTI_FORGERY_SALT)]
        public ActionResult Create(BasicBlogViewModel vm)
        {
            //this is for basic validation
            if (!ModelState.IsValid)
                return View(vm);

            var existingBlog = RavenSession.Query<Blog>().Where(x => x.Name == vm.Name).FirstOrDefault();

            if (existingBlog != null)
                this.PropertyValidationMessage("Name", string.Format("Please pick another name. {0} has already been taken.", vm.Name));

            //do another check after this 'advanced' validation
            if (!ModelState.IsValid)
                return View(vm);

            var blog = new Blog();
            blog.Id = Blog.NewId(vm.Name).Full();
            blog.Guid = Stamp.GUID().ToString();
            blog.Name = vm.Name;
            blog.Title = vm.Title;
            blog.Description = vm.Description;
            blog.Keywords = vm.Keywords;

            RavenSession.Store(blog);

            //take care of the temporary account

            var transient = CookieMonster.GetFromCookie<TransientAccount>(Request.Cookies[TransientAccount.COOKIE_NAME]);
            if (!transient.IsFound)
            {
                var init = new TransientAccount();
                init.BlogGuids.Add(blog.Guid);
                Response.Cookies.Add(CookieMonster.SetCookie(init, TransientAccount.COOKIE_NAME));
            }
            else
            {
                transient.Item.BlogGuids.Add(blog.Guid);
                Response.Cookies.Add(CookieMonster.SetCookie(transient.Item, TransientAccount.COOKIE_NAME));
            }

            this.RavenSession.SaveChanges();

            return Redirect("/b/" + blog.Name);
        }

        [HttpGet]
        public ActionResult GetPosts(string feedId, int page = SiteConstants.INITIAL_PAGE, int pageSize = SiteConstants.MAXIMUM_PAGE_SIZE)
        {
            var posts = BlogPost.GetByFeedId(RavenSession, feedId, page, pageSize);

            if (!posts.IsFound)
            {
                return HttpDoc<IEnumerable<BlogPost>>.OK(new List<BlogPost>()).ToJson();
            }
            else
            {
                return HttpDoc<IEnumerable<BlogPost>>.OK(posts.Items).ToJson();
            }
        }

        [HttpPost]
        public ActionResult CreatePost(string feedId, string content, string title = null, string link = null)
        {
            var feed = RavenSession.Load<BlogFeed>(feedId);

            if (feed == null)
                return HttpDoc<EmptyHttpReponse>.OK(EmptyHttpReponse.Instance).ToJson();

            var post = feed.NewPost(content, title: title, link:link);
        
            RavenSession.Store(post);
            RavenSession.Store(feed);
            RavenSession.SaveChanges();

            return HttpDoc<BlogPost>.OK(post).ToJson();
        }

        [HttpPost]
        public ActionResult DeletePost(string feedId, string postId)
        {
            try
            {
                var res = BlogPost.Delete(RavenSession, feedId, postId);
                
                if (res.IsTrue)
                    return HttpDoc<None>.OK(None.True()).ToJson();
                else
                    return HttpDoc<None>.InternalServerError(res.Message).ToJson();
            }
            catch (Exception ex)
            {
                return HttpDoc<None>.OK(None.False(ex)).ToJson();
            }
        }

        [HttpPost]
        public ActionResult CreateFeed(string blogId, string title, string description)
        {
            var blog = RavenSession.Load<Blog>(blogId);

            if (blog == null)
                return HttpDoc<EmptyHttpReponse>.OK(EmptyHttpReponse.Instance).ToJson();

            var titleUrl = blog.ConvertFeedTitleToUrl(title);

            var isUrlExist = BlogFeed.CheckIfUrlExist(RavenSession, titleUrl);

            if (isUrlExist)
                return HttpDoc<EmptyHttpReponse>.PreconditionFailed("The url generated from the title is '" + titleUrl + "'. Unfortunately this is already taken. Please change your title and try again").ToJson(); 

            var feed = blog.CreateFeed(title, description);

            RavenSession.Store(blog);
            RavenSession.Store(feed);
            RavenSession.SaveChanges();

            return HttpDoc<BlogFeed>.OK(feed).ToJson();
        }

        [HttpPost]
        public ActionResult DeleteFeed(string feedId)
        {
            try
            {
                var feed = RavenSession.Load<BlogFeed>(feedId);
                if (feed == null)
                    return HttpDoc<EmptyHttpReponse>.NotFound("Feed Id is not found").ToJson();

                var blog = RavenSession.Load<Blog>(feed.BlogId);

                if (blog == null)
                    return HttpDoc<EmptyHttpReponse>.NotFound("Blog Id from Feed is not found").ToJson();

                blog.RemoveFeed(feed.Id);

                RavenSession.Store(blog);
                RavenSession.Delete(feed);
                RavenSession.SaveChanges();

                return HttpDoc<EmptyHttpReponse>.OK(EmptyHttpReponse.Instance).ToJson();
            }
            catch (Exception ex)
            {
                return HttpDoc<EmptyHttpReponse>.InternalServerError(ex.Message).ToJson();
            }
        }
    }
}

using System.Web.Mvc;
using System.Web.Routing;

namespace HobiHobi.Web.Startup
{
    public static class Routing
    {
        static string[] _rootNamespace = new string[] { "HobiHobi.Web.Controllers" };

        public static void Configure(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "BlogFeedRssJs", // Route name
                "f/rssjs/{slug}", // URL with parameters
                new { controller = "Blog", action = "FeedRssJs" }, // Parameter defaults
                _rootNamespace
            );

            routes.MapRoute(
                "BlogFeedRss", // Route name
                "f/rss/{slug}", // URL with parameters
                new { controller = "Blog", action = "FeedRss" }, // Parameter defaults
                _rootNamespace
            );

            routes.MapRoute(
                "BlogFeedItem", // Route name
                "f/{feedSlug}/{postSlug}", // URL with parameters
                new { controller = "Blog", action = "FeedItem" }, // Parameter defaults
                _rootNamespace
            );

            routes.MapRoute(
                "BlogFeed", // Route name
                "f/{slug}", // URL with parameters
                new { controller = "Blog", action = "Feed" }, // Parameter defaults
                _rootNamespace
            );

            routes.MapRoute(
                "BlogSubscriptionFeedList", // Route name
                "b/opml/{name}", // URL with parameters
                new { controller = "Blog", action = "BlogOpmlSubscriptionList" }, // Parameter defaults
                _rootNamespace
            );


            routes.MapRoute(
                "Blog", // Route name
                "b/{name}", // URL with parameters
                new { controller = "Blog", action = "Index" }, // Parameter defaults
                _rootNamespace
            );

            routes.MapRoute(
                "Syndication", // Route name
                "s/{name}", // URL with parameters
                new { controller = "Syndication", action = "Index" }, // Parameter defaults
                _rootNamespace
            );

            routes.MapRoute(
                "SyndicationOpml", // Route name
                "s/opml/{name}", // URL with parameters
                new { controller = "Syndication", action = "GetOpml" }, // Parameter defaults
                _rootNamespace
            );

            routes.MapRoute(
                "SyndicationRiverJs", // Route name
                "s/riverjs/{name}", // URL with parameters
                new { controller = "Syndication", action = "GetRiverJs" }, // Parameter defaults
                _rootNamespace
            );

            routes.MapRoute(
                "RiverFeedName", // Route name
                "r/feed/{name}/{feedname}", // URL with parameters
                new { controller = "River", action = "GetFeed" }, // Parameter defaults
                _rootNamespace
            );

            routes.MapRoute(
                "RiverOpml", // Route name
                "r/opml/{name}", // URL with parameters
                new { controller = "River", action = "GetOpml" }, // Parameter defaults
                _rootNamespace
            );

            routes.MapRoute(
                "RiverCss", // Route name
                "r/css/{name}/{etag}", // URL with parameters
                new { controller = "River", action = "GetCss" }, // Parameter defaults
                _rootNamespace
            );

            routes.MapRoute(
                "RiverJs", // Route name
                "r/js/{name}/{etag}", // URL with parameters
                new { controller = "River", action = "GetJs" }, // Parameter defaults
                _rootNamespace
            );

            routes.MapRoute(
                "River", // Route name
                "r/{name}", // URL with parameters
                new { controller = "River", action = "Index" }, // Parameter defaults
                _rootNamespace
            );

            routes.MapRoute(
                "UploadOpmlFile", // Route name
                "opml/uploadopml/{id}", // URL with parameters
                new { controller = "Opml", action = "UploadOpml", id = UrlParameter.Optional }, // Parameter defaults
                _rootNamespace
            );

            routes.MapRoute(
                "RenderOpmlEditor", // Route name
                "opml/xml/{id}", // URL with parameters
                new { controller = "Opml", action = "RenderXml" }, // Parameter defaults
                _rootNamespace
            );

            routes.MapRoute(
                "NewOpmlEditor", // Route name
                "opml/new", // URL with parameters
                new { controller = "Opml", action = "New" }, // Parameter defaults
                _rootNamespace
            );

            routes.MapRoute(
                "LatestOPML", // Route name
                "opml/latest", // URL with parameters
                new { controller = "Opml", action = "Latest" }, // Parameter defaults
                _rootNamespace
            );

            routes.MapRoute(
                "OpmlEditor", // Route name
                "opml/{id}", // URL with parameters
                new { controller = "Opml", action = "Index", id = UrlParameter.Optional }, // Parameter defaults
                _rootNamespace
            );


            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }, // Parameter defaults
                _rootNamespace
            );

        }
    }
}
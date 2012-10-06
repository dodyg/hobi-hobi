using HobiHobi.Core.Framework;
using System;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace HobiHobi.Web.Controllers
{
    /// <summary>
    /// Controll css/js
    /// </summary>
    public class SiteController : RavenController
    {
        public ActionResult Css()
        {
            var combinedUrls = new string[]{
                "/content/bootstrap.min",
                "/content/bootstrap-responsive.min",
                "/content/site"
            };

            var css = HttpContext.Cache["CSS"] as string;

            if (css == null)
            {
                var combined = CombineFiles(combinedUrls, ".css", MvcApplication.CommonCssTag);
                HttpContext.Cache.Add("CSS", combined,
                    dependencies: null,
                    absoluteExpiration: System.Web.Caching.Cache.NoAbsoluteExpiration,
                    slidingExpiration: new TimeSpan(10, 0, 0, 0),
                    onRemoveCallback: null,
                    priority: System.Web.Caching.CacheItemPriority.Normal);

                this.CompressAndSetLongExpirationCache();
                return Content(combined, "text/css");
            }
            else
            {
                this.CompressAndSetLongExpirationCache();
                return Content(css, "text/css");
            }
        }

        public ActionResult Js()
        {
            var combinedUrls = new string[]{
                "/scripts/json2",
                "/scripts/underscore-min",
                "/scripts/jquery-1.8.0.min",
                "/scripts/modernizr-2.5.3",
                "/scripts/bootstrap.min",
                "/scripts/jquery.isotope.min",
                "/scripts/jquery.unobtrusive-ajax.min",
                "/scripts/jquery.validate.min",
                "/scripts/jquery.validate.unobtrusive.min",
                "/scripts/jquery.slug",
                "/scripts/jquery.xml2json",
                "/scripts/angular.min"
            };

            var js = HttpContext.Cache["JS"] as string;

            if (js == null)
            {
                var combined = CombineFiles(combinedUrls, ".js", MvcApplication.CommonCssTag);
                HttpContext.Cache.Add("JS", combined,
                    dependencies: null,
                    absoluteExpiration: System.Web.Caching.Cache.NoAbsoluteExpiration,
                    slidingExpiration: new TimeSpan(10, 0, 0, 0),
                    onRemoveCallback: null,
                    priority: System.Web.Caching.CacheItemPriority.Normal);

                this.CompressAndSetLongExpirationCache();
                return Content(combined, "application/javascript");
            }
            else
            {
                this.CompressAndSetLongExpirationCache();
                return Content(js, "application/javascript");
            }
        }

        public string CombineFiles(string[] urls, string extension, string tag)
        {
            var paths = new string[urls.Length];

            for (int i = 0; i < urls.Length; i++)
            {
                paths[i] = Server.MapPath(urls[i] + extension);
            }

            var combined = new StringBuilder();
            combined.Append("/* tag " + tag + " Generated " + DateTime.UtcNow.ToString() + " */\n");

            for (int i = 0; i < urls.Length; i++)
            {
                combined.Append("/* Source File " + urls[i] + " */\n\n");
                combined.Append(System.IO.File.ReadAllText(paths[i]));
            }

            return combined.ToString();
        }
    }
}

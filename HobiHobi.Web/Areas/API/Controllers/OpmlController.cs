using HobiHobi.Core.Utils;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HobiHobi.Core.Framework;
using System.Web.Caching;

namespace HobiHobi.Web.Areas.API.Controllers
{
    public class OpmlController : Controller
    {
        public ActionResult Root()
        {
            var cacheKey = "RemoteOpmlDirectoryList";

            var opml = HttpContext.Cache[cacheKey] as String;
            var mime = "text/xml";

            //If there's a specific request to disable caching, handle it
            //This is useful in the process of validating to make sure remotely stored OPML file is edited properly
            if (Request.QueryString["disable-cache"].Exists())
                opml = null;

            if (opml == null)
            {
                try
                {
                    var remoteOpmlFile = new Uri(System.Configuration.ConfigurationManager.AppSettings["Source.RootOpmlUrl"]);

                    var client = new RestClient(Texts.FromUriHost(remoteOpmlFile));

                    var request = new RestRequest(remoteOpmlFile.PathAndQuery, method: Method.GET);
                    request.AddHeader("Accept", "*/*");
                    request.RequestFormat = DataFormat.Xml;

                    var response = client.Execute(request);
                    opml = response.Content;

                    HttpContext.Cache.Add(cacheKey, opml, null, DateTime.Now.AddHours(12), Cache.NoSlidingExpiration, CacheItemPriority.High, null);  
                }
                catch
                {
                    opml = "Error in retrieving opml document";
                    mime = "text/plain";
                }
            }
            
            this.Compress();
            return Content(opml, mime);
        }
    }
}

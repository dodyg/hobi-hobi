using HobiHobi.Core.Caching;
using HobiHobi.Core.Framework;
using HobiHobi.Core.Syndications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using System.Xml;

namespace HobiHobi.Web.Areas.API.Controllers
{
    public class RssController : Controller
    {
        public ActionResult Slim(string url, int maxSize)
        {
            try
            {
                var cacheKey = url;
                var cachedFeed = this.HttpContext.Cache[cacheKey] as CacheItem<SyndicationFeed>;
                string cachedEtagValue = String.Empty;
                
                if (cachedFeed != null && cachedFeed.ETags.ContainsKey(maxSize.ToString()))
                    cachedEtagValue = cachedFeed.ETags[maxSize.ToString()]; 
                
                string cachedEtag = this.FormatEtag(cachedEtagValue);
                string ifNoneMatch = this.GetEtag();

                if (!ifNoneMatch.IsNullOrWhiteSpace() && ifNoneMatch == cachedEtag)
                {
                    return new HttpStatusCodeResult(304, "Not Modified");
                }

                SyndicationFeed feed;

                if (cachedFeed == null)
                {
                    var fetcher = new SyndicationFetcher();
                    var content = fetcher.Fetch(new Uri(url));

                    if (content.IsFound)
                    {
                        feed = Filter(content.Item, maxSize);
                        string newEtag = Guid.NewGuid().ToString();

                        var newCacheItem = new CacheItem<SyndicationFeed>(content.Item);
                        newCacheItem.ETags.Add(maxSize.ToString(), newEtag);

                        //cache item for 6 hours
                        HttpContext.Cache.Add(cacheKey, newCacheItem, null, Cache.NoAbsoluteExpiration, new TimeSpan(6, 0, 0), CacheItemPriority.Default, null);
                        
                        this.Response.Cache.SetCacheability(HttpCacheability.Public);
                        this.Response.Cache.SetETag(this.FormatEtag(newEtag));
                    }
                    else
                    {
                        return HttpNotFound();
                    }
                }
                else
                {
                    feed = Filter(cachedFeed.Item, maxSize);
                    if (cachedEtagValue.IsNullOrWhiteSpace())
                    {
                        string newEtag = Guid.NewGuid().ToString();
                        if (cachedFeed.ETags.ContainsKey(maxSize.ToString()))
                            cachedFeed.ETags[maxSize.ToString()] = newEtag;
                        else
                            cachedFeed.ETags.Add(maxSize.ToString(), newEtag);

                        HttpContext.Cache[cacheKey] = cachedFeed; //refresh the cached item with new etag values
                        this.Response.Cache.SetCacheability(HttpCacheability.Public);
                        this.Response.Cache.SetETag(this.FormatEtag(newEtag));
                    }
                }

                var rssOutput = new StringBuilder();
                using (var xml = XmlWriter.Create(rssOutput))
                {
                    feed.SaveAsRss20(xml);
                }

                //Read http://baleinoid.com/whaly/2009/07/xmlwriter-and-utf-8-encoding-without-signature/
                var payload = rssOutput.ToString().Replace("encoding=\"utf-16\"", ""); //remove the Processing Instruction encoding mark for the xml body = it's a hack I know

                this.Compress();
                return Content(payload, "application/rss+xml");
            }
            catch (Exception ex)
            {
                return HttpDoc<EmptyHttpReponse>.PreconditionFailed(ex.Message).ToJson();
            }
        }

        public SyndicationFeed Filter(SyndicationFeed input, int maxSize)
        {
            if (maxSize < 0)
                maxSize = 0;

            //we have to bloody manually copy the object since nothing else works (tried binaryclone and json serialize/deserialize)
            var output = new SyndicationFeed();
            output.Title = input.Title;
            output.Description = input.Description;
            output.Language = input.Language;
            output.LastUpdatedTime = input.LastUpdatedTime;
            foreach (var l in input.Links)
                output.Links.Add(l);

            foreach (var c in input.Contributors)
                output.Contributors.Add(c);

            foreach (var x in input.ElementExtensions)
                output.ElementExtensions.Add(x);

            foreach (var c in input.Categories)
                output.Categories.Add(c);

            foreach (var a in input.Authors)
                output.Authors.Add(a);

            foreach (var a in input.AttributeExtensions)
                output.AttributeExtensions.Add(a.Key, a.Value);

            output.Copyright = input.Copyright;
            output.BaseUri = input.BaseUri;
            output.Generator = input.Generator;
            output.Id = input.Id;
            output.ImageUrl = input.ImageUrl;

            output.Items = input.Items.Take(maxSize);

            return output;
        }
    }
}

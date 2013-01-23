using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;

namespace HobiHobi.Web.Areas.API.Controllers
{
    //namespace tricks comes from http://stackoverflow.com/questions/5149426/adding-namespaces-to-a-syndicationfeed-rather-than-individual-elements
    public class UpdatesController : Controller
    {
        const string RSS_SOFTWARE_UPDATES_EXTENSION = "http://rivers.silverkeytech.com/en/mobile-app-updates";
        [HttpGet]
        public ActionResult AndroidRivers()
        {
            var feed = new SyndicationFeed();
            feed.Language = "en";
            feed.Description = new TextSyndicationContent("This is the update feed for Android Rivers", TextSyndicationContentKind.Plaintext);
            feed.Title = new TextSyndicationContent("Android Rivers Update Feed", TextSyndicationContentKind.Plaintext);
            feed.Items = new SyndicationItem[]{
                Update("Android Rivers 1.02 released", 1002, 
                @"
- Fix sorting issue that is case insensitive
                 ",
                  new DateTime(2013, 1, 20).ToUniversalTime(),
                  new Uri("http://goo.gl/Vo2bx"))
            };

            feed.AttributeExtensions.Add(new XmlQualifiedName("mobileapp", XNamespace.Xmlns.ToString()), RSS_SOFTWARE_UPDATES_EXTENSION);

            var rssOutput = new StringBuilder();
            using (var xml = XmlWriter.Create(rssOutput))
            {
                feed.SaveAsRss20(xml);
            }

            //Read http://baleinoid.com/whaly/2009/07/xmlwriter-and-utf-8-encoding-without-signature/
            var payload = rssOutput.ToString().Replace("encoding=\"utf-16\"", ""); //remove the Processing Instruction encoding mark for the xml body = it's a hack I know
            return Content(payload, "application/rss+xml"); 
        }

        SyndicationItem Update(string title, int version, string releaseNotes, DateTime publishDate, Uri freeDownloadUrl)
        {
            var item = new SyndicationItem();
            item.Title = new TextSyndicationContent(title, TextSyndicationContentKind.Plaintext);
            item.PublishDate = publishDate;
            item.Summary = new TextSyndicationContent(releaseNotes, TextSyndicationContentKind.Plaintext);
            item.Links.Add(SyndicationLink.CreateMediaEnclosureLink(freeDownloadUrl, "application/vnd.android.package-archive", 0));
            XNamespace xs = RSS_SOFTWARE_UPDATES_EXTENSION;
            item.ElementExtensions.Add(new XElement(xs + "version", version));
            return item;
        }
    }
}

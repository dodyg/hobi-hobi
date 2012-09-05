using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using HobiHobi.Core.Syndications;
using HobiHobi.Core.Framework;
using HobiHobi.Core.Subscriptions;

namespace HobiHobi.Tests.Core.Syndications
{
    [TestFixture]
    public class SyndicationListTests
    {
        [Test]
        public void SaveSyndicationList()
        {
            var syndication = new SyndicationList();
            syndication.Id = SyndicationList.NewId("dodyg").Full();
            syndication.Guid = Stamp.GUID().ToString();
            syndication.Name = "dodyg";
            syndication.Title = "Dody's Syndication Wall";
            syndication.Description = "Amazing Wall";
            syndication.Keywords = "dody, syndication";
            var opml = new Opml();
            opml.LoadFromXML(_sampleOPML);
            syndication.Sources = new RssSubscription(opml);

            using (var store = Raven.GetStoreFromServer())
            {
                using (var session = store.OpenSession(Raven.DATABASE_NAME))
                {
                    session.Store(syndication);
                    session.SaveChanges();
                }
            }
        }


        string _sampleOPML =
@"<?xml version=""1.0"" encoding=""ISO-8859-1""?>
<opml version=""2.0"">
	<head>
		<title>mySubscriptions.opml</title>
		<dateCreated>Sat, 18 Jun 2005 12:11:52 GMT</dateCreated>
		<dateModified>Tue, 02 Aug 2005 21:42:48 GMT</dateModified>
		<ownerName>Dave Winer</ownerName>
		<ownerEmail>dave@scripting.com</ownerEmail>
		<expansionState></expansionState>
		<vertScrollState>1</vertScrollState>
		<windowTop>61</windowTop>
		<windowLeft>304</windowLeft>
		<windowBottom>562</windowBottom>
		<windowRight>842</windowRight>
		</head>
	<body>
		<outline text=""CNET News.com"" description=""Tech news and business reports by CNET News.com. Focused on information technology, core topics include computers, hardware, software, networking, and Internet media."" htmlUrl=""http://news.com.com/"" language=""unknown"" title=""CNET News.com"" type=""rss"" version=""RSS2"" xmlUrl=""http://news.com.com/2547-1_3-0-5.xml""/>
		<outline text=""washingtonpost.com - Politics"" description=""Politics"" htmlUrl=""http://www.washingtonpost.com/wp-dyn/politics?nav=rss_politics"" language=""unknown"" title=""washingtonpost.com - Politics"" type=""rss"" version=""RSS2"" xmlUrl=""http://www.washingtonpost.com/wp-srv/politics/rssheadlines.xml""/>
		<outline text=""Scobleizer: Microsoft Geek Blogger"" description=""Robert Scoble's look at geek and Microsoft life."" htmlUrl=""http://radio.weblogs.com/0001011/"" language=""unknown"" title=""Scobleizer: Microsoft Geek Blogger"" type=""rss"" version=""RSS2"" xmlUrl=""http://radio.weblogs.com/0001011/rss.xml""/>
		<outline text=""Yahoo! News: Technology"" description=""Technology"" htmlUrl=""http://news.yahoo.com/news?tmpl=index&amp;cid=738"" language=""unknown"" title=""Yahoo! News: Technology"" type=""rss"" version=""RSS2"" xmlUrl=""http://rss.news.yahoo.com/rss/tech""/>
		<outline text=""Workbench"" description=""Programming and publishing news and comment"" htmlUrl=""http://www.cadenhead.org/workbench/"" language=""unknown"" title=""Workbench"" type=""rss"" version=""RSS2"" xmlUrl=""http://www.cadenhead.org/workbench/rss.xml""/>
		<outline text=""InfoWorld: Top News"" description=""The latest on Top News from InfoWorld"" htmlUrl=""http://www.infoworld.com/news/index.html"" language=""unknown"" title=""InfoWorld: Top News"" type=""rss"" version=""RSS2"" xmlUrl=""http://www.infoworld.com/rss/news.xml""/>
		<outline text=""NYT &gt; Business"" description=""Find breaking news &amp; business news on Wall Street, media &amp; advertising, international business, banking, interest rates, the stock market, currencies &amp; funds."" htmlUrl=""http://www.nytimes.com/pages/business/index.html?partner=rssnyt"" language=""unknown"" title=""NYT &gt; Business"" type=""rss"" version=""RSS2"" xmlUrl=""http://www.nytimes.com/services/xml/rss/nyt/Business.xml""/>
		<outline text=""NYT &gt; Technology"" description="""" htmlUrl=""http://www.nytimes.com/pages/technology/index.html?partner=rssnyt"" language=""unknown"" title=""NYT &gt; Technology"" type=""rss"" version=""RSS2"" xmlUrl=""http://www.nytimes.com/services/xml/rss/nyt/Technology.xml""/>
		<outline text=""Scripting News"" description=""It's even worse than it appears."" htmlUrl=""http://www.scripting.com/"" language=""unknown"" title=""Scripting News"" type=""rss"" version=""RSS2"" xmlUrl=""http://www.scripting.com/rss.xml""/>
		</body>
	</opml>";
    }
}

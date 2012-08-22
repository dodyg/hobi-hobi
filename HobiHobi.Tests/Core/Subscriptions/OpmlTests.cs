﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HobiHobi.Core.Subscriptions;
using NUnit.Framework;

namespace HobiHobi.Tests.Core.Subscriptions
{
    [TestFixture]
    public class OpmlTests
    {      
        [Test]
        public void LoadFromXML()
        {
            var opml = new Opml();
            opml.LoadFromXML(_sampleOPML);
            Console.Out.WriteLine("Hello " + opml.Title);
            Assert.IsNotNull(opml.Title);
            Assert.IsTrue(opml.Title == "mySubscriptions.opml");
            Assert.IsTrue(opml.OwnerEmail == "dave@scripting.com");
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
		<outline text=""Christian Science Monitor | Top Stories"" description=""Read the front page stories of csmonitor.com."" htmlUrl=""http://csmonitor.com"" language=""unknown"" title=""Christian Science Monitor | Top Stories"" type=""rss"" version=""RSS"" xmlUrl=""http://www.csmonitor.com/rss/top.rss""/>
		<outline text=""Dictionary.com Word of the Day"" description=""A new word is presented every day with its definition and example sentences from actual published works."" htmlUrl=""http://dictionary.reference.com/wordoftheday/"" language=""unknown"" title=""Dictionary.com Word of the Day"" type=""rss"" version=""RSS"" xmlUrl=""http://www.dictionary.com/wordoftheday/wotd.rss""/>
		<outline text=""The Motley Fool"" description=""To Educate, Amuse, and Enrich"" htmlUrl=""http://www.fool.com"" language=""unknown"" title=""The Motley Fool"" type=""rss"" version=""RSS"" xmlUrl=""http://www.fool.com/xml/foolnews_rss091.xml""/>
		<outline text=""InfoWorld: Top News"" description=""The latest on Top News from InfoWorld"" htmlUrl=""http://www.infoworld.com/news/index.html"" language=""unknown"" title=""InfoWorld: Top News"" type=""rss"" version=""RSS2"" xmlUrl=""http://www.infoworld.com/rss/news.xml""/>
		<outline text=""NYT &gt; Business"" description=""Find breaking news &amp; business news on Wall Street, media &amp; advertising, international business, banking, interest rates, the stock market, currencies &amp; funds."" htmlUrl=""http://www.nytimes.com/pages/business/index.html?partner=rssnyt"" language=""unknown"" title=""NYT &gt; Business"" type=""rss"" version=""RSS2"" xmlUrl=""http://www.nytimes.com/services/xml/rss/nyt/Business.xml""/>
		<outline text=""NYT &gt; Technology"" description="""" htmlUrl=""http://www.nytimes.com/pages/technology/index.html?partner=rssnyt"" language=""unknown"" title=""NYT &gt; Technology"" type=""rss"" version=""RSS2"" xmlUrl=""http://www.nytimes.com/services/xml/rss/nyt/Technology.xml""/>
		<outline text=""Scripting News"" description=""It's even worse than it appears."" htmlUrl=""http://www.scripting.com/"" language=""unknown"" title=""Scripting News"" type=""rss"" version=""RSS2"" xmlUrl=""http://www.scripting.com/rss.xml""/>
		<outline text=""Wired News"" description=""Technology, and the way we do business, is changing the world we know. Wired News is a technology - and business-oriented news service feeding an intelligent, discerning audience. What role does technology play in the day-to-day living of your life? Wired News tells you. How has evolving technology changed the face of the international business world? Wired News puts you in the picture."" htmlUrl=""http://www.wired.com/"" language=""unknown"" title=""Wired News"" type=""rss"" version=""RSS"" xmlUrl=""http://www.wired.com/news_drop/netcenter/netcenter.rdf""/>
		</body>
	</opml>";
    }
}

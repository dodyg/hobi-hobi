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
		<outline text=""NYT &gt; Business"" description=""Find breaking news &amp; business news on Wall Street, media &amp; advertising, international business, banking, interest rates, the stock market, currencies &amp; funds."" htmlUrl=""http://www.nytimes.com/pages/business/index.html?partner=rssnyt"" language=""unknown"" title=""NYT &gt; Business"" type=""rss"" version=""RSS2"" xmlUrl=""http://www.nytimes.com/services/xml/rss/nyt/Business.xml""/>
		<outline text=""NYT &gt; Technology"" description="""" htmlUrl=""http://www.nytimes.com/pages/technology/index.html?partner=rssnyt"" language=""unknown"" title=""NYT &gt; Technology"" type=""rss"" version=""RSS2"" xmlUrl=""http://www.nytimes.com/services/xml/rss/nyt/Technology.xml""/>
		<outline text=""Scripting News"" description=""It's even worse than it appears."" htmlUrl=""http://www.scripting.com/"" language=""unknown"" title=""Scripting News"" type=""rss"" version=""RSS2"" xmlUrl=""http://www.scripting.com/rss.xml""/>
		</body>
	</opml>";
    }
}

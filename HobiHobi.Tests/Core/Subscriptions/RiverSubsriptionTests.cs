using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using HobiHobi.Core.Subscriptions;

namespace HobiHobi.Tests.Core.Subscriptions
{
    [TestFixture]
    public class RiverSubsriptionTests
    {
        [Test]
        public void LoadFromOPML()
        {
            var opml = new Opml();
            var res = opml.LoadFromXML(_sample);
            Assert.IsTrue(res.IsTrue, "OPML loading must be true, not " + res.Message);

            var subscription = new RiverSubscription(opml);

            Assert.IsTrue(subscription.Items.Count > 0, "River must contains items");

            Assert.IsTrue(subscription.Items.Count > 0);
            var item1 = subscription.Items.First();
            Assert.IsNotNullOrEmpty(item1.Title);
            Assert.IsNotNullOrEmpty(item1.Text);
//            Assert.IsNotNullOrEmpty(item1.Name); //depending on sample data
//            Assert.IsNotNullOrEmpty(item1.Description); //depending on sample data
            Assert.IsNotNullOrEmpty(item1.JSONPUri.ToString());
        }

        string _sample = @"<?xml version=""1.0"" encoding=""ISO-8859-1""?>
<opml version=""2.0"">
<head>
<title>Default Rivers</title>
<dateCreated>Wed, 29 Aug 2012 09:05:58 GMT</dateCreated>
<dateModified>Wed, 29 Aug 2012 09:05:58 GMT</dateModified>
<ownerName>Hobi Hobi</ownerName>
</head>
<body>
<outline text=""Dave's River"" title=""Dave River"" type=""river"" url=""http://static.scripting.com/houston/rivers/iowaRiver3.js""/>
<outline text=""No Agenda River"" title=""No Agenda River"" type=""river"" url=""http://s3.amazonaws.com/river.curry.com/rivers/radio2/River3.js""/>
</body>
</opml>
";
    }
}

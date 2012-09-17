using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using HobiHobi.Core.Utils;

namespace HobiHobi.Tests.Core.Utils
{
    [TestFixture]
    public class TextsTests
    {
        [Test]
        public void TestTruncateHTMLSafeish()
        {
            // Case where we just make it to start of HREF (so effectively an empty link)

            // 'simple' nested none attributed tags
            Assert.AreEqual(@"<h1>1234</h1><b><i>56789</i>012</b>",
            Texts.TruncateHTMLSafeishChar(
                @"<h1>1234</h1><b><i>56789</i>012345</b>",
                12));

            // In middle of a!
            Assert.AreEqual(@"<h1>1234</h1><a href=""testurl""><b>567</b></a>",
            Texts.TruncateHTMLSafeishChar(
                @"<h1>1234</h1><a href=""testurl""><b>5678</b></a><i><strong>some italic nested in string</strong></i>",
                7));

            // more
            Assert.AreEqual(@"<div><b><i><strong>1</strong></i></b></div>",
            Texts.TruncateHTMLSafeishChar(
                @"<div><b><i><strong>12</strong></i></b></div>",
                1));

            // br
            Assert.AreEqual(@"<h1>1 3 5</h1><br />6",
            Texts.TruncateHTMLSafeishChar(
                @"<h1>1 3 5</h1><br />678<br />",
                6));
        }

        [Test]
        public void TestTruncateHTMLSafeishWord()
        {
            // zero case
            Assert.AreEqual(@" ...",
                            Texts.TruncateHTMLSafeishWord(
                                @"",
                               5));

            // 'simple' nested none attributed tags
            Assert.AreEqual(@"<h1>one two <br /></h1><b><i>three  ...</i></b>",
            Texts.TruncateHTMLSafeishWord(
                @"<h1>one two <br /></h1><b><i>three </i>four</b>",
                3), "we have added ' ...' to end of summary");

            // In middle of a!
            Assert.AreEqual(@"<h1>one two three </h1><a href=""testurl""><b class=""mrclass"">four  ...</b></a>",
            Texts.TruncateHTMLSafeishWord(
                @"<h1>one two three </h1><a href=""testurl""><b class=""mrclass"">four five </b></a><i><strong>some italic nested in string</strong></i>",
                4));

            // start of h1
            Assert.AreEqual(@"<h1>one two three  ...</h1>",
            Texts.TruncateHTMLSafeishWord(
                @"<h1>one two three </h1><a href=""testurl""><b>four five </b></a><i><strong>some italic nested in string</strong></i>",
                3));

            // more than words available
            Assert.AreEqual(@"<h1>one two three </h1><a href=""testurl""><b>four five </b></a><i><strong>some italic nested in string</strong></i> ...",
            Texts.TruncateHTMLSafeishWord(
                @"<h1>one two three </h1><a href=""testurl""><b>four five </b></a><i><strong>some italic nested in string</strong></i>",
                99));
        }

        [Test]
        public void TestTruncateHTMLSafeishWordXML()
        {
            // zero case
            Assert.AreEqual(@" ...",
                            Texts.TruncateHTMLSafeishWord(
                                @"",
                               5));

            // 'simple' nested none attributed tags
            string output = Texts.TruncateHTMLSafeishCharXML(
                @"<body><h1>one two </h1><b><i>three </i>four</b></body>",
                13);
            Assert.AreEqual(@"<body>\r\n  <h1>one two </h1>\r\n  <b>\r\n    <i>three</i>\r\n  </b>\r\n</body>", output,
             "XML version, no ... yet and addeds '\r\n  + spaces?' to format document");

            // In middle of a!
            Assert.AreEqual(@"<h1>one two three </h1><a href=""testurl""><b class=""mrclass"">four  ...</b></a>",
            Texts.TruncateHTMLSafeishCharXML(
                @"<body><h1>one two three </h1><a href=""testurl""><b class=""mrclass"">four five </b></a><i><strong>some italic nested in string</strong></i></body>",
                4));

            // start of h1
            Assert.AreEqual(@"<h1>one two three  ...</h1>",
            Texts.TruncateHTMLSafeishCharXML(
                @"<h1>one two three </h1><a href=""testurl""><b>four five </b></a><i><strong>some italic nested in string</strong></i>",
                3));

            // more than words available
            Assert.AreEqual(@"<h1>one two three </h1><a href=""testurl""><b>four five </b></a><i><strong>some italic nested in string</strong></i> ...",
            Texts.TruncateHTMLSafeishCharXML(
                @"<h1>one two three </h1><a href=""testurl""><b>four five </b></a><i><strong>some italic nested in string</strong></i>",
                99));
        }
    }
}

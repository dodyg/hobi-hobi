using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using HobiHobi.Core.Syndications;
using System.Globalization;
using NodaTime;
using HobiHobi.Core.Utils;
using System.Text.RegularExpressions;

namespace HobiHobi.Tests.Core.Syndications
{
    [TestFixture]
    public class DateParsingTest
    {
        [Test]
        public void TestUniversalFormat()
        {
            var dateString = "Fri Nov 30 16:30:00 EST 2012";
            var pattern = "ddd MMM dd HH:mm:ss EST yyyy";

            var time = DateTimeParser.ConvertWithTimezone(dateString);
            
            Assert.IsTrue(time.IsTrue, "Exception " + time.Message);
            Assert.IsTrue(false, "Date time " + time.Value.ToString());
        }

        [Test]
        public void GenerateTimeZoneString()
        {
            var regexMatching = DateTimeParser.GenerateRegexMatchingString();
            Assert.IsTrue(regexMatching.Length > 0, "There must be some regex");
        }

        [Test]
        public void TestMatches()
        {
            var dateString = "Fri Nov 30 16:30:00 EST 2012";
            var pattern = DateTimeParser.GenerateRegexMatchingString();
            var reg = new Regex(pattern);

            var res = reg.Match(dateString);
            Assert.IsTrue(res.Success, "Match must happens");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using HobiHobi.Core.Utils;

namespace HobiHobi.Tests.Core.Utils
{
    [TestFixture]
    public class UrlShortenerTests
    {
        [Test, Explicit]
        public void TestGoogle()
        {
            var url = "http://hobieu.apphb.com";
            var res = UrlShortener.GetGoogle(url);

            Assert.IsTrue(res.IsTrue, "Shorten operation must be true");
            Assert.IsTrue(!string.IsNullOrWhiteSpace(res.Value), "Shortened url payload must exists");
        }
    }
}

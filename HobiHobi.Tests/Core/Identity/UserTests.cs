using NUnit.Framework;

namespace HobiHobi.Tests.Core.Identity
{
    [TestFixture]
    public class UserTests
    {
        [Test]
        public void TestPasswordComparision()
        {
            var password = "Hello World";
            var hash = HobiHobi.Core.Identity.User.HashPassword(password);
            var verification = HobiHobi.Core.Identity.User.VerifyPassword(password, hash);
            Assert.IsTrue(verification);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using HobiHobi.Core;

namespace HobiHobi.Tests
{
    [TestFixture]
    public class SymmCryptoTests
    {
        [Test]
        public void TestGenerateSalt()
        {
            var salt = SymmCrypto.GenerateSalt();
            Assert.IsNotEmpty(salt);
        }

        [Test]
        public void TestEncrypDecrypt()
        {
            var salt = SymmCrypto.GenerateSalt();

            string password = "what can be done here";
            var sym = new SymmCrypto(password, salt);
            var plainText = "Good Morning Cairo";
            var crypto = sym.GetEncryptedValue(plainText);
            var decrypt = sym.GetDecryptedValue(crypto);

            Assert.IsTrue(plainText == decrypt);
        }
    }
}

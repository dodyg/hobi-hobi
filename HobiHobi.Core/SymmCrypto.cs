using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Configuration;

namespace HobiHobi.Core
{
    //License/Copyright for this code belongs to its creator
    //http://blogs.msdn.com/b/nikhiln/archive/2008/05/18/net-2-0-symmetric-encryption-code-sample.aspx
    public class SymmCrypto
    {
        private readonly string IV = string.Empty;
        private readonly string Key = string.Empty;
        /// <summary>
        /// Initializes a new instance of the <see cref="CryptoHelper"/> class.
        /// </summary>
        public SymmCrypto(string password, string salt)
        {
            var pdb = new Rfc2898DeriveBytes(password, Convert.FromBase64String(salt));

            IV = Convert.ToBase64String(pdb.GetBytes(8));
            Key = Convert.ToBase64String(pdb.GetBytes(16));
        }

        /// <summary>
        /// Gets the encrypted value.
        /// </summary>
        /// <param name="inputValue">The input value.</param>
        /// <returns></returns>
        public string GetEncryptedValue(string inputValue)
        {
            TripleDESCryptoServiceProvider provider = this.GetCryptoProvider();
            // Create a MemoryStream.
            MemoryStream mStream = new MemoryStream();

            // Create a CryptoStream using the MemoryStream 
            // and the passed key and initialization vector (IV).
            CryptoStream cStream = new CryptoStream(mStream,
                provider.CreateEncryptor(), CryptoStreamMode.Write);

            byte[] toEncrypt = new UTF8Encoding().GetBytes(inputValue);

            // Write the byte array to the crypto stream and flush it.
            cStream.Write(toEncrypt, 0, toEncrypt.Length);
            cStream.FlushFinalBlock();

            // Get an array of bytes from the 
            // MemoryStream that holds the 
            // encrypted data.
            byte[] ret = mStream.ToArray();

            // Close the streams.
            cStream.Close();
            mStream.Close();

            // Return the encrypted buffer.
            return Convert.ToBase64String(ret);
        }

        /// <summary>
        /// Gets the crypto provider.
        /// </summary>
        /// <returns></returns>
        private TripleDESCryptoServiceProvider GetCryptoProvider()
        {
            TripleDESCryptoServiceProvider provider = new TripleDESCryptoServiceProvider();
            provider.IV = Convert.FromBase64String(IV);
            provider.Key = Convert.FromBase64String(Key);
            return provider;
        }

        /// <summary>
        /// Gets the decrypted value.
        /// </summary>
        /// <param name="inputValue">The input value.</param>
        /// <returns></returns>
        public string GetDecryptedValue(string inputValue)
        {
            TripleDESCryptoServiceProvider provider = this.GetCryptoProvider();
            byte[] inputEquivalent = Convert.FromBase64String(inputValue);
            // Create a new MemoryStream.
            MemoryStream msDecrypt = new MemoryStream();

            // Create a CryptoStream using the MemoryStream 
            // and the passed key and initialization vector (IV).
            CryptoStream csDecrypt = new CryptoStream(msDecrypt,
                provider.CreateDecryptor(),
                CryptoStreamMode.Write);
            csDecrypt.Write(inputEquivalent, 0, inputEquivalent.Length);
            csDecrypt.FlushFinalBlock();

            csDecrypt.Close();

            //Convert the buffer into a string and return it.
            return new UTF8Encoding().GetString(msDecrypt.ToArray());
        }

        public static string GenerateSalt()
        {
            var rng = new RNGCryptoServiceProvider();
            var key = new byte[24];
            rng.GetBytes(key);

            for (var i = 0; i < key.Length; ++i)
            {
                int keyByte = key[i] & 0xFE;
                var parity = 0;
                for (var b = keyByte; b != 0; b >>= 1) parity ^= b & 1;
                key[i] = (byte)(keyByte | (parity == 0 ? 1 : 0));
            }

            return Convert.ToBase64String(key);
        }

        public static SymmCrypto CreateFromConfig()
        {
            return new SymmCrypto(GetPasswordFromConfig(), GetPasswordFromConfig());
        }

        /// <summary>
        /// Get value of TripleDES_Salt from web.config
        /// </summary>
        /// <returns></returns>
        public static string GetSaltFromConfig()
        {
            return System.Configuration.ConfigurationManager.AppSettings["TripleDES_Salt"];
        }

        /// <summary>
        /// Get value of TripleDES_Password from web.config
        /// </summary>
        /// <returns></returns>
        public static string GetPasswordFromConfig()
        {
            return System.Configuration.ConfigurationManager.AppSettings["TripleDES_Password"];
        }
    }
}
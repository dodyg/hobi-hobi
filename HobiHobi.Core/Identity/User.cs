using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using HobiHobi.Core.Framework;
using System.Web;

namespace HobiHobi.Core.Identity
{
    public class User
    {
        public static Key NewKey(string value = null)
        {
            return Key.Generate("User/", value);
        }

        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public AccountStatus Status { get; set; }
        public AccountLevel Level { get; set; }
        public DateTime DateCreated { get; set; }

        public static string HashPassword(string password)
        {
            var hash = BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt(15));
            return hash;
        }

        public static bool VerifyPassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);

        }

        public const string USER_COOKIE_NAME = "UserInformation";

        /// <summary>
        /// Write basic information to cookie
        /// </summary>
        /// <param name="usr"></param>
        /// <returns></returns>
        public static HttpCookie WriteCookie(User usr)
        {
            var crypto = SymmCrypto.CreateFromConfig();
            var encrypted = crypto.GetEncryptedValue(usr.Id + "|" + usr.Level.ToString());
     
            var ck = new HttpCookie(USER_COOKIE_NAME);
            ck.Values.Add("UserId", encrypted);
            ck.Values.Add("FirstName", usr.FirstName);
            ck.Values.Add("LastName", usr.LastName);
            ck.Values.Add("Email", usr.Email);
            ck.Expires = DateTime.Now.AddDays(1);
            
            return ck;
        }

        /// <summary>
        /// Get user info from stored cookie
        /// </summary>
        /// <param name="cookie"></param>
        /// <returns></returns>
        public static IQuerySetOne<UserInfo> GetFromCookie(HttpCookie cookie)
        {
            if (cookie == null)
                new QuerySetOne<UserInfo>(null);

            try
            {
                var crypto = SymmCrypto.CreateFromConfig();
                var decrypted = crypto.GetDecryptedValue(cookie.Values["UserId"]);
                var idSplits = decrypted.Split(new char[] { '|' });
                
                var info = new UserInfo
                {
                    Id = idSplits[0],
                    FirstName = cookie.Values["FirstName"],
                    LastName = cookie.Values["LastName"],
                    Email = cookie.Values["Email"],
                    Level = (AccountLevel)Enum.Parse(typeof(AccountLevel), idSplits[1])
                };

                return new QuerySetOne<UserInfo>(info);
            }
            catch (Exception)
            {
                return new QuerySetOne<UserInfo>(null);
            }
        }

        /// <summary>
        /// Expire the user info cookie
        /// </summary>
        /// <returns></returns>
        public static HttpCookie ExpireCookie()
        {
            var ck = new HttpCookie(USER_COOKIE_NAME);
            ck.Expires = DateTime.Now.AddDays(-1);
            return ck;
        }
    }
}

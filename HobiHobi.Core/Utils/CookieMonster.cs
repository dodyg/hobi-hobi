using HobiHobi.Core.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace HobiHobi.Core.Utils
{
    public static class CookieMonster
    {
        static DateTime GetExpirationDate()
        {
            return DateTime.Now.AddDays(30);
        }

        static DateTime GetExpiredDate()
        {
            return DateTime.Now.AddDays(-1);
        }

        public static IQuerySetOne<T> GetFromCookie<T>(HttpCookie cookie) where T : class
        {
            if (cookie == null)
                return new QuerySetOne<T>(null);
            else
            {
                var account = JsonConvert.DeserializeObject<T>(cookie.Value);
                return new QuerySetOne<T>(account);
            }
        }

        public static HttpCookie SetCookie<T>(T data, string cookieName) where T : class
        {
            var cookie = new HttpCookie(cookieName);
            cookie.Value = JsonConvert.SerializeObject(data);
            cookie.Expires = GetExpirationDate();
            return cookie;
        }

        public static HttpCookie ExpireCookie(string cookieName)
        {
            var cookie = new HttpCookie(cookieName);
            cookie.Value = "";
            cookie.Expires = GetExpiredDate();
            return cookie;
        }
    }
}

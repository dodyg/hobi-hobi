using HobiHobi.Core.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace HobiHobi.Core.Identity
{
    [Serializable]
    public class UserInfo
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public AccountLevel Level { get; set; }

        public static IQuerySetOne<UserInfo> GetFromContext(HttpContext context)
        {
            var item = context.Items["UserInfo"] as UserInfo;

            if (item == null)
                return new QuerySetOne<UserInfo>(null);
            else
                return new QuerySetOne<UserInfo>(item);
        }
    }
}

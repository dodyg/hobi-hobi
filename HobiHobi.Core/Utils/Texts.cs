using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HobiHobi.Core.Utils
{
    public static class Texts
    {
        public static string LimitTextForRiverJs(string str)
        {
            if (str == null)
                return str;

            if (str.Length <= 280)
                return str;
            else
            {
                str = str.Substring(0, 279); //take only 280 according to riverjs.org
                var lastEmpty = str.LastIndexOf(' ');
                if (lastEmpty != 279)
                    str = str.Substring(0, lastEmpty);

                str += "...";

                return str;
            }
        }

        public static string FromUriHost(Uri uri)
        {
            if (uri.IsDefaultPort)
                return uri.Scheme + "://" + uri.DnsSafeHost;
            else
                return uri.Scheme + "://" + uri.DnsSafeHost + ":" + uri.Port;
        }

        public static string ConvertTitleToName(string title)
        {
            return title.Replace(" ", "_").Replace("#", "_").Replace("'", "_").Replace(".", "_")
                .Replace("/", "_").Replace("\\", "_");
        }
    }
}

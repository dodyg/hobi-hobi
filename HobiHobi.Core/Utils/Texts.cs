using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.XPath;

namespace HobiHobi.Core.Utils
{
    public static class Texts
    {
        const int CUT_POINT = 276;

        public static string LimitTextForRiverJs(string str)
        {
            if (str == null)
                return str;

            if (str.Length <= 280)
                return str;
            else
            {
                str = StripTagsRegex(str); //remove any html tags

                if (str.Length > CUT_POINT)
                {
                    str = str.Substring(0, CUT_POINT); //take only 280 according to riverjs.org

                    var lastEmpty = str.LastIndexOf(' '); //take care we do not truncate words 
                    if (lastEmpty != CUT_POINT)
                        str = str.Substring(0, lastEmpty);

                    str += "...";
                }

                return str;
            }
        }

        static Regex _htmlRegex = new Regex("<.*?>", RegexOptions.Compiled);

        public static string StripTagsRegex(string source)
        {
            return _htmlRegex.Replace(source, string.Empty);
        }

        public static string FromUriHost(Uri uri)
        {
#if DEBUG
            if (uri.IsDefaultPort)
                return uri.Scheme + "://" + uri.DnsSafeHost;
            else
                return uri.Scheme + "://" + uri.DnsSafeHost + ":" + uri.Port;
#else
            return uri.Scheme + "://" + uri.DnsSafeHost;
#endif
        }

        public static string ConvertTitleToName(string title)
        {
            return title.Replace(" ", "-").Replace("#", "-").Replace("'", "-").Replace(".", "-")
                .Replace("/", "-").Replace("\\", "-");
        }

        public static string ConvertTitleToUrl(string title)
        {
            return ConvertTitleToName(title);
        }
    }
}

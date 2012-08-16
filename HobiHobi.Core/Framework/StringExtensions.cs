using System;
using System.Text.RegularExpressions;
using System.Web;

namespace HobiHobi.Core.Framework
{
    /// <summary>
    /// 
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Convert the normal string to HtmlString for output
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static IHtmlString ToHtmlString(this string self)
        {
            return new HtmlString(self);
        }


        public static string @F(this string self, params object[] parameters)
        {
            return String.Format(self, parameters);
        }

        /// <summary>
        /// Indicate whether the string is empty or null or whitespace
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static bool IsNullOrWhiteSpace(this string self)
        {
            if (self == null)
                return true;
            else
                return String.IsNullOrEmpty(self.Trim());
        }

        /// <summary>
        /// The string is not null, empty or whitespace
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static bool Exists(this string self)
        {
            return !self.IsNullOrWhiteSpace();
        }

        /// <summary>
        /// Split ThisIsAnEnumeratorUsually into "This Is An Enumerator Usually". This is useful to present enumerator as drop down list as is.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static string SplitCamelCaseToWords(this string self)
        {
            return Regex.Replace(self, "([a-z](?=[A-Z])|[A-Z](?=[A-Z][a-z]))", "$1 ");
        }

        public static string Truncate(this string source, int length = 33)
        {
            if (string.IsNullOrWhiteSpace(source))
                return source;

            if (source.Length > length)
            {
                source = source.Substring(0, length);
                source += "...";
            }

            return source;
        }

        public static string ForceEmptyStringWhenNull(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return "";
            else
                return str;
        }
    }
}

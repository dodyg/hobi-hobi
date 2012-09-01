using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace HobiHobi.Core.Framework
{
    public static class HtmlHelperExtensions
    {
        /// Generate a javascript link tag with base reference to /scripts/
        /// </summary>
        /// <param name="html"></param>
        /// <param name="jsFile">the javascript file without .js extensions</param>
        /// <returns></returns>
        public static HtmlString JavascriptLink(this HtmlHelper html, params string[] jsFile)
        {
            var str = new StringBuilder();

            foreach (var fl in jsFile)
                str.AppendLine(@"<script type=""text/javascript"" src=""{0}""></script>".F(fl + ".js"));

            return new HtmlString(str.ToString());
        }
    }
}

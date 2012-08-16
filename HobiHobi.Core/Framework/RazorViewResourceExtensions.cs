using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Compilation;
using System.Globalization;
using System.Threading;

namespace HobiHobi.Core.Framework
{
    public static class RazorViewResourceExtensions
    {
        public static string Local(this WebViewPage<dynamic> page, string key)
        {
            return page.ViewContext.HttpContext.GetLocalResourceObject(page.VirtualPath, key) as string;
        }

        public static string Global(this WebViewPage<dynamic> page, string classKey, string key)
        {
            var log = NLog.LogManager.GetLogger("Global");
            log.Debug("Global UI culture " + Thread.CurrentThread.CurrentUICulture.DisplayName);
            return page.ViewContext.HttpContext.GetGlobalResourceObject(classKey, key) as string;
        }

        public static string Local(this WebViewPage page, string key)
        {
            return page.ViewContext.HttpContext.GetLocalResourceObject(page.VirtualPath, key) as string;
        }

        public static string Global(this WebViewPage page, string classKey, string key)
        {
            var log = NLog.LogManager.GetLogger("Global");
            log.Debug("Global UI culture " + Thread.CurrentThread.CurrentUICulture.DisplayName);
            return page.ViewContext.HttpContext.GetGlobalResourceObject(classKey, key) as string;
        }
    }
}

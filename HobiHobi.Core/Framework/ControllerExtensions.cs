using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.IO.Compression;
using System.Web;
using HobiHobi.Core.Identity;

namespace HobiHobi.Core.Framework
{
    public static class ControllerExtensions
    {
        public static void Compress(this Controller self)
        {
            string acceptEncoding = self.Request.Headers["Accept-Encoding"];
            if (string.IsNullOrWhiteSpace(acceptEncoding))
                return;
            else if (acceptEncoding.ToLower().Contains("gzip"))
            {
                self.Response.Filter = new GZipStream(self.Response.Filter, CompressionMode.Compress);
                self.Response.AppendHeader("Content-Encoding", "gzip");
            }
            else if (acceptEncoding.ToLower().Contains("deflate"))
            {
                self.Response.Filter = new DeflateStream(self.Response.Filter, CompressionMode.Compress);
                self.Response.AppendHeader("Content-Encoding", "deflate");
            }
        }

        public static IQuerySetOne<UserInfo> GetCurrentUser(this Controller self)
        {
            return UserInfo.GetFromContext(HttpContext.Current);
        }
    }
}

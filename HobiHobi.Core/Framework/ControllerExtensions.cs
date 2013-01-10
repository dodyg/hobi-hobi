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
        public static ModelPropertyErrors ProduceAJAXErrorMessage(this Controller self, ModelStateDictionary ms)
        {
            var errors = new ModelPropertyErrors();

            foreach (var key in ms.Keys)
            {
                ModelState modelState = ms[key];
                if (modelState.Errors.Any())
                {
                    var r = new ModelPropertyError { Key = key };
                    foreach (ModelError error in modelState.Errors)
                    {
                        r.Errors.Add(error.ErrorMessage);
                    }
                    errors.Properties.Add(r);
                }
            }

            return errors;
        }

        public static bool IfSecureConnectionOnAppHarbor(this Controller self)
        {
            var secure = self.Request.Headers["X-Forwarded-Proto"] == "https"; //specific hack for appharbor to detect whether request is secure
            return secure;
        }

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

        public static string GetEtag(this Controller self)
        {
            return self.Request.Headers["If-None-Match"];
        }

        public static string FormatEtag(this Controller self, string etag)
        {
            return "\"" + etag + "\"";
        }

        public static void SetLongExpirationCache(this Controller self)
        {
                self.Response.Cache.SetCacheability(HttpCacheability.Public);
                self.Response.Cache.SetExpires(DateTime.Now.AddDays(60));
        }

        public static void CompressAndSetLongExpirationCache(this Controller self)
        {
            Compress(self);
            SetLongExpirationCache(self);
        }
    }
}

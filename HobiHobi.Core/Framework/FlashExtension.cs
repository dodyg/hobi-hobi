using System;
using System.Web;
using System.Web.Mvc;

namespace HobiHobi.Core.Framework
{
    public static class FlashExtension
    {
        /// <summary>
        /// Enable flash message in three different types.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="type"></param>
        /// <param name="message"></param>
        /// <param name="values"></param>
        public static void Flash(this Controller self, FlashType type, string message, params object[] values)
        {
            Flash(self, type, "", message, values);
        }

        /// <summary>
        /// Enable flash message in three different types and specific key channel on which it will be displayed
        /// </summary>
        /// <param name="self"></param>
        /// <param name="type"></param>
        /// <param name="key"></param>
        /// <param name="message"></param>
        /// <param name="values"></param>
        public static void Flash(this Controller self, FlashType type, string key, string message, params object[] values)
        {
            self.TempData["FlashMessage_" + key] = message.F(values);
            self.TempData["FlashMessageType_" + key] = type.ToString();
        }


        /// <summary>
        /// Enable notice flash message
        /// </summary>
        /// <param name="self"></param>
        /// <param name="message"></param>
        /// <param name="values"></param>
        public static void FlashNotice(this Controller self, string message, params object[] values)
        {
            Flash(self, FlashType.Notice, message, values);
        }

        /// <summary>
        /// Enable success flash message
        /// </summary>
        /// <param name="self"></param>
        /// <param name="message"></param>
        /// <param name="values"></param>
        public static void FlashSuccess(this Controller self, string message, params object[] values)
        {
            Flash(self, FlashType.Success, message, values);
        }

        /// <summary>
        /// Enable error flash message
        /// </summary>
        /// <param name="self"></param>
        /// <param name="message"></param>
        /// <param name="values"></param>
        public static void FlashError(this Controller self, string message, params object[] values)
        {
            Flash(self, FlashType.Error, message, values);
        }

        /// <summary>
        /// Enable info flash message
        /// </summary>
        /// <param name="self"></param>
        /// <param name="message"></param>
        /// <param name="values"></param>
        public static void FlashInfo(this Controller self, string message, params object[] values)
        {
            Flash(self, FlashType.Info, message, values);
        }

        /// <summary>
        /// Pass a certain key to the view to signify certain message intention. It's up to the view to decide what string to display
        /// </summary>
        /// <param name="self"></param>
        /// <param name="key"></param>
        /// <param name="type"></param>
        public static void FlashKey(this Controller self, string key, FlashType type = FlashType.Notice)
        {
            Flash(self, type, key, "");
        }

        static string GetCssClassFor(FlashType type)
        {
            switch (type)
            {
                case FlashType.Error: return "alert alert-error";
                case FlashType.Notice: return "alert";
                case FlashType.Info: return "alert alert-info";
                case FlashType.Success: return "alert alert-success";
                default: return "flashInfo";
            }
        }

        /// <summary>
        /// Show the flash message in general channel or specific channel. The output uses .flashError, .flashNotice and .flashSuccess css class.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static IHtmlString Flash(this WebViewPage self, string key = null)
        {
            var tmpData = self.TempData["FlashMessage_" + key];
            var tmpType = self.TempData["FlashMessageType_" + key];

            if (tmpData != null)
            {
                FlashType type;

                if (Enum.TryParse(tmpType + "", out type))
                {
                    string cssClass = GetCssClassFor(type);
                    return @"<div class=""{0}""><a class=""close"" data-dismiss=""alert"" href=""#"">x</a>{1}</div>".F(cssClass, tmpData).ToHtmlString();
                }
                else
                    return @"<div class=""flashInfo"">{0}</div>".F(tmpData).ToHtmlString();
            }
            else
                return "".ToHtmlString();
        }

        /// <summary>
        /// Interrogate whether a certain flash type exists
        /// </summary>
        /// <param name="self"></param>
        /// <param name="type"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool IfFlash(this WebViewPage self, FlashType type, string key = null)
        {
            var tmpData = self.TempData["FlashMessage_" + key];
            var tmpType = self.TempData["FlashMessageType_" + key];

            if (tmpData != null)
            {
                FlashType tp;

                if (Enum.TryParse(tmpType + "", out tp))
                {
                    return tp == type;
                }
                else
                    return false;
            }
            else
                return false;
        }

        /// <summary>
        /// Show the flash message in general channel or specific channel. The output uses .flashError, .flashNotice and .flashSuccess css class.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static IHtmlString Flash(this ViewUserControl self, string key = null)
        {
            var tmpData = self.TempData["FlashMessage_" + key];
            var tmpType = self.TempData["FlashMessageType_" + key];

            if (tmpData != null)
            {
                FlashType type;

                if (Enum.TryParse(tmpType + "", out type))
                {
                    string cssClass = GetCssClassFor(type);
                    return @"<div class=""{0}"">{1}</div>".F(cssClass, tmpData).ToHtmlString();
                }
                else
                    return @"<div class=""flashInfo"">{0}</div>".F(tmpData).ToHtmlString();
            }
            else
                return "".ToHtmlString();
        }

        public static IHtmlString OnlyIf(this ViewUserControl self, string key, string message)
        {
            if (self.Request[key] != null)
                return message.ToHtmlString();
            else
                return "".ToHtmlString();
        }

        /// <summary>
        /// Show the flash message in general channel or specific channel. The output uses .flashError, .flashNotice and .flashSuccess css class. If key is found, show message.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="key"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static IHtmlString Flash(this WebViewPage self, string key, string message, FlashType type)
        {
            var tmpData = self.TempData["FlashMessage_" + key];//ignore 
            var flush = self.TempData["FlashMessageType_" + key]; //ignore

            if (tmpData != null)
            {
                string cssClass = GetCssClassFor(type);
                return @"<div class=""{0}"">{1}</div>".F(cssClass, message).ToHtmlString();
            }
            else
                return "".ToHtmlString();
        }

        /// <summary>
        /// Show the flass message in the general or specific channel. The output is depending on how the FlashKey format is set up (.flashError, flasjNotice and .flashSuccess)
        /// </summary>
        /// <param name="self"></param>
        /// <param name="key"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static IHtmlString Flash(this WebViewPage self, string key, string message)
        {
            var tmpData = self.TempData["FlashMessage_" + key];//ignore 
            var tmpType = self.TempData["FlashMessageType_" + key]; //ignore

            if (tmpData != null)
            {
                FlashType type;

                if (Enum.TryParse(tmpType + "", out type))
                {
                    string cssClass = GetCssClassFor(type);
                    return @"<div class=""{0}"">{1}</div>".F(cssClass, message).ToHtmlString();
                }
            }

            return "".ToHtmlString();
        }


        /// <summary>
        /// Show the flash message in general channel or specific channel. The output uses .flashError, .flashNotice and .flashSuccess css class. If key is found, show message.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="key"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static IHtmlString Flash(this ViewUserControl self, string key, string message, FlashType type)
        {
            var tmpData = self.TempData["FlashMessage_" + key];//ignore 
            var flush = self.TempData["FlashMessageType_" + key]; //ignore

            if (tmpData != null)
            {
                string cssClass = GetCssClassFor(type);
                return @"<div class=""{0}"">{1}</div>".F(cssClass, message).ToHtmlString();
            }
            else
                return "".ToHtmlString();
        }

        public static IHtmlString FlashFormat(this WebViewPage self, string message, FlashType type)
        {
            string cssClass = GetCssClassFor(type);
            return @"<div class=""{0}"">{1}</div>".F(cssClass, message).ToHtmlString();
        }

        /// <summary>
        /// Format message based on a condition. Otherwise do not show.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="condition"></param>
        /// <param name="message"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IHtmlString FlashFormat(this WebViewPage self, bool condition, string message, FlashType type)
        {
            if (condition)
            {
                string cssClass = GetCssClassFor(type);
                return @"<div class=""{0}"">{1}</div>".F(cssClass, message).ToHtmlString();
            }
            else
                return "".ToHtmlString();
        }

        public static IHtmlString FlashFormat(this ViewUserControl self, bool condition, string message, FlashType type)
        {
            if (condition)
            {
                string cssClass = GetCssClassFor(type);
                return @"<div class=""{0}"">{1}</div>".F(cssClass, message).ToHtmlString();
            }
            else
                return "".ToHtmlString();
        }
    }
}

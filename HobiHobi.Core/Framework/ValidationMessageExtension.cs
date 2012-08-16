using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace HobiHobi.Core.Framework
{
    public static class ValidationMessageExtension
    {
        /// <summary>
        /// Make it easier to write general validation message (non property) that appears on Html.ValidationSummary() 
        /// </summary>
        /// <param name="self"></param>
        /// <param name="message"></param>
        /// <param name="values"></param>
        public static void ValidationMessage(this Controller self, string message, params object[] values)
        {
            self.ModelState.AddModelError(string.Empty, message.F(values));
        }

        /// <summary>
        /// Make it easier to write property based validation message that appears on Html.ValidationSummary()
        /// </summary>
        /// <param name="self"></param>
        /// <param name="property"></param>
        /// <param name="message"></param>
        /// <param name="values"></param>
        public static void PropertyValidationMessage(this Controller self, string property, string message, params object[] values)
        {
            self.ModelState.AddModelError(property, message.F(values));
        }
    }
}

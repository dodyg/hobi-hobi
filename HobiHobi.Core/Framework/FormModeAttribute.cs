using System;
using System.Web.Mvc;

namespace HobiHobi.Core.Framework
{
    [AttributeUsage(AttributeTargets.Method)]
    public class FormModeAttribute : ActionFilterAttribute
    {
        FormMode _method;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:FormModeAttribute"/> class.
        /// </summary>
        public FormModeAttribute(FormMode method)
        {
            _method = method;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.ViewBag.FormMode = _method;
        }
    }
}
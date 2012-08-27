using System.Web.Mvc;

namespace HobiHobi.Web.Areas.Manage
{
    public class ManageAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Manage";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Manage_default",
                "Manage/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional, controller = "Home" },
                new string[] { "HobiHobi.Web.Areas.Manage.Controllers"}
            );
        }
    }
}

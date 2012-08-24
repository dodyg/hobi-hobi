using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Raven.Client;

namespace HobiHobi.Web.Controllers
{
    public class RavenController : Controller
    {
        public IDocumentSession RavenSession { get; protected set; }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
#if DEBUG
            RavenSession = MvcApplication.Store.OpenSession(MvcApplication.DATABASE_NAME);
#else
            RavenSession = MvcApplication.Store.OpenSession();
#endif
        }

        protected void SaveChangesAndTerminate()
        {
            using (RavenSession)
            {
                if (RavenSession != null)
                    RavenSession.SaveChanges();

                RavenSession = null;
            }
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.IsChildAction)
                return;

            using (RavenSession)
            {
                if (filterContext.Exception != null)
                    return;

                if (RavenSession != null)
                    RavenSession.SaveChanges();
            }
        }
    }
}

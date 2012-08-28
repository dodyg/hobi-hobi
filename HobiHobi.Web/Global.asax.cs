using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using FluentValidation;
using FluentValidation.Mvc;
using HobiHobi.Web.IoC;
using Raven.Client.Document;
using Raven.Client.Extensions;
using Raven.Client.Indexes;
using System.Web.Security;
using System.Security.Principal;
using Newtonsoft.Json;
using HobiHobi.Core.Identity;
namespace HobiHobi.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static DocumentStore Store;
        public static string CommonJsTag;
        public static string CommonCssTag;

#if DEBUG
        public const string DATABASE_NAME = "hobihobi";
#endif

        /// <summary>
        /// We are using this because we are not using any stupid membership provider
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            HttpCookie authCookie = Context.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                var authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                var usr = JsonConvert.DeserializeObject<UserInfo>(authTicket.UserData);

                string[] roles = new string[] { usr.Level.ToString() };

                var userPrincipal = new GenericPrincipal(new GenericIdentity(authTicket.Name), roles);
                Context.User = userPrincipal;

                Context.Items["UserInfo"] = usr;

            }
        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }, // Parameter defaults
                new string[] { "HobiHobi.Web.Controllers"}
            );

        }

        void InitializeRavenDB()
        {

            Store = new DocumentStore { ConnectionStringName = "RavenDB" };
            Store.Initialize();

            IndexCreation.CreateIndexes(Assembly.GetCallingAssembly(), Store);

#if DEBUG
            Store.DatabaseCommands.EnsureDatabaseExists(DATABASE_NAME);
#endif
        }

        protected void Application_Start()
        {
            InitializeRavenDB();
            CommonCssTag = System.Configuration.ConfigurationManager.AppSettings["Site.CssTag"];
            CommonJsTag = System.Configuration.ConfigurationManager.AppSettings["Site.JsTag"];

            //wire up all the necessary objects used in this web application
            ContainerBuilder builder = BootStrap.RegisterAll();
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));


            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            DataAnnotationsModelValidatorProvider.AddImplicitRequiredAttributeForValueTypes = false;
            FluentValidationModelValidatorProvider.Configure(provider =>
            {
                provider.ValidatorFactory = container.Resolve<IValidatorFactory>();
            });
        }
    }
}
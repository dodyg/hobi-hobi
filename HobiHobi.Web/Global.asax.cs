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
namespace HobiHobi.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static DocumentStore Store;

#if DEBUG
        public const string DATABASE_NAME = "hobihobi";
#endif

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
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
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

            //wire up all the necessary objects used in this web application
            ContainerBuilder builder = BootStrap.RegisterAll();
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));


            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            FluentValidationModelValidatorProvider.Configure(provider =>
            {
                provider.ValidatorFactory = container.Resolve<IValidatorFactory>();
            });
        }
    }
}
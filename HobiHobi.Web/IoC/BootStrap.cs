using System.Reflection;
using Autofac;
using Autofac.Integration.Mvc;
using FluentValidation;
using HobiHobi.Core.Framework;

namespace HobiHobi.Web.IoC
{
    public class BootStrap
    {
        public static ContainerBuilder RegisterAll()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<LogInjectionModule>();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<ValidatorFactory>().As<IValidatorFactory>();
            HobiHobi.Core.BootStrap.Register(builder, BootStrapType.Web);

            return builder;
        }
    }
}
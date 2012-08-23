using System;
using HobiHobi.Core.Framework;
using NLog;

namespace HobiHobi.Web.IoC
{
    /// <summary>
    /// Inject the instance of NLog object to anyone implementing ILogParticipant. It's available in Log property.
    /// </summary>
    public class LogInjectionModule : Autofac.Module
    {
        protected override void AttachToComponentRegistration(Autofac.Core.IComponentRegistry componentRegistry, Autofac.Core.IComponentRegistration registration)
        {
            base.AttachToComponentRegistration(componentRegistry, registration);
            registration.Activated += new EventHandler<Autofac.Core.ActivatedEventArgs<object>>(registration_Activated);
        }

        void registration_Activated(object sender, Autofac.Core.ActivatedEventArgs<object> e)
        {
            var log = e.Instance as ILogParticipant;
            if (log != null)
                log.Log = LogManager.GetLogger(e.Instance.GetType().Name);
        }
    }
}
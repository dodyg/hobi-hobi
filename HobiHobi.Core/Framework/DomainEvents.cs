using System.Web.Mvc;

namespace HobiHobi.Core.Framework
{
    /// <summary>
    /// Ref http://www.udidahan.com/2009/06/14/domain-events-salvation/
    /// </summary>
    public static class DomainEvents
    {
        //Raises the given domain event
        public static void Raise<T>(T args) where T : IDomainEvent
        {
            foreach (var handler in DependencyResolver.Current.GetServices<Handles<T>>())
                handler.Handle(args);
        }
    }
}

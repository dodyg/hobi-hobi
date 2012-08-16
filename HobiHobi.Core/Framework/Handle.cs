
namespace HobiHobi.Core.Framework
{
    public interface Handles<T> where T : IDomainEvent
    {
        void Handle(T args);
    }
}

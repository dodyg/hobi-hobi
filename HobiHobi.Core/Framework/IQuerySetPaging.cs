
namespace HobiHobi.Core.Framework
{
    /// <summary>
    /// Provide a read only interface for many items query result + paging information
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IQuerySetPaging<T> : IQuerySetMany<T>
    {
        PagingInfo PagingInfo { get; }
    }
}

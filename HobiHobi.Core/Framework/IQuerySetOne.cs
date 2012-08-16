using System;

namespace HobiHobi.Core.Framework
{
    public interface IQuerySetOne<out T>
    {
        bool IsFound { get; }
        T Item { get; }
        Exception Exception { get; }
    }
}

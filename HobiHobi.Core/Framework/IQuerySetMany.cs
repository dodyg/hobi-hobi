using System;
using System.Collections.Generic;

namespace HobiHobi.Core.Framework
{
    public interface IQuerySetMany<T>
    {
        int Count { get; }
        bool IsFound { get; }
        IEnumerable<T> Items { get; }
        Exception Exception { get; }
    }
}

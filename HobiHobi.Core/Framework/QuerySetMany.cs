using System.Collections.Generic;

namespace HobiHobi.Core.Framework
{
    /// <summary>
    /// A query set expecting a list, array, or any IEnumerable<>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class QuerySetMany<T> : QuerySet<T>, IQuerySetMany<T> where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:QueryMulti"/> class.
        /// </summary>
        public QuerySetMany()
            : base()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:QueryMulti"/> class.
        /// </summary>
        public QuerySetMany(IEnumerable<T> items)
            : base(multiple: items)
        {

        }

        IEnumerable<T> IQuerySetMany<T>.Items
        {
            get { return this.Multiple; }
        }
    }
}


namespace HobiHobi.Core.Framework
{
    /// <summary>
    /// A query set expecting a single item
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class QuerySetOne<T> : QuerySet<T>, IQuerySetOne<T> where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:QuerySingle"/> class.
        /// </summary>
        public QuerySetOne()
            : base()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:QuerySingle"/> class.
        /// </summary>
        public QuerySetOne(T item)
            : base(single: item)
        {

        }

        T IQuerySetOne<T>.Item
        {
            get { return this.Single; }
        }
    }
}

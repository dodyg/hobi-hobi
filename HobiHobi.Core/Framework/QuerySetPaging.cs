using System.Collections.Generic;

namespace HobiHobi.Core.Framework
{
    /// <summary>
    /// A query set expecting a list, array or any IEnumerable and also paging information
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class QuerySetPaging<T> : QuerySetMany<T>, IQuerySetPaging<T> where T : class
    {
        /// <summary>
        /// Gets or sets the paging informatino.
        /// </summary>
        /// <value>The paging informatino.</value>
        public PagingInfo PagingInfo
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:QuerySetPaging"/> class.
        /// </summary>
        public QuerySetPaging()
            : base()
        {

        }

        public QuerySetPaging(IEnumerable<T> items, int currentPage, int pageSize, int totalResults)
            : base(items)
        {
            PagingInfo = new PagingInfo
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalResults = totalResults
            };
        }
    }
}

using System;

namespace HobiHobi.Core.Framework
{
    public class PagingInfo
    {
        /// <summary>
        /// Gets or sets the current page.
        /// </summary>
        /// <value>The current page.</value>
        public int CurrentPage
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the summary.
        /// </summary>
        /// <value>The summary.</value>
        public int PageSize
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the total results.
        /// </summary>
        /// <value>The total results.</value>
        public int TotalResults
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the total number of pages
        /// </summary>
        public int TotalPages
        {
            get
            {
                var remainder = 0;
                var totalPages = Math.DivRem(TotalResults, PageSize, out remainder);
                if (remainder > 0)
                    totalPages++;
                return totalPages;
            }
        }
    }
}

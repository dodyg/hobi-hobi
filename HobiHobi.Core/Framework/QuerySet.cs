using System;
using System.Collections.Generic;
using System.Linq;

namespace HobiHobi.Core.Framework
{
    /// <summary>
    /// This is a class to hold query results with several nice properties
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class QuerySet<T> where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:QuerySet"/> class.
        /// By ignoring all the optional parameters, the query set is in status of NotFound
        /// </summary>
        /// <param name="single">Set a value here if a single value is returned</param>
        /// <param name="multiple">Set a value here if a list of value is returned</param>
        public QuerySet(T single = null, IEnumerable<T> multiple = null)
        {
            if (single == null && multiple == null)
                NotFound();
            else if (single != null)
                Found(single);
            else
                Found(multiple);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:QuerySet"/> class.
        /// </summary>
        public QuerySet()
        {
            Type = QuerySetType.None;
        }

        /// <summary>
        /// Gets or sets the type of the query set.
        /// </summary>
        /// <value>The type of the query set.</value>
        public QuerySetType Type
        {
            get;
            private set;
        }

        public void NotFound()
        {
            Type = QuerySetType.None;
        }

        T _single;

        public T Single
        {
            get { return _single; }
        }

        protected IEnumerable<T> _multiple;

        public IEnumerable<T> Multiple
        {
            get { return _multiple; }
        }

        /// <summary>
        /// Query returns one result
        /// </summary>
        /// <param name="single"></param>
        public void Found(T single)
        {
            if (single == null)
                NotFound();
            else
            {
                Type = QuerySetType.Single;
                _single = single;
            }
        }

        /// <summary>
        /// Query returns one or more result
        /// </summary>
        /// <param name="multiple"></param>
        public void Found(IEnumerable<T> multiple)
        {
            if (multiple == null)
                NotFound();
            else if (!multiple.Any())
            {
                _multiple = multiple;
                NotFound();
            }
            else
            {
                Type = QuerySetType.Multiple;
                _multiple = multiple;
            }
        }

        Exception _ex;
        public Exception Exception
        {
            get { return _ex; }
        }

        public void Error(Exception ex)
        {
            _ex = ex;
            Type = QuerySetType.QueryError;
        }

        public int Count
        {
            get
            {
                switch (Type)
                {
                    case QuerySetType.None: return 0;
                    case QuerySetType.Single: return 1;
                    case QuerySetType.Multiple: return _multiple.Count();
                    default: return 0;
                }
            }
        }

        public bool IsFound
        {
            get { return (Type != QuerySetType.None && Type != QuerySetType.QueryError); }
        }
    }
}

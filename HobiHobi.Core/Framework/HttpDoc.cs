using System;

namespace HobiHobi.Core.Framework
{

    public class HttpDoc<T>
    {
        /// <summary>
        /// Gets or sets the HTTP Status Code.
        /// </summary>
        /// <value>The summary.</value>
        public int StatusCode
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the HTTP Status Message .
        /// </summary>
        /// <value>The status message.</value>
        public string StatusMessage
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the error details.
        /// </summary>
        /// <value>The error details.</value>
        public string ErrorDetails
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>The data.</value>
        public T Data
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the date created.
        /// </summary>
        /// <value>The date created.</value>
        public DateTime DateCreated
        {
            get;
            set;
        }

        public static HttpDoc<T> OK(T data)
        {
            return new HttpDoc<T>
            {
                StatusCode = 200,
                StatusMessage = "OK",
                Data = data,
                DateCreated = Stamp.Time()
            };
        }

        public static HttpDoc<T> Unauthorized(string errorDetails)
        {
            return new HttpDoc<T>
            {
                StatusCode = 401,
                StatusMessage = "Unauthorized",
                ErrorDetails = errorDetails,
                DateCreated = Stamp.Time()
            };
        }

        public static HttpDoc<T> NotFound(string errorDetails)
        {
            return new HttpDoc<T>
            {
                StatusCode = 404,
                StatusMessage = "Not Found",
                ErrorDetails = errorDetails,
                DateCreated = Stamp.Time()
            };
        }

        public static HttpDoc<T> PreconditionFailed(string errorDetails)
        {
            return new HttpDoc<T>
            {
                StatusCode = 412,
                StatusMessage = "Precondition Failed",
                ErrorDetails = errorDetails,
                DateCreated = Stamp.Time()
            };
        }

        public static HttpDoc<T> InternalServerError(string errorDetails)
        {
            return new HttpDoc<T>
            {
                StatusCode = 500,
                StatusMessage = "Internal Server Error",
                ErrorDetails = errorDetails,
                DateCreated = Stamp.Time()
            };
        }
    }
}

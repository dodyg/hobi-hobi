using System;

namespace HobiHobi.Core.Framework
{
    /// <summary>
    /// This is a generic class that indicates result of an operation with two values (status, value)
    /// </summary>
    /// <typeparam name="S">Status</typeparam>
    /// <typeparam name="T">Payload</typeparam>
    public class Result2<S, T>
    {
        public S Status { get; private set; }

        public T Value { get; private set; }

        private Exception _exceptionObject;
        public Exception ExceptionObject
        {
            get
            {
                //This is deliberate because sometime people do not always pass exception when the false value is set. So 
                //if somebody tries to retrieve this Exception object while the result wasn't set in the first place, we must give them valid 
                //exception object although it won't contains much information about stack trace
                if (_exceptionObject == null && IsFalse)
                    _exceptionObject = new ApplicationException(Message ?? "There is no exception message available");

                return _exceptionObject;

            }

            protected set
            {
                _exceptionObject = value;
            }
        }


        public Result2()
        {

        }

        public Result2(S status, T payload)
        {
            Status = status;
            Value = payload;
        }

        public Result2(Exception e)
        {
            ExceptionObject = e;
        }

        public string Message { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is true.
        /// </summary>
        /// <value><c>true</c> if this instance is true; otherwise, <c>false</c>.</value>
        public bool IsTrue
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is false.
        /// </summary>
        /// <value><c>true</c> if this instance is false; otherwise, <c>false</c>.</value>
        public bool IsFalse
        {
            get { return !IsTrue; }

            set { IsTrue = !value; }
        }

        /// <summary>
        /// Sets the true value
        /// </summary>
        /// <param name="val">The value.</param>
        public void SetTrue(S status, T val)
        {
            IsTrue = true;
            Status = status;
            Value = val;
        }

        public void SetFalse()
        {
            IsTrue = false;
        }

        /// <summary>
        /// Sets the false value by passing exception
        /// </summary>
        /// <param name="e">The e.</param>
        public void SetFalse(Exception e)
        {
            IsTrue = false;
            ExceptionObject = e;
            Message = e.Message;
        }

        /// <summary>
        /// Return false return value.
        /// </summary>
        /// <returns></returns>
        public static Result2<S, T> False()
        {
            var r = new Result2<S, T>();
            r.SetFalse();

            return r;
        }

        /// <summary>
        /// Return false return value.
        /// </summary>
        /// <param name="e">The e.</param>
        /// <returns></returns>
        public static Result2<S, T> False(Exception e)
        {
            var r = new Result2<S, T>();
            r.SetFalse(e);

            return r;
        }

        public static Result2<S, T> False(string errorMsg)
        {
            var r = new Result2<S, T>();
            r.SetFalse();
            r.Message = errorMsg;

            return r;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static Result2<S, T> True(S status, T val)
        {
            var r = new Result2<S, T>();
            r.SetTrue(status, val);

            return r;
        }
    }
}

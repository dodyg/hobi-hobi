﻿using System;
using System.Collections.Generic;

namespace HobiHobi.Core.Framework
{
    /// <summary>
    /// This structure to indicates no value (to be used with generics). We need to resort to this because System.Void does not work.
    /// </summary>
    public class None
    {
        public static None Instance
        {
            get { return new None(); }
        }

        public static Result<None> True()
        {
            return Result<None>.True(None.Instance);
        }

        public static Result<None> False(Exception ex)
        {
            return Result<None>.False(ex);
        }
    }

    /// <summary>
    /// This structure to indicates no value (to be used with generics). We need to resort to this because System.Void does not work.
    /// </summary>
    public class Nothing : None
    {
        /// <summary>
        /// Return an instance of Nothing
        /// </summary>
        public static new Nothing Instance
        {
            get { return new Nothing(); }
        }

        /// <summary>
        /// Return an empty list of Nothing
        /// </summary>
        public static List<Nothing> EmptyList
        {
            get { return new List<Nothing>() { }; }
        }

        public static List<Nothing> OneItemList
        {
            get { return new List<Nothing>() { Nothing.Instance }; }
        }

    }
}

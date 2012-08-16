using System;
using System.Globalization;

namespace HobiHobi.Core.Framework
{
    public static class DateTimeExtensions
    {
        public static string ToIsoTime(this DateTime time)
        {
            return time.ToString("yyyy-MM-ddTHH:mm:ssZ", DateTimeFormatInfo.InvariantInfo);
        }
    }

    public static class DateTimeOffsetExtensions
    {
        public static string ToIsoTime(this DateTimeOffset time)
        {
            return time.ToString("yyyy-MM-ddTHH:mm:ssZ", DateTimeFormatInfo.InvariantInfo);
        }
    }
}

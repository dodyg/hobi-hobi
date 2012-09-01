using System;

namespace HobiHobi.Core.Framework
{
    public static class Stamp
    {
        public static DateTime Time()
        {
            return DateTime.UtcNow;
        }

        public static string ETag()
        {
            return Guid.NewGuid().ToString();
        }

        public static Guid GUID()
        {
            return Guid.NewGuid();
        }
    }
}

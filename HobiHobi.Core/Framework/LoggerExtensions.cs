using System;
using NLog;

namespace HobiHobi.Core.Framework
{
    public static class LoggerExtensions
    {
        public static void ReportException(this Logger log, string msg, Exception ex)
        {
            log.Debug("EXCEPTION {0} - {1} - STACK - {2}".F(msg, ex.Message, ex.StackTrace));
        }
    }
}

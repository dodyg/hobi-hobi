using NLog;

namespace HobiHobi.Core.Framework
{
    public interface ILogParticipant
    {
        Logger Log { get; set; }
    }
}

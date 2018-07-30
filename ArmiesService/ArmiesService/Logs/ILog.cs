using System;

namespace ArmiesService.Logs
{
    public interface ILog
    {
        void Information(string message);

        void Warning(string message);

        void Error(Exception e, string message);
    }
}

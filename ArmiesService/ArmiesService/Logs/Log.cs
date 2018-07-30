using Microsoft.Extensions.Logging;
using System;

namespace ArmiesService.Logs
{
    class Log : ILog
    {
        private readonly ILogger<Log> logger;

        public Log(ILogger<Log> logger)
        {
            this.logger = logger;
        }

        public void Information(string message)
        {
            logger.LogInformation(message);
        }

        public void Warning(string message)
        {
            logger.LogWarning(message);
        }

        public void Error(Exception e, string message)
        {
            logger.LogError(e, message);
        }
    }
}

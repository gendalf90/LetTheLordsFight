using Microsoft.Extensions.Logging;

namespace UsersService.Logs
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
    }
}

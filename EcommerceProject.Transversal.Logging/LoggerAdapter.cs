using EcommerceProject.Transversal.Common;
using Microsoft.Extensions.Logging;
using WatchDog;

namespace EcommerceProject.Transversal.Logging
{
    public class LoggerAdapter<T> : IAppLogger<T>
    {
        private readonly ILogger<T> _logger;

        public LoggerAdapter(ILoggerFactory LoggerFactory)
        {
            _logger = LoggerFactory.CreateLogger<T>();
        }


        public void LogInformation(string message, params object[] args)
        {
            _logger.LogInformation(message, args);
            WatchLogger.Log(message);
        }

        public void LogWarning(string message, params object[] args)
        {
            _logger.LogWarning(message, args);
            WatchLogger.Log(message);
        }

        public void LogError(string message, params object[] args)
        {
            _logger.LogError(message, args);
            WatchLogger.Log(message);
        }
    }
}
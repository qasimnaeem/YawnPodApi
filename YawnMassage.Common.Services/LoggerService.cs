using YawnMassage.Common.Domain.Contracts;
using Microsoft.Extensions.Logging;
using System;

namespace YawnMassage.Common.Services
{
    public class LoggerService : ILoggerService
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger<LoggerService> _genericLogger;

        public LoggerService(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
            _genericLogger = _loggerFactory.CreateLogger<LoggerService>();
        }

        public void LogError(string errorMessage)
        {
            _genericLogger.LogError(errorMessage);
        }

        public void LogError(Exception ex, string errorMessage)
        {
            _genericLogger.LogError(ex, errorMessage);
        }

        public void LogInfo(string message)
        {
            _genericLogger.LogInformation(message);
        }

        public void LogTrace(string message)
        {
            _genericLogger.LogTrace(message);
        }

        public void LogDebug(string message)
        {
            _genericLogger.LogDebug(message);
        }

        public void LogError<T>(string errorMessage)
        {
            GetLogger<T>().LogError(errorMessage);
        }

        public void LogError<T>(Exception ex, string errorMessage)
        {
            GetLogger<T>().LogError(ex, errorMessage);
        }

        public void LogInfo<T>(string message)
        {
            GetLogger<T>().LogInformation(message);
        }

        public void LogTrace<T>(string message)
        {
            GetLogger<T>().LogTrace(message);
        }

        public void LogDebug<T>(string message)
        {
            GetLogger<T>().LogDebug(message);
        }

        private ILogger<T> GetLogger<T>()
        {
            var logger = _loggerFactory.CreateLogger<T>();
            return logger;
        }
    }
}

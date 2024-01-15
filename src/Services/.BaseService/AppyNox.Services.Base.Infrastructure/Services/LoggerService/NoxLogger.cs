using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Base.Core.AsyncLocals;
using Microsoft.Extensions.Logging;

namespace AppyNox.Services.Base.Infrastructure.Services.LoggerService
{
    public class NoxLogger(ILogger<INoxLogger> logger, string layer) : INoxLogger
    {
        #region [ Fields ]

        protected readonly ILogger _logger = logger;

        private readonly string _layer = layer;

        #endregion

        #region [ Public Methods ]

        public virtual void LogTrace(string message)
        {
            if (_logger.IsEnabled(LogLevel.Trace))
            {
                _logger.LogTrace("{@LogData}", CreateLogData(message, LogLevel.Trace));
            }
        }

        public virtual void LogDebug(string message)
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("{@LogData}", CreateLogData(message, LogLevel.Debug));
            }
        }

        public virtual void LogInformation(string message)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("{@LogData}", CreateLogData(message, LogLevel.Information));
            }
        }

        public virtual void LogWarning(string message)
        {
            if (_logger.IsEnabled(LogLevel.Warning))
            {
                _logger.LogWarning("{@LogData}", CreateLogData(message, LogLevel.Warning));
            }
        }

        public virtual void LogError(Exception exception, string message)
        {
            if (_logger.IsEnabled(LogLevel.Error))
            {
                _logger.LogError(exception, "{@LogData}", CreateLogData(message, LogLevel.Error));
            }
        }

        public virtual void LogCritical(Exception exception, string message)
        {
            if (_logger.IsEnabled(LogLevel.Critical))
            {
                _logger.LogCritical(exception, "{@LogData}", CreateLogData(message, LogLevel.Critical));
            }
        }

        protected virtual object CreateLogData(string message, LogLevel logLevel)
        {
            var timeStamp = DateTime.UtcNow;
            var correlationId = CorrelationContext.CorrelationId;

            return new
            {
                Level = logLevel.ToString(),
                Layer = _layer,
                TimeStamp = timeStamp,
                Message = message,
                CorrelationId = correlationId
            };
        }

        #endregion
    }
}
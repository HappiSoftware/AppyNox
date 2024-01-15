using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Base.Core.AsyncLocals;
using Microsoft.Extensions.Logging;

namespace AppyNox.Services.Base.Infrastructure.Services.LoggerService
{
    /// <summary>
    /// Provides an implementation of <see cref="INoxApiLogger"/> for logging API-related messages.
    /// </summary>
    public class NoxApiLogger(ILogger<INoxApiLogger> logger) : NoxLogger(logger, "Api"), INoxApiLogger
    {
        #region [ Public Methods ]

        public void LogCritical(Exception exception, string message, Guid correlationId)
        {
            if (_logger.IsEnabled(LogLevel.Critical))
            {
                _logger.LogCritical(exception, "{@LogData}", CreateLogData(message, LogLevel.Critical, correlationId));
            }
        }

        #endregion

        #region [ Private Methods ]

        private static object CreateLogData(string message, LogLevel logLevel, Guid correlationId)
        {
            var timeStamp = DateTime.UtcNow;

            return new
            {
                Level = logLevel.ToString(),
                Layer = "Api",
                TimeStamp = timeStamp,
                Message = message,
                CorrelationId = correlationId
            };
        }

        #endregion
    }
}
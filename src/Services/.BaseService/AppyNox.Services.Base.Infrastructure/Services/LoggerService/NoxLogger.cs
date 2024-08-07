using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Base.Core.AsyncLocals;
using Microsoft.Extensions.Logging;

namespace AppyNox.Services.Base.Infrastructure.Services.LoggerService;

public class NoxLogger<T>(ILogger<T> logger, string layer) : INoxLogger
{
    #region [ Fields ]

    protected readonly ILogger<T> _logger = logger;

    private readonly string _layer = layer;

    #endregion

    #region [ Public Methods ]

    public virtual void LogTrace(string message, bool includeContext = true)
    {
        if (_logger.IsEnabled(LogLevel.Trace))
        {
            _logger.LogTrace("{@LogData}", CreateLogData(message, LogLevel.Trace, includeContext));
        }
    }

    public virtual void LogDebug(string message, bool includeContext = true)
    {
        if (_logger.IsEnabled(LogLevel.Debug))
        {
            _logger.LogDebug("{@LogData}", CreateLogData(message, LogLevel.Debug, includeContext));
        }
    }

    public virtual void LogInformation(string message, bool includeContext = true)
    {
        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation("{@LogData}", CreateLogData(message, LogLevel.Information, includeContext));
        }
    }

    public virtual void LogWarning(string message, bool includeContext = true)
    {
        if (_logger.IsEnabled(LogLevel.Warning))
        {
            _logger.LogWarning("{@LogData}", CreateLogData(message, LogLevel.Warning, includeContext));
        }
    }

    public virtual void LogError(Exception exception, string message, bool includeContext = true)
    {
        if (_logger.IsEnabled(LogLevel.Error))
        {
            _logger.LogError(exception, "{@LogData}", CreateLogData(message, LogLevel.Error, includeContext));
        }
    }

    public virtual void LogCritical(Exception exception, string message, bool includeContext = true)
    {
        if (_logger.IsEnabled(LogLevel.Critical))
        {
            _logger.LogCritical(exception, "{@LogData}", CreateLogData(message, LogLevel.Critical, includeContext));
        }
    }

    protected virtual object CreateLogData(string message, LogLevel logLevel, bool includeContext)
    {
        var timeStamp = DateTime.UtcNow;

        var logData = new
        {
            Level = logLevel.ToString(),
            Layer = _layer,
            TimeStamp = timeStamp,
            Message = message,
            SystemLog = true
        };

        if (includeContext)
        {
            return new
            {
                logData.Level,
                logData.Layer,
                logData.TimeStamp,
                logData.Message,
                NoxContext.CorrelationId,
                NoxContext.CompanyId,
                NoxContext.UserId
            };
        }

        return logData;
    }


    #endregion
}
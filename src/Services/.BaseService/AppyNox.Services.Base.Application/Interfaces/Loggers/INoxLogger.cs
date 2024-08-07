namespace AppyNox.Services.Base.Application.Interfaces.Loggers;

public interface INoxLogger
{
    #region Public Methods

    void LogCritical(Exception exception, string message, bool includeContext = true);

    void LogDebug(string message, bool includeContext = true);

    void LogError(Exception exception, string message, bool includeContext = true);

    void LogInformation(string message, bool includeContext = true);

    void LogTrace(string message, bool includeContext = true);

    void LogWarning(string message, bool includeContext = true);

    #endregion
}
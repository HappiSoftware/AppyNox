namespace AppyNox.Services.Base.Application.Interfaces.Loggers
{
    public interface INoxLogger
    {
        #region Public Methods

        void LogCritical(Exception exception, string message);

        void LogDebug(string message);

        void LogError(Exception exception, string message);

        void LogInformation(string message);

        void LogTrace(string message);

        void LogWarning(string message);

        #endregion
    }
}
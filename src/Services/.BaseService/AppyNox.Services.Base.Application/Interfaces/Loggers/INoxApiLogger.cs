namespace AppyNox.Services.Base.Application.Interfaces.Loggers
{
    /// <summary>
    /// Defines the logger interface for API-related logging.
    /// </summary>
    public interface INoxApiLogger : INoxLogger
    {
        #region [ Public Methods ]

        void LogCritical(Exception exception, string message, Guid correlationId);

        #endregion
    }
}
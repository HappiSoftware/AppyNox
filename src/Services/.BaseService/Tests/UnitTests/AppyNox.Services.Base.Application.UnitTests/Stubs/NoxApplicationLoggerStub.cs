using AppyNox.Services.Base.Application.Interfaces.Loggers;

namespace AppyNox.Services.Base.Application.UnitTests.Stubs
{
    /// <summary>
    /// A stub implementation of <see cref="INoxApplicationLogger"/> for unit testing, simulating logging behavior.
    /// </summary>
    public class NoxApplicationLoggerStub<T> : INoxApplicationLogger<T>
    {
        #region [ Public Methods ]

        public void LogTrace(string message, bool includeContext = false)
        {
            Console.WriteLine($"Trace: {message}");
        }

        public void LogDebug(string message, bool includeContext = false)
        {
            Console.WriteLine($"Debug: {message}");
        }

        public void LogInformation(string message, bool includeContext = false)
        {
            Console.WriteLine($"Information: {message}");
        }

        public void LogWarning(string message, bool includeContext = false)
        {
            Console.WriteLine($"Warning: {message}");
        }

        public void LogError(Exception exception, string message, bool includeContext = false)
        {
            Console.WriteLine($"Error: {message}. Exception: {exception.Message}");
        }

        public void LogCritical(Exception exception, string message, bool includeContext = false)
        {
            Console.WriteLine($"Critical: {message}. Exception: {exception.Message}");
        }

        #endregion
    }
}
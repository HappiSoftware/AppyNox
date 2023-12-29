using AppyNox.Services.Base.Application.Logger;

namespace AppyNox.Services.Base.IntegrationTests.Stubs
{
    /// <summary>
    /// A stub implementation of <see cref="INoxApplicationLogger"/> for integration testing, simulating logging behavior.
    /// </summary>
    public class NoxApplicationLoggerStub : INoxApplicationLogger
    {
        #region [ Public Methods ]

        public void LogTrace(string message)
        {
            Console.WriteLine($"Trace: {message}");
        }

        public void LogDebug(string message)
        {
            Console.WriteLine($"Debug: {message}");
        }

        public void LogInformation(string message)
        {
            Console.WriteLine($"Information: {message}");
        }

        public void LogWarning(string message)
        {
            Console.WriteLine($"Warning: {message}");
        }

        public void LogError(Exception exception, string message)
        {
            Console.WriteLine($"Error: {message}. Exception: {exception.Message}");
        }

        public void LogCritical(Exception exception, string message)
        {
            Console.WriteLine($"Critical: {message}. Exception: {exception.Message}");
        }

        #endregion
    }
}
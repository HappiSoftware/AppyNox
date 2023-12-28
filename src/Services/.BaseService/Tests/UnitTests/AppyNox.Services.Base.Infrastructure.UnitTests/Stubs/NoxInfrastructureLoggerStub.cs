using AppyNox.Services.Base.Infrastructure.Logger;

namespace AppyNox.Services.Base.Infrastructure.UnitTests.Stubs
{
    public class NoxInfrastructureLoggerStub : INoxInfrastructureLogger
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
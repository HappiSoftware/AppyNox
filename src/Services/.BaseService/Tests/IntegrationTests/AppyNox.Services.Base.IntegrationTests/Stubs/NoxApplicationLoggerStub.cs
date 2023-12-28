using AppyNox.Services.Base.Application.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.Base.IntegrationTests.Stubs
{
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
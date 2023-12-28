
namespace AppyNox.Services.Base.Infrastructure.Services
{
    public interface INoxLogger
    {
        void LogCritical(Exception exception, string message);
        void LogDebug(string message);
        void LogError(Exception exception, string message);
        void LogInformation(string message);
        void LogTrace(string message);
        void LogWarning(string message);
    }
}
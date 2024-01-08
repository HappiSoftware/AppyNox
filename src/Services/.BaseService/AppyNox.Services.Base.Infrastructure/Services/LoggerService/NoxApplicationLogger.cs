using AppyNox.Services.Base.Application.Interfaces.Loggers;
using Microsoft.Extensions.Logging;

namespace AppyNox.Services.Base.Infrastructure.Services.LoggerService
{
    /// <summary>
    /// Provides an implementation of <see cref="INoxApplicationLogger"/> for logging application-related messages.
    /// </summary>
    public class NoxApplicationLogger(ILogger<INoxApplicationLogger> logger) : NoxLogger(logger, "Application"), INoxApplicationLogger
    {
    }
}
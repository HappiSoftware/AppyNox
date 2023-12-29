using AppyNox.Services.Base.Infrastructure.Logger;
using AppyNox.Services.Base.Infrastructure.Services.LoggerService;
using Microsoft.Extensions.Logging;

namespace AppyNox.Services.Base.Application.Logger
{
    /// <summary>
    /// Provides an implementation of <see cref="INoxApplicationLogger"/> for logging application-related messages.
    /// </summary>
    public class NoxApplicationLogger(ILogger<INoxApplicationLogger> logger) : NoxLogger(logger, "Application"), INoxApplicationLogger
    {
    }
}
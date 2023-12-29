using AppyNox.Services.Base.Infrastructure.Services.LoggerService;
using Microsoft.Extensions.Logging;

namespace AppyNox.Services.Base.API.Logger
{
    /// <summary>
    /// Provides an implementation of <see cref="INoxApiLogger"/> for logging API-related messages.
    /// </summary>
    public class NoxApiLogger(ILogger<INoxApiLogger> logger) : NoxLogger(logger, "Api"), INoxApiLogger
    {
    }
}
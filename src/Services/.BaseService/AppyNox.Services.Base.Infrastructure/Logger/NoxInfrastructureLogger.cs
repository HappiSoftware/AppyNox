using AppyNox.Services.Base.Infrastructure.Services.LoggerService;
using Microsoft.Extensions.Logging;

namespace AppyNox.Services.Base.Infrastructure.Logger
{
    /// <summary>
    /// Provides an implementation of <see cref="INoxInfrastructureLogger"/> for logging infrastructure-related messages.
    /// This class extends the functionality of <see cref="NoxLogger"/> to focus specifically on the infrastructure layer.
    /// </summary>
    public class NoxInfrastructureLogger(ILogger<INoxInfrastructureLogger> logger) : NoxLogger(logger, "Infrastructure"), INoxInfrastructureLogger
    {
    }
}
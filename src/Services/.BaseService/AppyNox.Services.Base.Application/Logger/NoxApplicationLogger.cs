using AppyNox.Services.Base.Infrastructure.Logger;
using AppyNox.Services.Base.Infrastructure.Services.LoggerService;
using Microsoft.Extensions.Logging;

namespace AppyNox.Services.Base.Application.Logger
{
    public class NoxApplicationLogger(ILogger<INoxApplicationLogger> logger) : NoxLogger(logger, "Application"), INoxApplicationLogger
    {
    }
}
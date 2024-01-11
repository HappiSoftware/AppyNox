﻿using AppyNox.Services.Base.Application.Interfaces.Loggers;
using Microsoft.Extensions.Logging;

namespace AppyNox.Services.Base.Infrastructure.Services.LoggerService
{
    /// <summary>
    /// Provides an implementation of <see cref="INoxApiLogger"/> for logging API-related messages.
    /// </summary>
    public class NoxApiLogger(ILogger<INoxApiLogger> logger) : NoxLogger(logger, "Api"), INoxApiLogger
    {
    }
}
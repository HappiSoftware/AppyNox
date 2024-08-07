using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Base.Core.AsyncLocals;
using Microsoft.Extensions.Logging;

namespace AppyNox.Services.Base.Infrastructure.Services.LoggerService;

/// <summary>
/// Provides an implementation of <see cref="INoxApiLogger<T>"/> for logging API-related messages.
/// </summary>
public class NoxApiLogger<T>(ILogger<T> logger) : NoxLogger<T>(logger, "Api"), INoxApiLogger<T>
{
}
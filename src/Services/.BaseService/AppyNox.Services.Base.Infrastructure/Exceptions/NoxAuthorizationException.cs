using AppyNox.Services.Base.Infrastructure.Exceptions.Base;
using AppyNox.Services.Base.Infrastructure.Exceptions.Interfaces;
using System.Net;

namespace AppyNox.Services.Base.Infrastructure.Exceptions;

internal class NoxAuthorizationException(string message, int exceptionCode)
        : NoxInfrastructureException(
            message,
            exceptionCode,
            (int)HttpStatusCode.Forbidden), INoxAuthorizationException
{
}
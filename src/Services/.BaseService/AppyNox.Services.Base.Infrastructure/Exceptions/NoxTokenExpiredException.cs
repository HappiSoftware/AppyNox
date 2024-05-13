using AppyNox.Services.Base.Infrastructure.Exceptions.Base;
using AppyNox.Services.Base.Infrastructure.Exceptions.Interfaces;
using System.Net;

namespace AppyNox.Services.Base.Infrastructure.Exceptions;

internal class NoxTokenExpiredException(string message)
        : NoxInfrastructureException(
            message,
            (int)NoxInfrastructureExceptionCode.ExpiredToken,
            (int)HttpStatusCode.Unauthorized), INoxAuthenticationException
{
}
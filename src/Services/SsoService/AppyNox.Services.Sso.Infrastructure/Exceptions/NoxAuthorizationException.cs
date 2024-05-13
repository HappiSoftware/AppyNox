using AppyNox.Services.Base.Infrastructure.Exceptions.Interfaces;
using AppyNox.Services.Sso.Infrastructure.Exceptions.Base;
using System.Net;

namespace AppyNox.Services.Sso.Infrastructure.Exceptions;

internal class NoxAuthorizationException(string message, int exceptionCode)
        : NoxSsoInfrastructureException(message, exceptionCode, (int)HttpStatusCode.Forbidden), INoxAuthorizationException
{
}
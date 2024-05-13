using AppyNox.Services.Base.Infrastructure.Exceptions.Interfaces;
using AppyNox.Services.Sso.Infrastructure.Exceptions.Base;
using System.Net;

namespace AppyNox.Services.Sso.Infrastructure.Exceptions;

internal class NoxTokenExpiredException(string message)
        : NoxSsoInfrastructureException(message, (int)NoxSsoInfrastructureExceptionCode.ExpiredToken, (int)HttpStatusCode.Unauthorized), 
            INoxAuthenticationException, 
            INoxTokenExpiredException
{
}

public interface INoxTokenExpiredException
{
    // Marker Interface
}
using AppyNox.Services.Base.API.Exceptions.Base;
using AppyNox.Services.Base.API.Exceptions.Interfaces;
using System.Net;

namespace AppyNox.Services.Base.API.Exceptions;

internal class NoxTokenExpiredException(string message)
        : NoxApiException(
            message,
            (int)NoxApiExceptionCode.ExpiredToken,
            (int)HttpStatusCode.Unauthorized), INoxAuthenticationException
{
}
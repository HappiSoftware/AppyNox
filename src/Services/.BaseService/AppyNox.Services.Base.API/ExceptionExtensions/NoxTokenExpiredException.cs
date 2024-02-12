using AppyNox.Services.Base.API.ExceptionExtensions.Base;
using AppyNox.Services.Base.API.ExceptionExtensions.Interfaces;
using System.Net;

namespace AppyNox.Services.Base.API.ExceptionExtensions;

internal class NoxTokenExpiredException(string message)
        : NoxApiException(message, (int)NoxApiExceptionCode.ExpiredToken, (int)HttpStatusCode.Unauthorized), INoxAuthenticationException
{
}
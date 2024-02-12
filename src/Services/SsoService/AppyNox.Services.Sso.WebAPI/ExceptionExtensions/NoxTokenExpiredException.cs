using AppyNox.Services.Base.API.ExceptionExtensions.Interfaces;
using AppyNox.Services.Sso.WebAPI.ExceptionExtensions.Base;
using System.Net;

namespace AppyNox.Services.Sso.WebAPI.ExceptionExtensions;

internal class NoxTokenExpiredException(string message)
        : NoxSsoApiException(message, (int)NoxSsoApiExceptionCode.ExpiredToken, (int)HttpStatusCode.Unauthorized), INoxAuthenticationException
{
}
using AppyNox.Services.Base.API.Exceptions.Interfaces;
using AppyNox.Services.Sso.WebAPI.Exceptions.Base;
using System.Net;

namespace AppyNox.Services.Sso.WebAPI.Exceptions;

internal class NoxTokenExpiredException(string message)
        : NoxSsoApiException(message, (int)NoxSsoApiExceptionCode.ExpiredToken, (int)HttpStatusCode.Unauthorized), INoxAuthenticationException
{
}
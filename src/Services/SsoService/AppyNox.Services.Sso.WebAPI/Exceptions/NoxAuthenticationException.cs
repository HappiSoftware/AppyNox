using System.Net;
using AppyNox.Services.Base.API.Exceptions.Interfaces;
using AppyNox.Services.Sso.WebAPI.Exceptions.Base;

namespace AppyNox.Services.Sso.WebAPI.Exceptions;

internal class NoxAuthenticationException(string message, int exceptionCode)
        : NoxSsoApiException(message, exceptionCode, (int)HttpStatusCode.Unauthorized), INoxAuthenticationException
{
}
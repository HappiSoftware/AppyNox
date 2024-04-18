using System.Net;
using AppyNox.Services.Base.API.Exceptions.Interfaces;
using AppyNox.Services.Sso.WebAPI.Exceptions.Base;

namespace AppyNox.Services.Sso.WebAPI.Exceptions;

internal class NoxAuthorizationException(string message, int exceptionCode)
        : NoxSsoApiException(message, exceptionCode, (int)HttpStatusCode.Forbidden), INoxAuthorizationException
{
}
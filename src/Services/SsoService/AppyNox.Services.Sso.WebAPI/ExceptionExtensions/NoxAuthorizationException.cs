using AppyNox.Services.Sso.WebAPI.ExceptionExtensions.Base;
using AppyNox.Services.Base.API.ExceptionExtensions.Interfaces;
using System.Net;

namespace AppyNox.Services.Sso.WebAPI.ExceptionExtensions
{
    internal class NoxAuthorizationException(string message, int exceptionCode)
        : NoxSsoApiException(message, exceptionCode, (int)HttpStatusCode.Forbidden), INoxAuthorizationException
    {
    }
}
using AppyNox.Services.Authentication.WebAPI.ExceptionExtensions.Base;
using AppyNox.Services.Base.API.ExceptionExtensions.Interfaces;
using System.Net;

namespace AppyNox.Services.Authentication.WebAPI.ExceptionExtensions
{
    internal class NoxAuthenticationException(string message, int exceptionCode)
        : NoxSsoApiException(message, exceptionCode, (int)HttpStatusCode.Unauthorized), INoxAuthenticationException
    {
    }
}
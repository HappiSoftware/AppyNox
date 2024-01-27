using AppyNox.Services.Base.API.ExceptionExtensions.Base;
using AppyNox.Services.Base.API.ExceptionExtensions.Interfaces;
using System.Net;

namespace AppyNox.Services.Base.API.ExceptionExtensions
{
    internal class NoxAuthenticationException(string message, int exceptionCode)
        : NoxApiException(message, exceptionCode, (int)HttpStatusCode.Unauthorized), INoxAuthenticationException
    {
    }
}
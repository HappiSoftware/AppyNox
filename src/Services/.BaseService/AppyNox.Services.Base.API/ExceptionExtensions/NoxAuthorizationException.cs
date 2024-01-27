using AppyNox.Services.Base.API.ExceptionExtensions.Base;
using AppyNox.Services.Base.API.ExceptionExtensions.Interfaces;
using System.Net;

namespace AppyNox.Services.Base.API.ExceptionExtensions
{
    internal class NoxAuthorizationException(string message, int exceptionCode)
        : NoxApiException(message, exceptionCode, (int)HttpStatusCode.Forbidden), INoxAuthorizationException
    {
    }
}
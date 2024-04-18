using AppyNox.Services.Base.API.Exceptions.Base;
using AppyNox.Services.Base.API.Exceptions.Interfaces;
using System.Net;

namespace AppyNox.Services.Base.API.Exceptions;

internal class NoxAuthorizationException(string message, int exceptionCode)
        : NoxApiException(
            message,
            exceptionCode,
            (int)HttpStatusCode.Forbidden), INoxAuthorizationException
{
}
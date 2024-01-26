using AppyNox.Services.Base.API.ExceptionExtensions.Base;
using AppyNox.Services.Base.API.ExceptionExtensions.Interfaces;
using System.Net;

namespace AppyNox.Services.Base.API.ExceptionExtensions
{
    public class NoxAuthorizationException(string message)
        : NoxApiException(message, (int)HttpStatusCode.Forbidden, "Base"), INoxAuthorizationException
    {
    }
}
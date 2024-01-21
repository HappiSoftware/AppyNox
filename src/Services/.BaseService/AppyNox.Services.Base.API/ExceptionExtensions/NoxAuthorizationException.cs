using AppyNox.Services.Base.API.ExceptionExtensions.Base;
using AppyNox.Services.Base.API.ExceptionExtensions.Interfaces;
using System.Net;

namespace AppyNox.Services.Base.API.ExceptionExtensions
{
    public class NoxAuthorizationException(string message = "You have no claims to take this action.")
        : NoxApiException(message, (int)HttpStatusCode.Forbidden, "Base"), INoxAuthorizationException
    {
    }
}
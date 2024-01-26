using AppyNox.Services.Base.API.ExceptionExtensions.Base;
using System.Net;

namespace AppyNox.Services.Base.API.ExceptionExtensions
{
    public class NoxTokenExpiredException(string message)
        : NoxApiException(message, (int)HttpStatusCode.Unauthorized)
    {
    }
}
using AppyNox.Services.Authentication.WebAPI.ExceptionExtensions.Base;
using System.Net;

namespace AppyNox.Services.Authentication.WebAPI.ExceptionExtensions
{
    internal class NoxTokenExpiredException(string message)
        : NoxSsoApiException(message, (int)NoxSsoApiExceptionCode.ExpiredToken, (int)HttpStatusCode.Unauthorized)
    {
    }
}
using AppyNox.Services.Base.API.ExceptionExtensions.Base;

namespace AppyNox.Services.Base.API.ExceptionExtensions
{
    internal class AsyncLocalException(string message, int statusCode)
        : NoxApiException(message, statusCode)
    {
    }
}
using AppyNox.Services.Base.API.ExceptionExtensions.Base;
using System.Net;

namespace AppyNox.Services.Base.API.ExceptionExtensions
{
    /// <summary>
    /// Exception thrown when a required correlation ID is missing in an API request.
    /// </summary>
    internal class MissingCorrelationIdException(string message) : NoxApiException(message, (int)HttpStatusCode.BadRequest)
    {
    }
}
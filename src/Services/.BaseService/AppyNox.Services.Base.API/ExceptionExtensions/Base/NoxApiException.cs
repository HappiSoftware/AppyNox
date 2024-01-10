using AppyNox.Services.Base.Domain.ExceptionExtensions.Base;
using AppyNox.Services.Base.Domain.ExceptionExtensions.Enums;

namespace AppyNox.Services.Base.API.ExceptionExtensions.Base
{
    /// <summary>
    /// Represents exceptions specific to the API layer of the application.
    /// </summary>
    public class NoxApiException(string message, int statusCode, string service = "Base")
        : NoxException(ExceptionThrownLayer.Api, service, message, statusCode)
    {
    }
}
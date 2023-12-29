using AppyNox.Services.Base.Domain.ExceptionExtensions.Base;
using AppyNox.Services.Base.Domain.ExceptionExtensions.Enums;

namespace AppyNox.Services.Base.API.ExceptionExtensions.Base
{
    /// <summary>
    /// Represents exceptions specific to the API layer of the application.
    /// </summary>
    internal class NoxApiException(string message, int statusCode)
        : NoxException(ExceptionThrownLayer.ApiBase, message, statusCode)
    {
    }
}
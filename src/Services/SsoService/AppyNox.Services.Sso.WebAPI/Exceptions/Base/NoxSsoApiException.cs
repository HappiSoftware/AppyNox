using AppyNox.Services.Base.API.Exceptions.Base;
using AppyNox.Services.Base.Core.Enums;
using AppyNox.Services.Sso.Domain;

namespace AppyNox.Services.Sso.WebAPI.Exceptions.Base;

#region [ NoxSsoApiException Code ]

internal enum NoxSsoApiExceptionCode
{
    RefreshTokenInvalid = 1000,
}

#endregion

internal class NoxSsoApiException(
    string? message = null,
    int? exceptionCode = null,
    int? statusCode = null,
    Exception? innerException = null)
    : NoxApiExceptionBase(
        ExceptionProduct.AppyNox,
        SsoCommonStrings.Service,
        message,
        exceptionCode,
        statusCode,
        innerException)
{
}
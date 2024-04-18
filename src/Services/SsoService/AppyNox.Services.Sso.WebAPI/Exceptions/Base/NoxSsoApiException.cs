using AppyNox.Services.Base.API.Exceptions.Base;
using AppyNox.Services.Base.Core.Enums;
using AppyNox.Services.Sso.Domain;

namespace AppyNox.Services.Sso.WebAPI.Exceptions.Base;

#region [ NoxSsoApiException Code ]

internal enum NoxSsoApiExceptionCode
{
    SsoServiceApiError = 999,

    AuthenticationInvalidToken = 1000,

    AuthenticationNullToken = 1001,

    ExpiredToken = 1002,

    AuthorizationFailed = 1003,

    AuthorizationInvalidToken = 1004,

    InvalidAudience = 1005,

    RefreshToken = 1006,

    WrongCredentials = 1007,

    RefreshTokenNotFound = 1008,

    SignInError = 1009,

    Teapot = 1010,

    RefreshTokenInvalid = 1011,
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
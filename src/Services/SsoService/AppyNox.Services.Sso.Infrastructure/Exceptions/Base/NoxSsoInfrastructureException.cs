using AppyNox.Services.Base.Core.Enums;
using AppyNox.Services.Base.Infrastructure.Exceptions.Base;
using AppyNox.Services.Sso.Domain;

namespace AppyNox.Services.Sso.Infrastructure.Exceptions.Base;

#region [ NoxSsoInfrastructureException Code ]

internal enum NoxSsoInfrastructureExceptionCode
{
    UserCreationSagaCorrelationId = 1000,

    UserCreationSagaLicenseIdNull = 1001,

    UserCreationSagaCompanyIdNull = 1002,

    DeleteUserConsumerError = 1003,

    CreateUserConsumerError = 1004,

    UserCreationSagaLicenseIdOrCompanyIdNullError = 1005,

    ExpiredToken = 1006,

    AuthorizationFailed = 1007,

    AuthorizationInvalidToken = 1008,

    AuthenticationInvalidToken = 1009,
    
    InvalidAudience = 1010,

    RefreshToken = 1011,

    WrongCredentials = 1012,

    RefreshTokenNotFound = 1013,

    MessageDataNotDeserialized = 1014,

}

#endregion

internal class NoxSsoInfrastructureException(
    string? message = null,
    int? exceptionCode = null,
    int? statusCode = null,
    Exception? innerException = null)
    : NoxInfrastructureExceptionBase(
        ExceptionProduct.AppyNox,
        SsoCommonStrings.Service,
        message,
        exceptionCode,
        statusCode,
        innerException)
{
}
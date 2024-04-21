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

    UserCreationSagaLicenseIdOrCompanyIdNullError = 1005
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
using AppyNox.Services.Base.Application.Exceptions.Base;
using AppyNox.Services.Base.Core.Enums;
using AppyNox.Services.Sso.Domain;

namespace AppyNox.Services.Sso.Application.Exceptions;

#region [ NoxSsoApplicationException Code ]

internal enum NoxSsoApplicationExceptionCode
{
    CreateUserCommandError = 1000,

    DeleteUserCommandError = 1001,
}

#endregion

internal class NoxSsoApplicationException(
    string? message = null,
    int? exceptionCode = null,
    int? statusCode = null,
    Exception? innerException = null)
    : NoxApplicationExceptionBase(
        ExceptionProduct.AppyNox,
        SsoCommonStrings.Service,
        message,
        exceptionCode,
        statusCode,
        innerException)
{
}
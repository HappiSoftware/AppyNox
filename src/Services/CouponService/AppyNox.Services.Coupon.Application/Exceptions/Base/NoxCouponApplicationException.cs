using AppyNox.Services.Base.Application.Exceptions.Base;
using AppyNox.Services.Base.Core.Enums;
using AppyNox.Services.Coupon.Domain;

namespace AppyNox.Services.Coupon.Application.Exceptions.Base;

#region [ NoxSsoApiException Code ]

internal enum NoxCouponApplicationExceptionCode
{
    IdsMismatch = 1000,

    UnexpectedUpdateCommandError = 1001,

    UnexpectedDomainEventHandlerError = 1002,

    DtoMappingRegistryError = 1003
}

#endregion

internal class NoxCouponApplicationException(
    string? message = null,
    int? exceptionCode = null,
    int? statusCode = null,
    Exception? innerException = null)
    : NoxApplicationExceptionBase(
        ExceptionProduct.AppyNox,
        NoxCouponCommonStrings.Service,
        message,
        exceptionCode,
        statusCode,
        innerException)
{
}
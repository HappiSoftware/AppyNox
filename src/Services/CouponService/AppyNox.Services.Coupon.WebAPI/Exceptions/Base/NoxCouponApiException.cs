using AppyNox.Services.Base.API.Exceptions.Base;
using AppyNox.Services.Base.Core.Enums;
using AppyNox.Services.Coupon.Domain;

namespace AppyNox.Services.Coupon.WebAPI.Exceptions.Base;

#region [ NoxCouponApiException Code ]

internal enum NoxCouponApiExceptionCode
{
}

#endregion

internal class NoxCouponApiException(
    string? message = null,
    int? exceptionCode = null,
    int? statusCode = null,
    Exception? innerException = null)
    : NoxApiExceptionBase(
        ExceptionProduct.AppyNox,
        NoxCouponCommonStrings.Service,
        message,
        exceptionCode,
        statusCode,
        innerException)
{
}
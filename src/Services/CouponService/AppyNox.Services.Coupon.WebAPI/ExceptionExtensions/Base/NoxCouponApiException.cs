using AppyNox.Services.Base.API.Exceptions.Base;
using AppyNox.Services.Base.Core.Common;
using AppyNox.Services.Base.Core.Enums;

namespace AppyNox.Services.Coupon.WebAPI.ExceptionExtensions.Base;

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
        NoxExceptionStrings.Base,
        message,
        exceptionCode,
        statusCode,
        innerException)
{
}
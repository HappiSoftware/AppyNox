using AppyNox.Services.Base.Core.Enums;
using AppyNox.Services.Base.Infrastructure.Exceptions.Base;
using AppyNox.Services.Coupon.Domain;

namespace AppyNox.Services.Coupon.Infrastructure.Exceptions.Base;

#region [ NoxCouponInfrastructureException Code ]

internal enum NoxCouponInfrastructureExceptionCode
{

}

#endregion

internal class NoxCouponInfrastructureException(
    string? message = null,
    int? exceptionCode = null,
    int? statusCode = null,
    Exception? innerException = null)
    : NoxInfrastructureExceptionBase(
        ExceptionProduct.AppyNox,
        NoxCouponCommonStrings.Service,
        message,
        exceptionCode,
        statusCode,
        innerException)
{
}
using AppyNox.Services.Base.Core.Enums;
using AppyNox.Services.Base.Domain.Exceptions.Base;

namespace AppyNox.Services.Coupon.Domain.Exceptions.Base;

#region [ NoxCouponDomainException Code ]

internal enum NoxCouponDomainExceptionCode
{
    NullPropertyException = 1000,

    AmountValidation = 1001,

    CouponBuilderValidation = 1002,
}

#endregion

internal class NoxCouponDomainException(string message, int exceptionCode)
    : NoxDomainExceptionBase(ExceptionProduct.AppyNox, NoxCouponCommonStrings.Service, message, exceptionCode)
{
}
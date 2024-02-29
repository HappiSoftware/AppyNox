using AppyNox.Services.Base.Domain.ExceptionExtensions.Base;

namespace AppyNox.Services.Coupon.Domain.Exceptions.Base;

#region [ NoxCouponDomainException Code]

internal enum NoxCouponDomainExceptionCode
{
    NullPropertyException = 1000,

    AmountValidation = 1001,

    CouponBuilderValidation = 1002,
}

#endregion

internal class NoxCouponDomainException : NoxDomainException
{
    #region [ Fields ]

    private const string _service = "Coupon";

    #endregion

    #region [ Internal Constructors ]

    internal NoxCouponDomainException(string message, int exceptionCode)
        : base(message, exceptionCode, _service)
    {
    }

    #endregion
}
using AppyNox.Services.Base.Domain.ExceptionExtensions.Base;

namespace AppyNox.Services.Coupon.Domain.ExceptionExtensions.Base;

#region [ NoxCouponDomainException Code]

internal enum NoxCouponDomainExceptionCode
{
    AmountValidation = 1000,
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
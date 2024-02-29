using AppyNox.Services.Coupon.Domain.Localization;
using DomainException = AppyNox.Services.Coupon.Domain.Exceptions.Base.NoxCouponDomainException;
using ExceptionCode = AppyNox.Services.Coupon.Domain.Exceptions.Base.NoxCouponDomainExceptionCode;

namespace AppyNox.Services.Coupon.Domain.Coupons.Builders;

public sealed class CouponDetailBuilder()
{
    #region [ Properties ]

    internal string? Detail { get; private set; }

    private string? _code;

    internal string Code
    {
        get
        {
            return _code ?? throw new DomainException(CouponDomainResourceService.CodeNotNull,
                                                              (int)ExceptionCode.NullPropertyException);
        }

        private set => _code = value;
    }

    #endregion

    #region [ Relations ]

    private ICollection<CouponDetailTag>? _couponDetailTags;

    internal ICollection<CouponDetailTag> CouponDetailTags
    {
        get => _couponDetailTags
            ?? throw new DomainException(CouponDomainResourceService.TagsNotNull,
                                         (int)ExceptionCode.NullPropertyException);

        private set => _couponDetailTags = value;
    }

    #endregion

    #region [ Partial Builders ]

    public CouponDetailBuilder WithDetails(string code, string? detail)
    {
        Code = code;
        Detail = detail;
        return this;
    }

    public CouponDetailBuilder WithTags(ICollection<CouponDetailTag> tags)
    {
        CouponDetailTags = tags;
        return this;
    }

    #endregion

    #region [ Builder ]

    public CouponDetail Build()
    {
        return new CouponDetail(this);
    }

    #endregion
}
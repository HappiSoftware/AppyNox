using AppyNox.Services.Coupon.Domain.Localization;
using DomainException = AppyNox.Services.Coupon.Domain.Exceptions.Base.NoxCouponDomainException;
using ExceptionCode = AppyNox.Services.Coupon.Domain.Exceptions.Base.NoxCouponDomainExceptionCode;

namespace AppyNox.Services.Coupon.Domain.Coupons.Builders;

public sealed class CouponDetailTagBuilder()
{
    #region [ Properties ]

    internal bool BulkCreate { get; private set; } = false;

    private string? _tag;

    internal string Tag
    {
        get
        {
            return _tag ?? throw new DomainException(CouponDomainResourceService.TagNotNull,
                                                              (int)ExceptionCode.NullPropertyException);
        }

        private set => _tag = value;
    }

    #endregion

    #region [ Relations ]

    private CouponDetailId? _couponDetailId;

    internal CouponDetailId CouponDetailId
    {
        get
        {
            return BulkCreate ?
                new CouponDetailId(Guid.Empty)
                : _couponDetailId ?? throw new DomainException(CouponDomainResourceService.CouponDetailIdNotNull,
                                                               (int)ExceptionCode.NullPropertyException);
        }

        private set => _couponDetailId = value;
    }

    #endregion

    #region [ Partial Builders ]

    public CouponDetailTagBuilder WithDetails(string tag)
    {
        Tag = tag;
        return this;
    }

    public CouponDetailTagBuilder WithCouponDetailId(CouponDetailId couponDetailId)
    {
        CouponDetailId = couponDetailId;
        return this;
    }

    public CouponDetailTagBuilder MarkAsBulkCreate()
    {
        BulkCreate = true;
        return this;
    }

    #endregion

    #region [ Builder ]

    public CouponDetailTag Build()
    {
        return new CouponDetailTag(this);
    }

    #endregion
}
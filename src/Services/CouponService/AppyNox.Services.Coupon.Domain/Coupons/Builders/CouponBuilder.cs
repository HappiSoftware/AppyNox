using AppyNox.Services.Coupon.Domain.Localization;
using DomainException = AppyNox.Services.Coupon.Domain.Exceptions.Base.NoxCouponDomainException;
using ExceptionCode = AppyNox.Services.Coupon.Domain.Exceptions.Base.NoxCouponDomainExceptionCode;

namespace AppyNox.Services.Coupon.Domain.Coupons.Builders;

public sealed class CouponBuilder()
{
    #region [ Properties ]

    internal bool BulkCreate { get; private set; } = false;

    internal string Description { get; private set; } = string.Empty;

    internal string? Detail { get; private set; }

    private Amount? _amount;

    internal Amount Amount
    {
        get
        {
            return _amount ?? throw new DomainException(CouponDomainResourceService.CouponDetailNotNull,
                                                              (int)ExceptionCode.NullPropertyException);
        }

        private set => _amount = value;
    }

    internal string Code { get; private set; } = string.Empty;

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

    private CouponDetail? _couponDetail;

    internal CouponDetail CouponDetail
    {
        get
        {
            return _couponDetail ?? throw new DomainException(CouponDomainResourceService.CouponDetailNotNull,
                                                              (int)ExceptionCode.NullPropertyException);
        }

        private set => _couponDetail = value;
    }

    #endregion

    #region [ Partial Builders ]

    public CouponBuilder WithAmount(Amount amount)
    {
        Amount = amount;
        return this;
    }

    public CouponBuilder WithAmount(int discountAmount, int minimumAmount)
    {
        Amount = new Amount(discountAmount, minimumAmount);
        return this;
    }

    public CouponBuilder WithDetails(string code, string description, string? detail)
    {
        Code = code;
        Description = description;
        Detail = detail;
        return this;
    }

    public CouponBuilder WithCouponDetail(CouponDetail couponDetail)
    {
        CouponDetail = couponDetail;
        return this;
    }

    public CouponBuilder WithCouponDetailId(CouponDetailId couponDetailId)
    {
        CouponDetailId = couponDetailId;
        return this;
    }

    public CouponBuilder MarkAsBulkCreate()
    {
        BulkCreate = true;
        return this;
    }

    #endregion

    #region [ Builder ]

    private void Validate()
    {
        if (BulkCreate && _couponDetail == null)
        {
            throw new DomainException(CouponDomainResourceService.CouponDetailInComposeNotNull,
                                      (int)ExceptionCode.CouponBuilderValidation);
        }
        if (BulkCreate && (_couponDetailId != null && !_couponDetailId.Value.Equals(Guid.Empty)))
        {
            throw new DomainException(CouponDomainResourceService.CouponDetailIdShouldBeEmptyInCompose,
                                      (int)ExceptionCode.CouponBuilderValidation);
        }
        if (!BulkCreate && (_couponDetailId == null || _couponDetailId.Value.Equals(Guid.Empty)))
        {
            throw new DomainException(CouponDomainResourceService.CouponDetailIdMandatory,
                                      (int)ExceptionCode.CouponBuilderValidation);
        }
        if (!BulkCreate && _couponDetail != null)
        {
            throw new DomainException(CouponDomainResourceService.CouponDetailShouldBeEmptyInCompose,
                                      (int)ExceptionCode.CouponBuilderValidation);
        }
    }

    public Coupon Build()
    {
        Validate();
        return new Coupon(this);
    }

    #endregion
}
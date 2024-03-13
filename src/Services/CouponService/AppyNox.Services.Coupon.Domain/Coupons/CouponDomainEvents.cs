using AppyNox.Services.Base.Domain;

namespace AppyNox.Services.Coupon.Domain.Coupons;

public sealed record CouponUpdatedDomainEvent(CouponId CouponId, int MinimumAmount) : IDomainEvent;
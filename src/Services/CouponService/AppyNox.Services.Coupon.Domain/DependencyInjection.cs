using AppyNox.Services.Coupon.Domain.Localization;
using Microsoft.Extensions.Localization;

namespace AppyNox.Services.Coupon.Domain;

public static class DependencyInjection
{
    #region [ Public Methods ]

    public static void AddCouponDomainLocalizationService(this IStringLocalizerFactory localizerFactory)
    {
        CouponDomainResourceService.Initialize(localizerFactory);
    }

    #endregion
}
using Microsoft.Extensions.Localization;

namespace AppyNox.Services.Coupon.Domain.Localization
{
    public static class CouponDomainResourceService
    {
        #region [ Fields ]

        private static IStringLocalizer? _localizer;

        #endregion

        #region [ Exception Resources ]

        /// <summary>
        /// CouponDetailId must be set before it can be accessed.
        /// </summary>
        internal static LocalizedString CouponDetailIdNotNull => GetMessage("CouponDetailIdNotNull");

        /// <summary>
        /// CouponDetail must be set before it can be accessed.
        /// </summary>
        internal static LocalizedString CouponDetailNotNull => GetMessage("CouponDetailNotNull");

        /// <summary>
        /// Code must be set before it can be accessed.
        /// </summary>
        internal static LocalizedString CodeNotNull => GetMessage("CodeNotNull");

        /// <summary>
        /// CouponDetailTags must be set before it can be accessed.
        /// </summary>
        internal static LocalizedString TagsNotNull => GetMessage("TagsNotNull");

        /// <summary>
        /// Tag must be set before it can be accessed.
        /// </summary>
        internal static LocalizedString TagNotNull => GetMessage("TagNotNull");

        /// <summary>
        /// Amount must be set before it can be accessed.
        /// </summary>
        internal static LocalizedString AmountNotNull => GetMessage("AmountNotNull");

        /// <summary>
        /// CouponDetail can not be null in composite creation.
        /// </summary>
        internal static LocalizedString CouponDetailInComposeNotNull => GetMessage("CouponDetailInComposeNotNull");

        /// <summary>
        /// CouponDetailId is mandatory.
        /// </summary>
        internal static LocalizedString CouponDetailIdMandatory => GetMessage("CouponDetailIdMandatory");

        /// <summary>
        /// CouponDetailId should not be provided in composite creation.
        /// </summary>
        internal static LocalizedString CouponDetailIdShouldBeEmptyInCompose => GetMessage("CouponDetailIdShouldBeEmptyInCompose");

        /// <summary>
        /// CouponDetail should not be provided in composite creation.
        /// </summary>
        internal static LocalizedString CouponDetailShouldBeEmptyInCompose => GetMessage("CouponDetailShouldBeEmptyInCompose");

        #endregion

        #region [ Private Methods ]

        private static LocalizedString GetMessage(string key)
        {
            if (_localizer is null)
            {
                throw new InvalidOperationException("Nox Coupon Domain Resource service is not initialized.");
            }

            return _localizer[key];
        }

        #endregion

        #region [ Public Methods ]

        public static void Initialize(IStringLocalizerFactory factory)
        {
            _localizer = factory.Create(typeof(CouponDomainResourceService));
        }

        #endregion
    }
}
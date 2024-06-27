using Microsoft.Extensions.Localization;

namespace AppyNox.Services.Sso.Application.Localization
{
    public static class NoxSsoApplicationResourceService
    {
        #region [ Fields ]

        private static IStringLocalizer? _localizer;

        #endregion

        #region [ Exception Resources ]

        /// <summary>
        /// Dummy.
        /// </summary>
        internal static LocalizedString Dummy => GetMessage("Dummy");

        /// <summary>
        /// Authentication Failed.
        /// </summary>
        internal static LocalizedString AuthenticationFailed => GetMessage("AuthenticationFailed");


        #endregion

        #region [ Private Methods ]

        private static LocalizedString GetMessage(string key)
        {
            if (_localizer is null)
            {
                throw new InvalidOperationException("Nox Sso Application Resource service is not initialized.");
            }

            return _localizer[key];
        }

        #endregion

        #region [ Public Methods ]

        public static void Initialize(IStringLocalizerFactory factory)
        {
            _localizer = factory.Create(typeof(NoxSsoApplicationResourceService));
        }

        #endregion
    }
}
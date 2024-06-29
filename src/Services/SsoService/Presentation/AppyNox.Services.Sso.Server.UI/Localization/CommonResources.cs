using Microsoft.Extensions.Localization;

namespace AppyNox.Services.Sso.Server.UI.Localization
{
    public static class CommonResources
    {
        #region [ Fields ]

        private static IStringLocalizer? _localizer;

        #endregion

        #region [ Resources ]

        /// <summary>
        /// Dummy.
        /// </summary>
        internal static LocalizedString Dummy => GetMessage("Dummy");


        #endregion

        #region [ Private Methods ]

        private static LocalizedString GetMessage(string key)
        {
            if (_localizer is null)
            {
                throw new InvalidOperationException("Nox Sso Server UI Common Resource service is not initialized.");
            }

            return _localizer[key];
        }

        #endregion

        #region [ Public Methods ]

        public static void Initialize(IStringLocalizerFactory factory)
        {
            _localizer = factory.Create(typeof(CommonResources));
        }

        #endregion
    }
}
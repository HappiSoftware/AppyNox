using Microsoft.Extensions.Localization;

namespace AppyNox.Services.Authentication.WebAPI.Localization
{
    internal static class NoxSsoApiResourceService
    {
        #region [ Fields ]

        private static IStringLocalizer? _localizer;

        #endregion

        #region [ Exception Resources ]

        /// <summary>
        /// Invalid or unsupported audience.
        /// </summary>
        internal static LocalizedString InvalidAudience => GetMessage("InvalidAudience");

        /// <summary>
        /// Invalid SignIn attempt.
        /// </summary>
        internal static LocalizedString SignInError => GetMessage("SignInError");

        /// <summary>
        /// Role Not Found.
        /// </summary>
        internal static LocalizedString RoleNotFound => GetMessage("RoleNotFound");

        /// <summary>
        /// Record with id: {0} does not exist.
        /// <para>{0}: id</para>
        /// </summary>
        internal static LocalizedString RecordNotFound => GetMessage("RecordNotFound");

        /// <summary>
        /// Role with id {0} already has this claim.
        /// <para>{0}: rid</para>
        /// </summary>
        internal static LocalizedString AlreadyHasClaim => GetMessage("AlreadyHasClaim");

        /// <summary>
        /// User Not Found.
        /// </summary>
        internal static LocalizedString UserNotFound => GetMessage("UserNotFound");

        #endregion

        #region [ Common Resources ]

        /// <summary>
        /// SignIn Successful.
        /// </summary>
        internal static LocalizedString SignInSuccessful => GetMessage("SignInSuccessful");

        #endregion

        #region [ Private Methods ]

        private static LocalizedString GetMessage(string key)
        {
            if (_localizer is null)
            {
                throw new InvalidOperationException("Nox Sso Api Resource service is not initialized.");
            }

            return _localizer[key];
        }

        #endregion

        #region [ Public Methods ]

        public static void Initialize(IStringLocalizerFactory factory)
        {
            _localizer = factory.Create(typeof(NoxSsoApiResourceService));
        }

        #endregion
    }
}
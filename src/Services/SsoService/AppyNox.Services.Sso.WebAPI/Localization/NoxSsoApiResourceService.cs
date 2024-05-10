using Microsoft.Extensions.Localization;

namespace AppyNox.Services.Sso.WebAPI.Localization
{
    internal static class NoxSsoApiResourceService
    {
        #region [ Fields ]

        private static IStringLocalizer? _localizer;

        #endregion

        #region [ Exception Resources ]

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

        /// <summary>
        /// Refresh token could not be verified.
        /// </summary>
        internal static LocalizedString RefreshTokenInvalid => GetMessage("RefreshTokenInvalid");

        #endregion

        #region [ Common Resources ]

        /// <summary>
        /// SignIn Successful.
        /// </summary>
        internal static LocalizedString SignInSuccessful => GetMessage("SignInSuccessful");

        /// <summary>
        /// I am a teapot.
        /// </summary>
        internal static LocalizedString Teapot => GetMessage("Teapot");

        /// <summary>
        /// Ids don't match
        /// </summary>
        internal static LocalizedString IdMismatch => GetMessage("IdMismatch");

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
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

        /// <summary>
        /// I am a teapot.
        /// </summary>
        internal static LocalizedString Teapot => GetMessage("Teapot");

        /// <summary>
        /// Ids don't match
        /// </summary>
        internal static LocalizedString IdMismatch => GetMessage("IdMismatch");

        /// <summary>
        /// No refresh token found. Please re-login to get a new one.
        /// </summary>
        internal static LocalizedString RefreshTokenNotFound => GetMessage("RefreshTokenNotFound");

        /// <summary>
        /// Wrong Credentials.
        /// </summary>
        internal static LocalizedString WrongCredentials => GetMessage("WrongCredentials");

        /// <summary>
        /// Failed to save the refresh token. Please try again.
        /// </summary>
        internal static LocalizedString RefreshTokenError => GetMessage("RefreshTokenError");

        /// <summary>
        /// Token has expired.
        /// </summary>
        internal static LocalizedString ExpiredToken => GetMessage("ExpiredToken");

        /// <summary>
        /// Received JWT is invalid.
        /// </summary>
        internal static LocalizedString InvalidToken => GetMessage("InvalidToken");

        /// <summary>
        /// You have no claims to take this action.
        /// </summary>
        internal static LocalizedString UnauthorizedAccess => GetMessage("UnauthorizedAccess");

        /// <summary>
        /// JWT is null.
        /// </summary>
        internal static LocalizedString NullToken => GetMessage("NullToken");

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
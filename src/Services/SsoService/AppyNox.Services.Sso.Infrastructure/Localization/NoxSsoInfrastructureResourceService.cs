using Microsoft.Extensions.Localization;

namespace AppyNox.Services.Sso.Infrastructure.Localization
{
    public static class NoxSsoInfrastructureResourceService
    {
        #region [ Fields ]

        private static IStringLocalizer? _localizer;

        #endregion

        #region [ Exception Resources ]

        /// <summary>
        /// CorrelationId can not be null in UserCreationSaga in Initialization.
        /// </summary>
        internal static LocalizedString UserCreationSagaCorrelationIdError => GetMessage("UserCreationSagaCorrelationIdError");

        /// <summary>
        /// LicenseId can not be null in LicenseValidatedEvent if IsValid is true.
        /// </summary>
        internal static LocalizedString UserCreationSagaLicenseIdError => GetMessage("UserCreationSagaLicenseIdError");

        /// <summary>
        /// CompanyId can not be null for CreateApplicationUserMessage.
        /// </summary>
        internal static LocalizedString UserCreationSagaCompanyIdError => GetMessage("UserCreationSagaCompanyIdError");

        /// <summary>
        /// You have no claims to take this action.
        /// </summary>
        internal static LocalizedString UnauthorizedAccess => GetMessage("UnauthorizedAccess");

        /// <summary>
        /// JWT is null.
        /// </summary>
        internal static LocalizedString NullToken => GetMessage("NullToken");

        /// <summary>
        /// Received JWT is invalid.
        /// </summary>
        internal static LocalizedString InvalidToken => GetMessage("InvalidToken");

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
        /// Invalid or unsupported audience.
        /// </summary>
        internal static LocalizedString InvalidAudience => GetMessage("InvalidAudience");

        /// <summary>
        /// I am a teapot.
        /// </summary>
        internal static LocalizedString Teapot => GetMessage("Teapot");

        #endregion

        #region [ Private Methods ]

        private static LocalizedString GetMessage(string key)
        {
            if (_localizer is null)
            {
                throw new InvalidOperationException("Nox Sso Infrastructure Resource service is not initialized.");
            }

            return _localizer[key];
        }

        #endregion

        #region [ Public Methods ]

        public static void Initialize(IStringLocalizerFactory factory)
        {
            _localizer = factory.Create(typeof(NoxSsoInfrastructureResourceService));
        }

        #endregion
    }
}
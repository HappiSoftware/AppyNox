using Microsoft.Extensions.Localization;

namespace AppyNox.Services.Base.API.Localization
{
    public static class NoxApiResourceService
    {
        #region [ Fields ]

        private static IStringLocalizer? _localizer;

        #endregion

        #region [ Exception Resources ]

        /// <summary>
        /// Correlation ID is required.
        /// </summary>
        internal static LocalizedString CorrelationIdIsRequired => GetMessage("CorrelationIdIsRequired");

        /// <summary>
        /// A request with no correlation ID received.
        /// </summary>
        internal static LocalizedString MissingCorrelationIdMessage => GetMessage("MissingCorrelationIdMessage");

        /// <summary>
        /// Nox Exception is thrown.
        /// </summary>
        internal static LocalizedString NoxExceptionThrown => GetMessage("NoxExceptionThrown");

        /// <summary>
        /// An unexpected error occurred.
        /// </summary>
        internal static LocalizedString UnknownExceptionThrown => GetMessage("UnknownExceptionThrown");

        /// <summary>
        /// {0} is not a valid Access modifier.
        /// </summary>
        internal static LocalizedString InvalidAccessModifier => GetMessage("InvalidAccessModifier");

        #endregion

        #region [ Common Resources ]

        /// <summary>
        /// {0} Request Successful.
        /// </summary>
        internal static LocalizedString RequestSuccessful => GetMessage("RequestSuccessful");

        #endregion

        #region [ Shared Resources ]

        /// <summary>
        /// Unexpected Error Occurred.
        /// </summary>
        public static LocalizedString UnexpectedError => GetMessage("UnexpectedError");

        /// <summary>
        /// Received JWT is invalid.
        /// </summary>
        public static LocalizedString InvalidToken => GetMessage("InvalidToken");

        /// <summary>
        /// JWT is null.
        /// </summary>
        public static LocalizedString NullToken => GetMessage("NullToken");

        /// <summary>
        /// Unauthorized access. Please SignIn first.
        /// </summary>
        public static LocalizedString UnauthenticatedAccess => GetMessage("UnauthenticatedAccess");

        /// <summary>
        /// You have no claims to take this action.
        /// </summary>
        public static LocalizedString UnauthorizedAccess => GetMessage("UnauthorizedAccess");

        /// <summary>
        /// Token has expired.
        /// </summary>
        public static LocalizedString ExpiredToken => GetMessage("ExpiredToken");

        /// <summary>
        /// Wrong Credentials.
        /// </summary>
        public static LocalizedString WrongCredentials => GetMessage("WrongCredentials");

        /// <summary>
        /// Failed to save the refresh token. Please try again.
        /// </summary>
        public static LocalizedString RefreshTokenError => GetMessage("RefreshTokenError");

        /// <summary>
        /// No refresh token found. Please re-login to get a new one.
        /// </summary>
        public static LocalizedString RefreshTokenNotFound => GetMessage("RefreshTokenNotFound");

        /// <summary>
        /// I am a teapot.
        /// </summary>
        public static LocalizedString Teapot => GetMessage("Teapot");

        /// <summary>
        /// Ids don't match
        /// </summary>
        public static LocalizedString IdMismatch => GetMessage("IdMismatch");

        #endregion

        #region [ Public Methods ]

        public static void Initialize(IStringLocalizerFactory factory)
        {
            _localizer = factory.Create(typeof(NoxApiResourceService));
        }

        #endregion

        #region [ Private Methods ]

        private static LocalizedString GetMessage(string key)
        {
            if (_localizer is null)
            {
                throw new InvalidOperationException("Nox Api Resource service is not initialized.");
            }

            return _localizer[key];
        }

        #endregion
    }
}
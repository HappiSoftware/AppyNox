using Microsoft.Extensions.Localization;

namespace AppyNox.Services.Base.API.Localization
{
    internal static class NoxApiResourceService
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

        /// <summary>
        /// {0} Request Unsuccessful.
        /// </summary>
        internal static LocalizedString RequestUnsuccessful => GetMessage("RequestUnsuccessful");

        /// <summary>
        /// The request is invalid.
        /// </summary>
        internal static LocalizedString BadRequestWrapper => GetMessage("BadRequestWrapper");

        /// <summary>
        /// The request is denied due to unauthorized access.
        /// </summary>
        internal static LocalizedString UnauthorizedWrapper => GetMessage("UnauthorizedWrapper");

        /// <summary>
        /// The requested URI does not exist.
        /// </summary>
        internal static LocalizedString NotFoundWrapper => GetMessage("NotFoundWrapper");

        /// <summary>
        /// The request method is not allowed.
        /// </summary>
        internal static LocalizedString MethodNotAllowedWrapper => GetMessage("MethodNotAllowedWrapper");

        /// <summary>
        /// The media type of the request is unsupported.
        /// </summary>
        internal static LocalizedString UnsupportedMediaTypeWrapper => GetMessage("UnsupportedMediaTypeWrapper");

        /// <summary>
        /// An unknown error occurred that is not mapped in the NoxWrapper.
        /// </summary>
        internal static LocalizedString UnknownErrorWrapper => GetMessage("UnknownErrorWrapper");

        /// <summary>
        /// Unexpected Error Occurred.
        /// </summary>
        internal static LocalizedString UnexpectedError => GetMessage("UnexpectedError");

        /// <summary>
        /// Received JWT is invalid.
        /// </summary>
        internal static LocalizedString InvalidToken => GetMessage("InvalidToken");

        /// <summary>
        /// JWT is null.
        /// </summary>
        internal static LocalizedString NullToken => GetMessage("NullToken");

        /// <summary>
        /// Unauthorized access. Please SignIn first.
        /// </summary>
        internal static LocalizedString UnauthenticatedAccess => GetMessage("UnauthenticatedAccess");

        /// <summary>
        /// You have no claims to take this action.
        /// </summary>
        internal static LocalizedString UnauthorizedAccess => GetMessage("UnauthorizedAccess");

        /// <summary>
        /// Token has expired.
        /// </summary>
        internal static LocalizedString ExpiredToken => GetMessage("ExpiredToken");

        /// <summary>
        /// Wrong Credentials.
        /// </summary>
        internal static LocalizedString WrongCredentials => GetMessage("WrongCredentials");

        /// <summary>
        /// Failed to save the refresh token. Please try again.
        /// </summary>
        internal static LocalizedString RefreshTokenError => GetMessage("RefreshTokenError");

        /// <summary>
        /// No refresh token found. Please re-login to get a new one.
        /// </summary>
        internal static LocalizedString RefreshTokenNotFound => GetMessage("RefreshTokenNotFound");

        /// <summary>
        /// I am a teapot.
        /// </summary>
        internal static LocalizedString Teapot => GetMessage("Teapot");

        /// <summary>
        /// Ids don't match
        /// </summary>
        internal static LocalizedString IdMismatch => GetMessage("IdMismatch");

        #endregion

        #region [ Internal Methods ]

        internal static void Initialize(IStringLocalizerFactory factory)
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
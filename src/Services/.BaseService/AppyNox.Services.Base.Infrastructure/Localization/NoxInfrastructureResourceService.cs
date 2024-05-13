using Microsoft.Extensions.Localization;

namespace AppyNox.Services.Base.Infrastructure.Localization
{
    internal static class NoxInfrastructureResourceService
    {
        #region [ Fields ]

        private static IStringLocalizer? _localizer;

        #endregion

        #region [ Exception Resources ]

        /// <summary>
        /// An error occurred while saving changes to the database.
        /// </summary>
        internal static LocalizedString CommitException => GetMessage("CommitException");

        /// <summary>
        /// Entity of type {typeof(TEntity).Name} with ID {entityId} was not found.
        /// <para>{0}:typeof(TEntity).Name </para>
        /// <para>{1}: entityId</para>
        /// </summary>
        internal static LocalizedString EntityNotFound => GetMessage("EntityNotFound");

        /// <summary>
        /// JWT is null.
        /// </summary>
        internal static LocalizedString NullToken => GetMessage("NullToken");

        /// <summary>
        /// Received JWT is invalid.
        /// </summary>
        internal static LocalizedString InvalidToken => GetMessage("InvalidToken");


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

        #region [ Common Resources ]

        /// <summary>
        /// Unexpected Error Occurred.
        /// </summary>
        internal static LocalizedString UnexpectedError => GetMessage("UnexpectedError");

        #endregion

        #region [ Private Methods ]

        private static LocalizedString GetMessage(string key)
        {
            if (_localizer is null)
            {
                throw new InvalidOperationException("Nox Infrastructure Resource service is not initialized.");
            }

            return _localizer[key];
        }

        #endregion

        #region [ Internal Methods ]

        internal static void Initialize(IStringLocalizerFactory factory)
        {
            _localizer = factory.Create(typeof(NoxInfrastructureResourceService));
        }

        #endregion
    }
}
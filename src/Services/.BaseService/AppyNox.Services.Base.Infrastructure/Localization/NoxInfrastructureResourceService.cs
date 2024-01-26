using Microsoft.Extensions.Localization;

namespace AppyNox.Services.Base.Infrastructure.Localization
{
    public static class NoxInfrastructureResourceService
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

        #endregion

        #region [ Common Resources ]

        /// <summary>
        /// Unexpected Error Occurred.
        /// </summary>
        public static LocalizedString UnexpectedError => GetMessage("UnexpectedError");

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

        #region [ Public Methods ]

        public static void Initialize(IStringLocalizerFactory factory)
        {
            _localizer = factory.Create(typeof(NoxInfrastructureResourceService));
        }

        #endregion
    }
}
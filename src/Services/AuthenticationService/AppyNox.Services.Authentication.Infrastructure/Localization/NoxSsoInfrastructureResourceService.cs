using Microsoft.Extensions.Localization;

namespace AppyNox.Services.Authentication.Infrastructure.Localization
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
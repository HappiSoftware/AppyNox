using Microsoft.Extensions.Localization;

namespace AppyNox.Services.Base.Application.Localization
{
    public static class NoxApplicationResourceService
    {
        #region [ Fields ]

        private static IStringLocalizer? _localizer;

        #endregion

        #region [ Exception Resources ]

        /// <summary>
        /// This '{0}' entity has no access level mapping.
        /// <para>{0}: entity.FullName</para>
        /// </summary>
        internal static LocalizedString EntityHasNoAccessLevel => GetMessage("EntityHasNoAccessLevel");

        /// <summary>
        /// This '{0}' entity has no access level mapping for '{1}'.
        /// <para>{0}: entity.FullName</para>
        /// <para>{1}: accessType</para>
        /// </summary>
        internal static LocalizedString EntityHasNoAccessLevelForType => GetMessage("EntityHasNoAccessLevelForType");

        /// <summary>
        /// No enum value found for displayName '{0}' in '{1}'
        /// <para>{0}: displayName</para>
        /// <para>{1}: enumType</para>
        /// </summary>
        internal static LocalizedString EnumValueNotFoundForDisplay => GetMessage("EnumValueNotFoundForDisplay");

        /// <summary>
        /// This '{0}' level is not found in dto-entity mapping for '{1}'.
        /// <para>{0}: enumValue</para>
        /// <para>{1}: entity.FullName</para>
        /// </summary>
        internal static LocalizedString LevelNotFoundForEntity => GetMessage("LevelNotFoundForEntity");

        /// <summary>
        /// CommonDtoLevelEnums not found for: {0}.
        /// <para>{0}: enumVal</para>
        /// </summary>
        internal static LocalizedString CommonDtoLevelNotFound => GetMessage("CommonDtoLevelNotFound");

        /// <summary>
        /// Request responded with one or more validation errors for '{0}'
        /// <para>{0}: dtoType</para>
        /// </summary>
        internal static LocalizedString FluentValidationFailed => GetMessage("FluentValidationFailed");

        /// <summary>
        /// No validator found for '{0}'.
        /// <para>{0}: dtoType</para>
        /// </summary>
        internal static LocalizedString ValidatorNotFound => GetMessage("ValidatorNotFound");

        /// <summary>
        /// Unsupported attribute type for detail level mapping. Check DtoMappingRegistry.
        /// </summary>
        internal static LocalizedString UnsupportedLevelAttribute => GetMessage("UnsupportedLevelAttribute");

        #endregion

        #region [ Shared Resources ]

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
                throw new InvalidOperationException("Nox Application Resource service is not initialized.");
            }

            return _localizer[key];
        }

        #endregion

        #region [ Public Methods ]

        public static void Initialize(IStringLocalizerFactory factory)
        {
            _localizer = factory.Create(typeof(NoxApplicationResourceService));
        }

        #endregion
    }
}
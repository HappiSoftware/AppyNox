using AppyNox.Services.Base.Application.ExceptionExtensions.Base;
using AppyNox.Services.Base.Application.Localization;
using AppyNox.Services.Base.Core.Enums;
using AppyNox.Services.Base.Core.Extensions;
using System.Net;

namespace AppyNox.Services.Base.Application.ExceptionExtensions
{
    /// <summary>
    /// DtoDetailLevelNotFoundException, thrown in Application Base.
    /// This exception means DetailLevel value of the request
    /// is not meet any available Dto Level in the system.
    /// </summary>
    internal class DtoDetailLevelNotFoundException : NoxApplicationException
    {
        #region [ Internal Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="DtoDetailLevelNotFoundException"/> class when no enum value is found for a given display name.
        /// Http status code is set to 400.
        /// </summary>
        /// <param name="displayName">The display name for which no enum value is found.</param>
        /// <param name="enumType">The enum type being checked.</param>
        internal DtoDetailLevelNotFoundException(string displayName, Type enumType)
            : base(message: NoxApplicationResourceService.EnumValueNotFoundForDisplay.Format(displayName, enumType),
                  exceptionCode: (int)NoxApplicationExceptionCode.DtoDetailLevelNotFoundForDisplay,
                  statusCode: (int)HttpStatusCode.BadRequest)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DtoDetailLevelNotFoundException"/> class when a specific detail level is not found in dto-entity mapping.
        /// Http status code is set to 500.
        /// </summary>
        /// <param name="entity">The entity type.</param>
        /// <param name="enumValue">The enum value for the detail level.</param>
        internal DtoDetailLevelNotFoundException(Type entity, Enum enumValue)
            : base(message: NoxApplicationResourceService.LevelNotFoundForEntity.Format(entity.FullName ?? entity.Name, enumValue),
                  exceptionCode: (int)NoxApplicationExceptionCode.DtoDetailLevelNotFoundForDisplay)
        {
        }

        #endregion
    }
}
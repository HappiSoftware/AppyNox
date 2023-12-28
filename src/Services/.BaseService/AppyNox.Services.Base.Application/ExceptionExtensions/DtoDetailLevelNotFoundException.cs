using AppyNox.Services.Base.Application.ExceptionExtensions.Base;
using AppyNox.Services.Base.Domain.Common;
using AppyNox.Services.Base.Domain.Common.HttpStatusCodes;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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

        internal DtoDetailLevelNotFoundException(string displayName, Type enumType)
            : base($"No enum value found for displayName '{displayName}' in '{enumType}'", (int)NoxClientErrorResponseCodes.BadRequest)
        {
        }

        internal DtoDetailLevelNotFoundException(Type entity, Enum enumValue)
            : base($"This '{enumValue}' level is not found in dto-entity mapping for '{entity.FullName}'.", (int)NoxServerErrorResponseCodes.InternalServerError)
        {
        }

        internal DtoDetailLevelNotFoundException(CommonDtoLevelEnums enumVal)
            : base($"CommonDtoLevelEnums not found for: {enumVal}", (int)NoxServerErrorResponseCodes.InternalServerError)
        {
        }

        #endregion
    }
}
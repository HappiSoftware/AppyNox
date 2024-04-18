using AppyNox.Services.Base.Application.Exceptions.Base;
using AppyNox.Services.Base.Application.Localization;
using AppyNox.Services.Base.Core.Common;
using AppyNox.Services.Base.Core.Enums;
using AppyNox.Services.Base.Core.Extensions;
using System.Net;

namespace AppyNox.Services.Base.Application.Exceptions;

/// <summary>
/// Exception thrown when an access type mapping for an entity is not found.
/// </summary>
internal class AccessTypeNotFoundException : NoxApplicationException
{
    #region [ Internal Constructors ]

    /// <summary>
    /// Initializes a new instance of the <see cref="AccessTypeNotFoundException"/> class for a specific entity type.
    /// Http status code is set to 500.
    /// </summary>
    /// <param name="entity">The entity type for which the access level mapping is not found.</param>
    internal AccessTypeNotFoundException(Type entity)
        : base(
            message: NoxApplicationResourceService.EntityHasNoAccessLevel.Format(entity.FullName ?? entity.Name),
            exceptionCode: (int)NoxApplicationExceptionCode.AccessTypeEmpty)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AccessTypeNotFoundException"/> class for a specific entity type and access type.
    /// Http status code is set to 400.
    /// </summary>
    /// <param name="entity">The entity type for which the access level mapping is not found.</param>
    /// <param name="accessType">The specific access type that is not found.</param>
    internal AccessTypeNotFoundException(Type entity, string accessType)
        : base(
            message: NoxApplicationResourceService.EntityHasNoAccessLevelForType.Format(entity.FullName ?? entity.Name, accessType),
            exceptionCode: (int)NoxApplicationExceptionCode.AccessTypeError,
            statusCode: (int)HttpStatusCode.BadRequest)
    {
    }

    #endregion
}
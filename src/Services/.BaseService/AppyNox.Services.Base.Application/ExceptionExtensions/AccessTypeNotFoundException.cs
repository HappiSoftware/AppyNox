using AppyNox.Services.Base.Application.ExceptionExtensions.Base;
using System.Net;

namespace AppyNox.Services.Base.Application.ExceptionExtensions
{
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
            : base($"This '{entity.FullName}' entity has no access level mapping.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccessTypeNotFoundException"/> class for a specific entity type and access type.
        /// Http status code is set to 400.
        /// </summary>
        /// <param name="entity">The entity type for which the access level mapping is not found.</param>
        /// <param name="accessType">The specific access type that is not found.</param>
        internal AccessTypeNotFoundException(Type entity, string accessType)
            : base($"This '{entity.FullName}' entity has no access level mapping for '{accessType}'.", (int)HttpStatusCode.BadRequest)
        {
        }

        #endregion
    }
}
using AppyNox.Services.Base.Domain.Common.HttpStatusCodes;
using AppyNox.Services.Base.Infrastructure.ExceptionExtensions.Base;

namespace AppyNox.Services.Base.Infrastructure.ExceptionExtensions
{
    public class EntityNotFoundException<TEntity>(Guid entityId)
        : NoxInfrastructureException($"Entity of type {typeof(TEntity).Name} with ID {entityId} was not found.", (int)NoxClientErrorResponseCodes.NotFound)
    {
        #region [ Properties ]

        protected Guid EntityId { get; } = entityId;

        #endregion
    }
}
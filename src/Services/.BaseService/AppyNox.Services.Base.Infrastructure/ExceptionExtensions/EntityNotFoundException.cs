using AppyNox.Services.Base.Infrastructure.ExceptionExtensions.Base;
using System.Net;

namespace AppyNox.Services.Base.Infrastructure.ExceptionExtensions
{
    public class EntityNotFoundException<TEntity>(Guid entityId)
        : NoxInfrastructureException($"Entity of type {typeof(TEntity).Name} with ID {entityId} was not found.", (int)HttpStatusCode.NotFound)
    {
    }
}
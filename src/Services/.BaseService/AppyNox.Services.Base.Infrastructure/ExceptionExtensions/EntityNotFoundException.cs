using AppyNox.Services.Base.Core.Extensions;
using AppyNox.Services.Base.Infrastructure.ExceptionExtensions.Base;
using AppyNox.Services.Base.Infrastructure.Localization;
using System.Net;

namespace AppyNox.Services.Base.Infrastructure.ExceptionExtensions
{
    public class EntityNotFoundException<TEntity>(Guid entityId)
        : NoxInfrastructureException(NoxInfrastructureResourceService.EntityNotFound.Format(typeof(TEntity).Name, entityId), (int)HttpStatusCode.NotFound)
    {
    }
}
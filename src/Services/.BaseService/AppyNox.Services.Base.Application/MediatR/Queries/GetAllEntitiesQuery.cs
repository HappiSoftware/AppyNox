using AppyNox.Services.Base.Application.Interfaces.Repositories;
using AppyNox.Services.Base.Domain.Interfaces;
using MediatR;

namespace AppyNox.Services.Base.Application.MediatR.Queries
{
    public class GetAllEntitiesQuery<TEntity>(IQueryParameters queryParameters) : IRequest<IEnumerable<object>>
        where TEntity : class, IEntityWithGuid
    {
        #region [ Properties ]

        public IQueryParameters QueryParameters { get; } = queryParameters;

        #endregion
    }
}
using AppyNox.Services.Base.Domain.Interfaces;
using AppyNox.Services.Base.Infrastructure.Repositories.Common;
using MediatR;

namespace AppyNox.Services.Base.Application.MediatR.Queries
{
    public class GetAllEntitiesQuery<TEntity>(QueryParametersBase queryParameters) : IRequest<IEnumerable<object>>
        where TEntity : class, IEntityWithGuid
    {
        #region [ Properties ]

        public QueryParametersBase QueryParameters { get; } = queryParameters;

        #endregion
    }
}
using AppyNox.Services.Base.Domain.Interfaces;
using AppyNox.Services.Base.Infrastructure.Repositories.Common;
using MediatR;

namespace AppyNox.Services.Base.Application.MediatR.Queries
{
    public class GetEntityByIdQuery<TEntity>(Guid id, QueryParametersBase queryParameters) : IRequest<object>
        where TEntity : class, IEntityWithGuid
    {
        #region [ Properties ]

        public QueryParametersBase QueryParameters { get; } = queryParameters;

        public Guid Id { get; set; } = id;

        #endregion
    }
}
using AppyNox.Services.Base.Application.Interfaces.Repositories;
using AppyNox.Services.Base.Domain.Interfaces;
using MediatR;

namespace AppyNox.Services.Base.Application.MediatR.Queries
{
    public class GetEntityByIdQuery<TEntity>(Guid id, IQueryParameters queryParameters) : IRequest<object>
        where TEntity : class, IEntityWithGuid
    {
        #region [ Properties ]

        public IQueryParameters QueryParameters { get; } = queryParameters;

        public Guid Id { get; set; } = id;

        #endregion
    }
}
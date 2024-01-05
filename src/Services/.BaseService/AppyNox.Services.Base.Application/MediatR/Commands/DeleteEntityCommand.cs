using AppyNox.Services.Base.Domain.Interfaces;
using MediatR;

namespace AppyNox.Services.Base.Application.MediatR.Commands
{
    public class DeleteEntityCommand<TEntity>(Guid id) : IRequest
        where TEntity : class, IEntityWithGuid
    {
        #region [ Properties ]

        public Guid Id { get; set; } = id;

        #endregion
    }
}
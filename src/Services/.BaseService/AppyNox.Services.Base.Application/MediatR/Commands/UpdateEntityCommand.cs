using AppyNox.Services.Base.Domain.Interfaces;
using MediatR;

namespace AppyNox.Services.Base.Application.MediatR.Commands
{
    public class UpdateEntityCommand<TEntity>(Guid id, dynamic dto, string detailLevel) : IRequest
        where TEntity : class, IEntityWithGuid
    {
        #region [ Properties ]

        public Guid Id { get; set; } = id;

        public dynamic Dto { get; set; } = dto;

        public string DetailLevel { get; set; } = detailLevel;

        #endregion
    }
}
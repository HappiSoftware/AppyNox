using AppyNox.Services.Base.Domain.Interfaces;
using MediatR;

namespace AppyNox.Services.Base.Application.MediatR.Commands
{
    public class CreateEntityCommand<TEntity>(dynamic dto, string detailLevel) : IRequest<(Guid guid, object basicDto)>
        where TEntity : class, IEntityWithGuid
    {
        #region [ Properties ]

        public dynamic Dto { get; set; } = dto;

        public string DetailLevel { get; set; } = detailLevel;

        #endregion
    }
}
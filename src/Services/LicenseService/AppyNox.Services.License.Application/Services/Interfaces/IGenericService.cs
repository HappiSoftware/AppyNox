using AppyNox.Services.Base.Application.Services.Interfaces;
using AppyNox.Services.Base.Domain.Interfaces;

namespace AppyNox.Services.License.Application.Services.Interfaces
{
    public interface IGenericService<TEntity> : IGenericServiceBase<TEntity>
    where TEntity : class, IEntityWithGuid
    {
    }
}
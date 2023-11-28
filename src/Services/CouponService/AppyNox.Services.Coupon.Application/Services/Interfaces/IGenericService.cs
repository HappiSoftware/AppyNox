using AppyNox.Services.Base.Application.Services.Implementations;
using AppyNox.Services.Base.Application.Services.Interfaces;
using AppyNox.Services.Base.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.Coupon.Application.Services.Interfaces
{
    public interface IGenericService<TEntity, TDto> : IGenericServiceBase<TEntity, TDto>
    where TEntity : class, IEntityWithGuid
    where TDto : class
    {
    }
}
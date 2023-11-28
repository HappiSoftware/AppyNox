using AppyNox.Services.Base.Application.ExceptionExtensions;
using AppyNox.Services.Base.Application.Services.Implementations;
using AppyNox.Services.Base.Domain.Common;
using AppyNox.Services.Base.Domain.Interfaces;
using AppyNox.Services.Base.Infrastructure.Interfaces;
using AppyNox.Services.Coupon.Application.Dtos.DtoUtilities;
using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using System;
using FluentValidation.Results;
using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Models.Base;
using FluentValidation.Internal;
using AppyNox.Services.Coupon.Application.Services.Interfaces;

namespace AppyNox.Services.Coupon.Application.Services.Implementations
{
    public class GenericService<TEntity, TDto> : GenericServiceBase<TEntity, TDto>, IGenericService<TEntity, TDto>
    where TEntity : class, IEntityWithGuid
    where TDto : class
    {
        #region [ Public Constructors ]

        public GenericService(IGenericRepositoryBase<TEntity> repository, IMapper mapper, DtoMappingRegistry dtoMappingRegistry, IUnitOfWorkBase unitOfWork,
            IServiceProvider serviceProvider)
            : base(repository, mapper, dtoMappingRegistry, unitOfWork, serviceProvider)
        {
        }

        #endregion
    }
}
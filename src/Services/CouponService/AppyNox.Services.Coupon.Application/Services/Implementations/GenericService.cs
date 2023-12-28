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
using AppyNox.Services.Base.Application.DtoUtilities;
using AppyNox.Services.Base.Application.Logger;

namespace AppyNox.Services.Coupon.Application.Services.Implementations
{
    public class GenericService<TEntity> : GenericServiceBase<TEntity>, IGenericService<TEntity>
    where TEntity : class, IEntityWithGuid
    {
        #region [ Public Constructors ]

        public GenericService(IGenericRepositoryBase<TEntity> repository, IMapper mapper, IDtoMappingRegistryBase dtoMappingRegistry, IUnitOfWorkBase unitOfWork,
            IServiceProvider serviceProvider, INoxApplicationLogger logger)
            : base(repository, mapper, dtoMappingRegistry, unitOfWork, serviceProvider, logger)
        {
        }

        #endregion
    }
}
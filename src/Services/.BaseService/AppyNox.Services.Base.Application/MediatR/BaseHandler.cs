using AppyNox.Services.Base.Application.DtoUtilities;
using AppyNox.Services.Base.Application.ExceptionExtensions;
using AppyNox.Services.Base.Application.ExceptionExtensions.Base;
using AppyNox.Services.Base.Application.Interfaces.Caches;
using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Base.Application.Interfaces.Repositories;
using AppyNox.Services.Base.Core.Enums;
using AppyNox.Services.Base.Core.Extensions;
using AutoMapper;
using FluentValidation;
using FluentValidation.Internal;
using System.Dynamic;
using System.Text.Json;

namespace AppyNox.Services.Base.Application.MediatR
{
    internal abstract class BaseHandler<TEntity>(
        IMapper mapper,
        IDtoMappingRegistryBase dtoMappingRegistry,
        IServiceProvider serviceProvider,
        INoxApplicationLogger logger)
    {
        #region [ Fields ]

        protected readonly IMapper Mapper = mapper;

        protected readonly IDtoMappingRegistryBase DtoMappingRegistry = dtoMappingRegistry;

        protected readonly IServiceProvider ServiceProvider = serviceProvider;

        protected readonly INoxApplicationLogger Logger = logger;

        protected readonly Type EntityType = typeof(TEntity);

        protected readonly JsonSerializerOptions JsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        #endregion

        #region [ Protected Methods ]

        /// <summary>
        /// Gets the type of DTO (Data Transfer Object) based on the specified query parameters.
        /// </summary>
        /// <param name="queryParameters">The query parameters specifying access type, entity type, and detail level.</param>
        /// <returns>The type of DTO mapped for the given query parameters.</returns>

        protected Type GetDtoType(IQueryParameters queryParameters)
        {
            return DtoMappingRegistry.GetDtoType(queryParameters.AccessType, EntityType, queryParameters.DetailLevel); ;
        }

        /// <summary>
        /// Performs validation on a given DTO using FluentValidation
        /// </summary>
        /// <param name="dtoType">The type of the DTO to validate</param>
        /// <param name="dtoObject">The DTO object to validate</param>
        /// <exception cref="ValidatorNotFoundException">Thrown if validator for the DTO is not found in Dependency Injection Container</exception>
        /// <exception cref="FluentValidationException">Thrown if DTO validation is not succeed</exception>
        protected void FluentValidate(Type dtoType, dynamic dtoObject)
        {
            Type genericType = typeof(IValidator<>).MakeGenericType(dtoType);
            IValidator validator = ServiceProvider.GetService(genericType) as IValidator ?? throw new ValidatorNotFoundException(dtoType);
            var context = new ValidationContext<object>(dtoObject, new PropertyChain(), new DefaultValidatorSelector());
            var validationResult = validator.Validate(context);
            if (!validationResult.IsValid)
            {
                throw new FluentValidationException(dtoType, validationResult);
            }
        }

        /// <summary>
        /// Increase or decrease the value of TEntity TotalCount on Cache service.
        /// </summary>
        /// <param name="cacheService">Cache Service</param>
        /// <param name="countCacheKey">Cache Key used for Count for TEntity</param>
        /// <param name="isCreate">True for increasing the value, false for decreasing the value of Cache Count Item</param>
        /// <returns></returns>
        protected static async Task UpdateTotalCountOnCache(ICacheService cacheService, string countCacheKey, bool isCreate)
        {
            // Try to get the count from cache
            var cachedCount = await cacheService.GetCachedValueAsync(countCacheKey);
            if (int.TryParse(cachedCount, out int totalCount))
            {
                totalCount = isCreate ? totalCount + 1 : totalCount - 1;
                await cacheService.SetCachedValueAsync(countCacheKey, totalCount.ToString(), TimeSpan.FromMinutes(10));
            }
        }

        #endregion
    }
}
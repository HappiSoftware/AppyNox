using AppyNox.Services.Base.Application.Exceptions;
using AppyNox.Services.Base.Application.Interfaces.Caches;
using AutoMapper;
using FluentValidation;
using FluentValidation.Internal;
using System.Text.Json;

namespace AppyNox.Services.Base.Application.MediatR;

public abstract class BaseHandler<TEntity>(
        IMapper mapper,
        IServiceProvider serviceProvider)
{
    #region [ Fields ]

    protected readonly IMapper Mapper = mapper;

    protected readonly IServiceProvider ServiceProvider = serviceProvider;

    protected readonly Type EntityType = typeof(TEntity);

    protected readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    #endregion

    #region [ Protected Methods ]

    /// <summary>
    /// Performs validation on a given DTO using FluentValidation
    /// </summary>
    /// <param name="dtoObject">The DTO object to validate</param>
    /// <exception cref="ValidatorNotFoundException">Thrown if validator for the DTO is not found in Dependency Injection Container</exception>
    /// <exception cref="NoxFluentValidationException">Thrown if DTO validation is not succeed</exception>
    protected void FluentValidate<TDto>(TDto dtoObject)
    {
        Type genericType = typeof(IValidator<TDto>);
        IValidator validator = ServiceProvider.GetService(genericType) as IValidator ?? throw new ValidatorNotFoundException(typeof(TDto));
        var context = new ValidationContext<TDto>(dtoObject, new PropertyChain(), new DefaultValidatorSelector());
        var validationResult = validator.Validate(context);
        if (!validationResult.IsValid)
        {
            throw new NoxFluentValidationException(typeof(TDto), validationResult);
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
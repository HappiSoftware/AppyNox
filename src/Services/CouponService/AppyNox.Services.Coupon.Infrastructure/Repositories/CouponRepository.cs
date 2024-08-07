using AppyNox.Services.Base.Application.Interfaces.Caches;
using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Base.Application.Interfaces.Repositories;
using AppyNox.Services.Base.Core.Common;
using AppyNox.Services.Base.Infrastructure.Repositories;
using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Models.Extended;
using AppyNox.Services.Coupon.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using static AppyNox.Services.Base.Infrastructure.Repositories.RepositoryHelpers;
using CouponRoot = AppyNox.Services.Coupon.Domain.Coupons.Coupon;

namespace AppyNox.Services.Coupon.Infrastructure.Repositories;

public class CouponRepository(
    CouponDbContext context,
    INoxInfrastructureLogger<CouponRepository> noxInfrastructureLogger,
    INoxInfrastructureLogger<NoxRepositoryBase<CouponRoot>> baseLogger,
    ICacheService cacheService)
        : NoxRepositoryBase<CouponRoot>(context, baseLogger), ICouponRepository
{
    private readonly CouponDbContext _context = context;

    private readonly INoxInfrastructureLogger<CouponRepository> _logger = noxInfrastructureLogger;

    private readonly ICacheService _cacheService = cacheService;

    private readonly string _countCacheKey = $"total-count-{typeof(CouponRoot).Name}";

    public async Task<PaginatedList<CouponRoot>> GetAllEfCoreAsync(IQueryParameters queryParameters)
    {
        try
        {
            _logger.LogInformation($"Attempting to retrieve entities. Type: '{typeof(CouponRoot).Name}'.");

            // Try to get the count from cache
            var cachedCount = await _cacheService.GetCachedValueAsync(_countCacheKey);
            if (!int.TryParse(cachedCount, out int totalCount))
            {
                // Count not in cache or invalid, so retrieve from database and cache it
                totalCount = await _context.Coupons.CountAsync();
                await _cacheService.SetCachedValueAsync(_countCacheKey, totalCount.ToString(), TimeSpan.FromMinutes(10));
            }

            var query = _context.Coupons
                .AsQueryable();

            if (!string.IsNullOrEmpty(queryParameters.SortBy) && IsValidExpression(queryParameters.SortBy, _logger))
            {
                query = query.OrderBy(queryParameters.SortBy);
            }

            if (!string.IsNullOrEmpty(queryParameters.Filter) && IsValidExpression(queryParameters.Filter, _logger))
            {
                query = query.Where(queryParameters.Filter);
            }

            var entities = await query
                .Skip((queryParameters.PageNumber - 1) * queryParameters.PageSize)
                .Take(queryParameters.PageSize)
                .ToListAsync();

            _logger.LogInformation($"Successfully retrieved entities. Type: '{typeof(CouponWithAllRelationsDto).Name}'.");
            return new PaginatedList<CouponRoot>
            {
                Items = entities,
                ItemsCount = entities.Count,
                TotalCount = totalCount,
                CurrentPage = queryParameters.PageNumber,
                PageSize = queryParameters.PageSize
            };
        }
        catch (Exception)
        {
            throw;
        }
    }
}

public interface ICouponRepository
{
    Task<PaginatedList<CouponRoot>> GetAllEfCoreAsync(IQueryParameters queryParameters);
}
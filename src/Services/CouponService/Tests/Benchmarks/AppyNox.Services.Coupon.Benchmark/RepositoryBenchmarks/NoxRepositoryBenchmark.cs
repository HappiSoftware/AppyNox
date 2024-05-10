using AppyNox.Services.Base.Application.Interfaces.Caches;
using AppyNox.Services.Base.Application.Interfaces.Repositories;
using AppyNox.Services.Base.Infrastructure.Repositories.Common;
using AppyNox.Services.Coupon.Infrastructure.Repositories;
using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.DependencyInjection;
using CouponRoot = AppyNox.Services.Coupon.Domain.Coupons.Coupon;

namespace AppyNox.Services.Coupon.Benchmark.RepositoryBenchmarks;

[MemoryDiagnoser]
public class NoxRepositoryBenchmark
{
    private IServiceProvider ServiceProvider;

    [GlobalSetup]
    public void Setup()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddInfrastructureBenchmark();
        ServiceProvider = serviceCollection.BuildServiceProvider();
    }

    [Benchmark]
    public async Task NoxGetAllAsyncPlainSimpleBenchmark()
    {
        INoxRepository<CouponRoot> noxRepository = ServiceProvider.GetRequiredService<INoxRepository<CouponRoot>>();
        ICacheService cacheService = ServiceProvider.GetRequiredService<ICacheService>();
        IQueryParameters queryParameters = new QueryParameters();
        await noxRepository.GetAllAsync(queryParameters, cacheService);
    }

    [Benchmark]
    public async Task NoxGetAllAsyncPlainBenchmark()
    {
        INoxRepository<CouponRoot> noxRepository = ServiceProvider.GetRequiredService<INoxRepository<CouponRoot>>();
        ICacheService cacheService = ServiceProvider.GetRequiredService<ICacheService>();
        IQueryParameters queryParameters = new QueryParameters();
        await noxRepository.GetAllAsync(queryParameters, cacheService);
    }

    [Benchmark]
    public async Task CouponGetAllAsyncPlainBenchmark()
    {
        ICouponRepository couponRepository = ServiceProvider.GetRequiredService<ICouponRepository>();
        IQueryParameters queryParameters = new QueryParameters();
        await couponRepository.GetAllEfCoreAsync(queryParameters);
    }

    [Benchmark]
    public async Task NoxGetAllAsyncWithSortSimpleBenchmark()
    {
        INoxRepository<CouponRoot> noxRepository = ServiceProvider.GetRequiredService<INoxRepository<CouponRoot>>();
        ICacheService cacheService = ServiceProvider.GetRequiredService<ICacheService>();
        IQueryParameters queryParameters = new QueryParameters
        {
            SortBy = "Id desc, CouponDetail.detail desc"
        };
        await noxRepository.GetAllAsync(queryParameters, cacheService);
    }

    [Benchmark]
    public async Task NoxGetAllAsyncWithSortBenchmark()
    {
        INoxRepository<CouponRoot> noxRepository = ServiceProvider.GetRequiredService<INoxRepository<CouponRoot>>();
        ICacheService cacheService = ServiceProvider.GetRequiredService<ICacheService>();
        IQueryParameters queryParameters = new QueryParameters
        {
            SortBy = "Id desc, CouponDetail.detail desc"
        };
        await noxRepository.GetAllAsync(queryParameters, cacheService);
    }

    [Benchmark]
    public async Task CouponGetAllAsyncWithSortBenchmark()
    {
        ICouponRepository couponRepository = ServiceProvider.GetRequiredService<ICouponRepository>();
        IQueryParameters queryParameters = new QueryParameters
        {
            SortBy = "Id desc, CouponDetail.detail desc"
        };
        await couponRepository.GetAllEfCoreAsync(queryParameters);
    }

    [Benchmark]
    public async Task NoxGetAllAsyncWithFilterSimpleBenchmark()
    {
        INoxRepository<CouponRoot> noxRepository = ServiceProvider.GetRequiredService<INoxRepository<CouponRoot>>();
        ICacheService cacheService = ServiceProvider.GetRequiredService<ICacheService>();
        IQueryParameters queryParameters = new QueryParameters
        {
            Filter = "Amount.minAmount == 100 and (detail == \"Detail2\" || detail.Contains(\"2\"))"
        };
        await noxRepository.GetAllAsync(queryParameters, cacheService);
    }

    [Benchmark]
    public async Task NoxGetAllAsyncWithFilterBenchmark()
    {
        INoxRepository<CouponRoot> noxRepository = ServiceProvider.GetRequiredService<INoxRepository<CouponRoot>>();
        ICacheService cacheService = ServiceProvider.GetRequiredService<ICacheService>();
        IQueryParameters queryParameters = new QueryParameters
        {
            Filter = "Amount.minAmount == 100 and (detail == \"Detail2\" || detail.Contains(\"2\"))"
        };
        await noxRepository.GetAllAsync(queryParameters, cacheService);
    }

    [Benchmark]
    public async Task CouponGetAllAsyncWithFilterBenchmark()
    {
        ICouponRepository couponRepository = ServiceProvider.GetRequiredService<ICouponRepository>();
        IQueryParameters queryParameters = new QueryParameters
        {
            Filter = "Amount.minAmount == 100 and (detail == \"Detail2\" || detail.Contains(\"2\"))"
        };
        await couponRepository.GetAllEfCoreAsync(queryParameters);
    }

    [Benchmark]
    public async Task NoxGetAllAsyncWithSortAndFilterSimpleBenchmark()
    {
        INoxRepository<CouponRoot> noxRepository = ServiceProvider.GetRequiredService<INoxRepository<CouponRoot>>();
        ICacheService cacheService = ServiceProvider.GetRequiredService<ICacheService>();
        IQueryParameters queryParameters = new QueryParameters
        {
            SortBy = "Id desc, CouponDetail.detail desc",
            Filter = "Amount.minAmount == 100 and (detail == \"Detail2\" || detail.Contains(\"2\"))"
        };
        await noxRepository.GetAllAsync(queryParameters, cacheService);
    }

    [Benchmark]
    public async Task NoxGetAllAsyncWithSortAndFilterBenchmark()
    {
        INoxRepository<CouponRoot> noxRepository = ServiceProvider.GetRequiredService<INoxRepository<CouponRoot>>();
        ICacheService cacheService = ServiceProvider.GetRequiredService<ICacheService>();
        IQueryParameters queryParameters = new QueryParameters
        {
            SortBy = "Id desc, CouponDetail.detail desc",
            Filter = "Amount.minAmount == 100 and (detail == \"Detail2\" || detail.Contains(\"2\"))"
        };
        await noxRepository.GetAllAsync(queryParameters, cacheService);
    }

    [Benchmark]
    public async Task CouponGetAllAsyncWithSortAndFilterBenchmark()
    {
        ICouponRepository couponRepository = ServiceProvider.GetRequiredService<ICouponRepository>();
        IQueryParameters queryParameters = new QueryParameters
        {
            SortBy = "Id desc, CouponDetail.detail desc",
            Filter = "Amount.minAmount == 100 and (detail == \"Detail2\" || detail.Contains(\"2\"))"
        };
        await couponRepository.GetAllEfCoreAsync(queryParameters);
    }
}
using AppyNox.Services.Base.Application.DtoUtilities;
using AppyNox.Services.Base.Application.Services.Implementations;
using AppyNox.Services.Base.Domain.Interfaces;
using AppyNox.Services.Base.Infrastructure.Interfaces;
using AppyNox.Services.Base.Infrastructure.Repositories.Common;
using AppyNox.Services.Coupon.Application.Dtos.DtoUtilities;
using AppyNox.Services.Coupon.Application.Services.Implementations;
using AppyNox.Services.Coupon.Application.Services.Interfaces;
using AppyNox.Services.Coupon.Infrastructure.Data;
using AppyNox.Services.Coupon.Infrastructure.Repositories;
using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Linq.Expressions;
using System.Reflection;

namespace AppyNox.Services.Coupon.IntegrationTests.ServiceTests.Fixtures
{
    public class ServiceFixture<TEntity> : IDisposable where TEntity : class, IEntityWithGuid
    {
        #region Fields

        public readonly IServiceProvider ServiceProvider;

        public readonly GenericServiceBase<TEntity> GenericServiceBase;

        #endregion

        #region Public Constructors

        public ServiceFixture()
        {
            var services = new ServiceCollection();

            #region [ Infrastructure Layer DI ]

            services.AddDbContext<CouponDbContext>(options =>
                options.UseInMemoryDatabase("TestDatabase"));

            services.AddScoped(typeof(IGenericRepositoryBase<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWorkBase, UnitOfWork>();

            #endregion

            #region [ Application Layer DI ]

            services.AddAutoMapper(Assembly.Load("AppyNox.Services.Coupon.Application"));
            services.AddValidatorsFromAssembly(Assembly.Load("AppyNox.Services.Coupon.Application"));

            services.AddScoped(typeof(IGenericService<>), typeof(GenericService<>));
            services.AddSingleton(typeof(IDtoMappingRegistryBase), typeof(DtoMappingRegistry));

            #endregion

            ServiceProvider = services.BuildServiceProvider();
        }

        #endregion

        #region Public Methods

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
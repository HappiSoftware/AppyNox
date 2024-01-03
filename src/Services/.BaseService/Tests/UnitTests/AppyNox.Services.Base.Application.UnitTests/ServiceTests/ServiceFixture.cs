using AppyNox.Services.Base.Application.DtoUtilities;
using AppyNox.Services.Base.Infrastructure.Interfaces;
using AppyNox.Services.Base.Infrastructure.Repositories.Common;
using AutoMapper;
using Moq;
using System.Linq.Expressions;
using AppyNox.Services.Base.Application.Services.Implementations;
using AppyNox.Services.Base.Domain.Interfaces;
using AppyNox.Services.Base.Application.Logger;
using Microsoft.Extensions.Logging;

namespace AppyNox.Services.Base.Application.UnitTests.ServiceTests
{
    public class ServiceFixture<TEntity> : IDisposable
        where TEntity : class, IEntityWithGuid
    {
        #region [ Fields ]

        public readonly Mock<IGenericRepositoryBase<TEntity>> MockRepository;

        public readonly Mock<IMapper> MockMapper;

        public readonly Mock<IDtoMappingRegistryBase> MockDtoMappingRegistry;

        public readonly Mock<IUnitOfWorkBase> MockUnitOfWork;

        public readonly Mock<IServiceProvider> MockServiceProvider;

        public readonly GenericServiceBase<TEntity> GenericServiceBase;

        public readonly NoxApplicationLogger NoxApplicationLogger;

        private readonly Mock<ILogger<NoxApplicationLogger>> _mockLogger;

        #endregion

        #region [ Public Constructors ]

        public ServiceFixture()
        {
            MockRepository = new();
            MockMapper = new();
            MockDtoMappingRegistry = new();
            MockUnitOfWork = new();
            MockServiceProvider = new();
            _mockLogger = new();
            NoxApplicationLogger = new(_mockLogger.Object);
            GenericServiceBase = new GenericServiceBase<TEntity>(MockRepository.Object, MockMapper.Object, MockDtoMappingRegistry.Object, MockUnitOfWork.Object, MockServiceProvider.Object, NoxApplicationLogger);

            #region [ Repository Mocks ]

            MockRepository.Setup(repo => repo.GetAllAsync(It.IsAny<QueryParametersBase>(), It.IsAny<Expression<Func<TEntity, dynamic>>>()))
                .ReturnsAsync(new Mock<List<dynamic>>().Object);

            MockRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<Expression<Func<TEntity, dynamic>>>()))
                .ReturnsAsync(new Mock<TEntity>().Object);

            MockRepository.Setup(repo => repo.AddAsync(It.IsAny<TEntity>()))
                .ReturnsAsync(new Mock<TEntity>().Object);

            #endregion

            MockMapper.Setup(mapper => mapper.Map(It.IsAny<object>(), It.IsAny<Type>(), It.IsAny<Type>()))
                .Returns((object source, Type sourceType, Type destinationType) =>
                {
                    return Activator.CreateInstance(destinationType)!;
                });
        }

        #endregion

        #region [ Public Methods ]

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
using AppyNox.Services.Base.Application.Dtos;
using AppyNox.Services.Base.Application.DtoUtilities;
using AppyNox.Services.Base.Application.Interfaces.Caches;
using AppyNox.Services.Base.Application.Interfaces.Repositories;
using AppyNox.Services.Base.Application.Localization;
using AppyNox.Services.Base.Application.MediatR.Commands;
using AppyNox.Services.Base.Application.MediatR.Handlers.DDD;
using AppyNox.Services.Base.Application.UnitTests.Stubs;
using AppyNox.Services.Base.Core.Enums;
using AppyNox.Services.Base.Domain.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Moq;

namespace AppyNox.Services.Base.Application.UnitTests.CQRSFixtures
{
    public class NoxCQRSFixture<TEntity, TId> : IDisposable
        where TEntity : class, IHasStronglyTypedId
        where TId : class, IHasGuidId
    {
        #region [ Fields ]

        public readonly Mock<IDtoMappingRegistryBase> MockDtoMappingRegistry;

        public readonly Mock<IServiceProvider> MockServiceProvider;

        public readonly Mock<IQueryParameters> MockQueryParameters;

        public readonly Mock<ICacheService> _cacheService;

        public readonly Mock<INoxRepositoryBase<TEntity>> MockRepository;

        public readonly Mock<IMapper> MockMapper;

        private readonly Mock<IUnitOfWork> _mockUnitOfWork;

        private readonly NoxApplicationLoggerStub _noxApplicationLogger;

        #endregion

        #region [ Properties ]

        public Mock<IMediator> MockMediator { get; private set; }

        internal GetAllNoxEntitiesQueryHandler<TEntity> GetAllNoxEntitiesCommandHandler { get; set; }

        internal GetNoxEntityByIdQueryHandler<TEntity, TId> GetNoxEntityByIdCommandHandler { get; set; }

        internal CreateNoxEntityCommandHandler<TEntity> CreateNoxEntityCommandHandler { get; set; }

        internal DeleteNoxEntityCommandHandler<TEntity, TId> DeleteNoxEntityCommandHandler { get; set; }

        #endregion

        #region [ Public Constructors ]

        public NoxCQRSFixture()
        {
            MockRepository = new();
            MockMapper = new();
            MockDtoMappingRegistry = new();
            _mockUnitOfWork = new();
            MockServiceProvider = new();
            _noxApplicationLogger = new();
            MockMediator = new();
            _cacheService = new();

            #region [ QueryParameter Mocks ]

            MockQueryParameters = new Mock<IQueryParameters>();
            MockQueryParameters.Setup(p => p.PageNumber).Returns(1);
            MockQueryParameters.Setup(p => p.PageSize).Returns(10);
            MockQueryParameters.Setup(p => p.AccessType).Returns(DtoLevelMappingTypes.DataAccess);
            MockQueryParameters.Setup(p => p.Access).Returns(string.Empty);
            MockQueryParameters.Setup(p => p.DetailLevel).Returns("Simple");

            #endregion

            #region [ Handlers ]

            _cacheService.Setup(rcs => rcs.GetCachedValueAsync(It.IsAny<string>())).ReturnsAsync((string key) => null);
            _cacheService.Setup(rcs => rcs.SetCachedValueAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<TimeSpan?>()))
                .Returns(Task.CompletedTask);

            GetAllNoxEntitiesCommandHandler = new GetAllNoxEntitiesQueryHandler<TEntity>(MockRepository.Object, MockMapper.Object, MockDtoMappingRegistry.Object, MockServiceProvider.Object, _noxApplicationLogger, _cacheService.Object);
            GetNoxEntityByIdCommandHandler = new GetNoxEntityByIdQueryHandler<TEntity, TId>(MockRepository.Object, MockMapper.Object, MockDtoMappingRegistry.Object, MockServiceProvider.Object, _noxApplicationLogger);
            CreateNoxEntityCommandHandler = new CreateNoxEntityCommandHandler<TEntity>(MockRepository.Object, MockMapper.Object, MockDtoMappingRegistry.Object, MockServiceProvider.Object, _noxApplicationLogger, _mockUnitOfWork.Object, _cacheService.Object);
            DeleteNoxEntityCommandHandler = new DeleteNoxEntityCommandHandler<TEntity, TId>(MockRepository.Object, MockMapper.Object, MockDtoMappingRegistry.Object, MockServiceProvider.Object, _noxApplicationLogger, _mockUnitOfWork.Object, _cacheService.Object);

            #endregion

            MockMediator.Setup(m => m.Send(It.IsAny<GetNoxEntityByIdQuery<TEntity, TId>>(), It.IsAny<CancellationToken>()))
                .Returns((GetNoxEntityByIdQuery<TEntity, TId> cmd, CancellationToken ct) => GetNoxEntityByIdCommandHandler.Handle(cmd, ct));

            MockMediator.Setup(m => m.Send(It.IsAny<GetAllNoxEntitiesQuery<TEntity>>(), It.IsAny<CancellationToken>()))
                .Returns((GetAllNoxEntitiesQuery<TEntity> cmd, CancellationToken ct) => GetAllNoxEntitiesCommandHandler.Handle(cmd, ct));

            MockMediator.Setup(m => m.Send(It.IsAny<CreateNoxEntityCommand<TEntity>>(), It.IsAny<CancellationToken>()))
                .Returns((CreateNoxEntityCommand<TEntity> cmd, CancellationToken ct) => CreateNoxEntityCommandHandler.Handle(cmd, ct));

            MockMediator.Setup(m => m.Send(It.IsAny<DeleteNoxEntityCommand<TEntity, TId>>(), It.IsAny<CancellationToken>()))
                .Returns((DeleteNoxEntityCommand<TEntity, TId> cmd, CancellationToken ct) => DeleteNoxEntityCommandHandler.Handle(cmd, ct));

            #region [ Localization ]

            var localizer = new Mock<IStringLocalizer>();
            localizer.Setup(l => l[It.IsAny<string>()]).Returns(new LocalizedString("key", "mock value"));

            var localizerFactory = new Mock<IStringLocalizerFactory>();
            localizerFactory.Setup(lf => lf.Create(typeof(NoxApplicationResourceService))).Returns(localizer.Object);

            NoxApplicationResourceService.Initialize(localizerFactory.Object);

            #endregion
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
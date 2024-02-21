using AppyNox.Services.Base.Application.Dtos;
using AppyNox.Services.Base.Application.DtoUtilities;
using AppyNox.Services.Base.Application.Interfaces.Caches;
using AppyNox.Services.Base.Application.Interfaces.Repositories;
using AppyNox.Services.Base.Application.Localization;
using AppyNox.Services.Base.Application.MediatR.Commands;
using AppyNox.Services.Base.Application.MediatR.Handlers.Anemic;
using AppyNox.Services.Base.Application.UnitTests.Stubs;
using AppyNox.Services.Base.Core.Enums;
using AppyNox.Services.Base.Domain.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Moq;

namespace AppyNox.Services.Base.Application.UnitTests.CQRSFixtures;

public class GenericCQRSFixture<TEntity> : IDisposable
        where TEntity : class, IEntityWithGuid
{
    #region [ Fields ]

    public readonly Mock<IDtoMappingRegistryBase> MockDtoMappingRegistry;

    public readonly Mock<IServiceProvider> MockServiceProvider;

    public readonly Mock<IQueryParameters> MockQueryParameters;

    public readonly Mock<ICacheService> _cacheService;

    private readonly Mock<IGenericRepositoryBase<TEntity>> _mockRepository;

    private readonly Mock<IMapper> _mockMapper;

    private readonly Mock<IUnitOfWorkBase> _mockUnitOfWork;

    private readonly NoxApplicationLoggerStub _noxApplicationLogger;

    #endregion

    #region [ Properties ]

    public Mock<IMediator> MockMediator { get; private set; }

    public GetAllEntitiesQueryHandler<TEntity> GetAllEntitiesCommandHandler { get; set; }

    public GetEntityByIdQueryHandler<TEntity> GetEntityByIdCommandHandler { get; set; }

    public CreateEntityCommandHandler<TEntity> CreateEntityCommandHandler { get; set; }

    public UpdateEntityCommandHandler<TEntity> UpdateEntityCommandHandler { get; set; }

    public DeleteEntityCommandHandler<TEntity> DeleteEntityCommandHandler { get; set; }

    #endregion

    #region [ Public Constructors ]

    public GenericCQRSFixture()
    {
        _mockRepository = new();
        _mockMapper = new();
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
        MockQueryParameters.Setup(p => p.CommonDtoLevel).Returns(CommonDtoLevelEnums.Simple);
        MockQueryParameters.Setup(p => p.AccessType).Returns(DtoLevelMappingTypes.DataAccess);
        MockQueryParameters.Setup(p => p.Access).Returns(string.Empty);
        MockQueryParameters.Setup(p => p.DetailLevel).Returns("Simple");

        #endregion

        #region [ Repository Mocks ]

        _mockRepository.Setup(repo => repo.GetAllAsync(It.IsAny<IQueryParameters>(), It.IsAny<ICacheService>()))
            .ReturnsAsync(new Mock<PaginatedList>().Object);

        _mockRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new Mock<TEntity>().Object);

        _mockRepository.Setup(repo => repo.AddAsync(It.IsAny<TEntity>()))
            .ReturnsAsync(new Mock<TEntity>().Object);

        #endregion

        _mockMapper.Setup(mapper => mapper.Map(It.IsAny<object>(), It.IsAny<Type>(), It.IsAny<Type>()))
            .Returns((object source, Type sourceType, Type destinationType) =>
            {
                return Activator.CreateInstance(destinationType)!;
            });

        #region [ Handlers ]

        _cacheService.Setup(rcs => rcs.GetCachedValueAsync(It.IsAny<string>())).ReturnsAsync((string key) => null);
        _cacheService.Setup(rcs => rcs.SetCachedValueAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<TimeSpan?>()))
            .Returns(Task.CompletedTask);

        GetAllEntitiesCommandHandler = new GetAllEntitiesQueryHandler<TEntity>(_mockRepository.Object, _mockMapper.Object, MockDtoMappingRegistry.Object, MockServiceProvider.Object, _noxApplicationLogger, _cacheService.Object);
        GetEntityByIdCommandHandler = new GetEntityByIdQueryHandler<TEntity>(_mockRepository.Object, _mockMapper.Object, MockDtoMappingRegistry.Object, MockServiceProvider.Object, _noxApplicationLogger);
        CreateEntityCommandHandler = new CreateEntityCommandHandler<TEntity>(_mockRepository.Object, _mockMapper.Object, MockDtoMappingRegistry.Object, MockServiceProvider.Object, _noxApplicationLogger, _mockUnitOfWork.Object, _cacheService.Object);
        UpdateEntityCommandHandler = new UpdateEntityCommandHandler<TEntity>(_mockRepository.Object, _mockMapper.Object, MockDtoMappingRegistry.Object, MockServiceProvider.Object, _noxApplicationLogger, _mockUnitOfWork.Object);
        DeleteEntityCommandHandler = new DeleteEntityCommandHandler<TEntity>(_mockRepository.Object, _mockMapper.Object, MockDtoMappingRegistry.Object, MockServiceProvider.Object, _noxApplicationLogger, _mockUnitOfWork.Object, _cacheService.Object);

        #endregion

        MockMediator.Setup(m => m.Send(It.IsAny<GetEntityByIdQuery<TEntity>>(), It.IsAny<CancellationToken>()))
            .Returns((GetEntityByIdQuery<TEntity> cmd, CancellationToken ct) => GetEntityByIdCommandHandler.Handle(cmd, ct));

        MockMediator.Setup(m => m.Send(It.IsAny<GetAllEntitiesQuery<TEntity>>(), It.IsAny<CancellationToken>()))
            .Returns((GetAllEntitiesQuery<TEntity> cmd, CancellationToken ct) => GetAllEntitiesCommandHandler.Handle(cmd, ct));

        MockMediator.Setup(m => m.Send(It.IsAny<CreateEntityCommand<TEntity>>(), It.IsAny<CancellationToken>()))
            .Returns((CreateEntityCommand<TEntity> cmd, CancellationToken ct) => CreateEntityCommandHandler.Handle(cmd, ct));

        MockMediator.Setup(m => m.Send(It.IsAny<UpdateEntityCommand<TEntity>>(), It.IsAny<CancellationToken>()))
            .Returns((UpdateEntityCommand<TEntity> cmd, CancellationToken ct) => UpdateEntityCommandHandler.Handle(cmd, ct));

        MockMediator.Setup(m => m.Send(It.IsAny<DeleteEntityCommand<TEntity>>(), It.IsAny<CancellationToken>()))
            .Returns((DeleteEntityCommand<TEntity> cmd, CancellationToken ct) => DeleteEntityCommandHandler.Handle(cmd, ct));

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
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

    public readonly Mock<IGenericRepository<TEntity>> MockRepository;

    public readonly Mock<IMapper> MockMapper;

    private readonly Mock<IUnitOfWork> _mockUnitOfWork;

    private readonly NoxApplicationLoggerStub _noxApplicationLogger;

    #endregion

    #region [ Properties ]

    public Mock<IMediator> MockMediator { get; private set; }

    internal GetAllEntitiesQueryHandler<TEntity> GetAllEntitiesCommandHandler { get; set; }

    internal GetEntityByIdQueryHandler<TEntity> GetEntityByIdCommandHandler { get; set; }

    internal CreateEntityCommandHandler<TEntity> CreateEntityCommandHandler { get; set; }

    internal UpdateEntityCommandHandler<TEntity> UpdateEntityCommandHandler { get; set; }

    internal DeleteEntityCommandHandler<TEntity> DeleteEntityCommandHandler { get; set; }

    #endregion

    #region [ Public Constructors ]

    public GenericCQRSFixture()
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

        GetAllEntitiesCommandHandler = new GetAllEntitiesQueryHandler<TEntity>(MockRepository.Object, MockMapper.Object, MockDtoMappingRegistry.Object, MockServiceProvider.Object, _noxApplicationLogger, _cacheService.Object);
        GetEntityByIdCommandHandler = new GetEntityByIdQueryHandler<TEntity>(MockRepository.Object, MockMapper.Object, MockDtoMappingRegistry.Object, MockServiceProvider.Object, _noxApplicationLogger);
        CreateEntityCommandHandler = new CreateEntityCommandHandler<TEntity>(MockRepository.Object, MockMapper.Object, MockDtoMappingRegistry.Object, MockServiceProvider.Object, _noxApplicationLogger, _mockUnitOfWork.Object, _cacheService.Object);
        UpdateEntityCommandHandler = new UpdateEntityCommandHandler<TEntity>(MockRepository.Object, MockMapper.Object, MockDtoMappingRegistry.Object, MockServiceProvider.Object, _noxApplicationLogger, _mockUnitOfWork.Object);
        DeleteEntityCommandHandler = new DeleteEntityCommandHandler<TEntity>(MockRepository.Object, MockMapper.Object, MockDtoMappingRegistry.Object, MockServiceProvider.Object, _noxApplicationLogger, _mockUnitOfWork.Object, _cacheService.Object);

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

        #endregion

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
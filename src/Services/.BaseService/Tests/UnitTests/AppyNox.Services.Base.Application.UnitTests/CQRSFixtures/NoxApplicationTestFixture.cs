using AppyNox.Services.Base.Application.Interfaces.Caches;
using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Base.Application.Interfaces.Repositories;
using AppyNox.Services.Base.Application.Localization;
using AppyNox.Services.Base.Application.UnitTests.Stubs;
using AppyNox.Services.Base.Core.Enums;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Moq;

namespace AppyNox.Services.Base.Application.UnitTests.CQRSFixtures;

public class NoxApplicationTestFixture : IDisposable
{
    #region [ Fields ]

    private readonly Mock<ICacheService> _cacheService;

    private readonly Mock<IUnitOfWork> _unitOfWork;

    public readonly IServiceCollection ServiceCollection;

    public readonly Mock<IQueryParameters> MockQueryParameters;

    public bool DIInitialized { get; set; } = false;

    #endregion

    #region [ Public Constructors ]

    public NoxApplicationTestFixture()
    {
        ServiceCollection = new ServiceCollection();
        ServiceCollection.AddScoped(typeof(INoxApplicationLogger<>), typeof(NoxApplicationLoggerStub<>));
        
        _unitOfWork = new();

        #region [ QueryParameter Mocks ]

        MockQueryParameters = new Mock<IQueryParameters>();
        MockQueryParameters.Setup(p => p.PageNumber).Returns(1);
        MockQueryParameters.Setup(p => p.PageSize).Returns(10);
        MockQueryParameters.Setup(p => p.AccessType).Returns(DtoLevelMappingTypes.DataAccess);
        MockQueryParameters.Setup(p => p.Access).Returns(string.Empty);
        MockQueryParameters.Setup(p => p.DetailLevel).Returns("Simple");

        #endregion

        #region [ Localization ]

        var localizer = new Mock<IStringLocalizer>();
        localizer.Setup(l => l[It.IsAny<string>()]).Returns(new LocalizedString("key", "mock value"));

        var localizerFactory = new Mock<IStringLocalizerFactory>();
        localizerFactory.Setup(lf => lf.Create(typeof(NoxApplicationResourceService))).Returns(localizer.Object);

        NoxApplicationResourceService.Initialize(localizerFactory.Object);

        #endregion

        #region [ Cache Service / UnitOfWork ]

        _cacheService = new();
        _cacheService.Setup(rcs => rcs.GetCachedValueAsync(It.IsAny<string>())).ReturnsAsync((string key) => null);
        _cacheService.Setup(rcs => rcs.SetCachedValueAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<TimeSpan?>()))
            .Returns(Task.CompletedTask);

        ServiceCollection.AddScoped(typeof(ICacheService), _ => _cacheService.Object);
        ServiceCollection.AddScoped(typeof(IUnitOfWork), _ => _unitOfWork.Object);
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
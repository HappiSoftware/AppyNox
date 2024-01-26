using AppyNox.Services.Base.Infrastructure.Localization;
using AppyNox.Services.Base.Infrastructure.UnitTests.Stubs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Moq;

namespace AppyNox.Services.Base.Infrastructure.UnitTests.Fixtures
{
    public class RepositoryFixture : IDisposable
    {
        #region [ Fields ]

        public readonly NoxInfrastructureLoggerStub NoxLoggerStub = new();

        private bool _disposed = false;

        #endregion

        #region Public Constructors

        public RepositoryFixture()
        {
            var localizer = new Mock<IStringLocalizer>();
            localizer.Setup(l => l[It.IsAny<string>()]).Returns(new LocalizedString("key", "mock value"));

            var localizerFactory = new Mock<IStringLocalizerFactory>();
            localizerFactory.Setup(lf => lf.Create(typeof(NoxInfrastructureResourceService))).Returns(localizer.Object);

            NoxInfrastructureResourceService.Initialize(localizerFactory.Object);
        }

        #endregion

        #region [ Public Methods ]

        public static TContext CreateDatabaseContext<TContext>() where TContext : DbContext
        {
            var databaseName = Guid.NewGuid().ToString(); // Unique database name
            var options = new DbContextOptionsBuilder<TContext>()
                .UseInMemoryDatabase(databaseName)
                .Options;

            // Using Activator to create an instance of TContext
            return Activator.CreateInstance(typeof(TContext), options) as TContext
                ?? throw new InvalidOperationException("Could not create an instance of the database context.");
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;
        }

        #endregion

        #region [ Private Destructors ]

        ~RepositoryFixture()
        {
            Dispose(false);
        }

        #endregion
    }
}
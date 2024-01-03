using AppyNox.Services.Base.Infrastructure.UnitTests.Stubs;
using Microsoft.EntityFrameworkCore;

namespace AppyNox.Services.Base.Infrastructure.UnitTests.Fixtures
{
    public class RepositoryFixture : IDisposable
    {
        #region [ Fields ]

        public readonly NoxInfrastructureLoggerStub NoxLoggerStub = new();

        #endregion

        #region [ Public Methods ]

        public TContext CreateDatabaseContext<TContext>() where TContext : DbContext
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
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
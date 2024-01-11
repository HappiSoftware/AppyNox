using AppyNox.Services.Authentication.Infrastructure.Data.SagaStateMaps;
using AppyNox.Services.Authentication.Infrastructure.MassTransit.Sagas;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;

namespace AppyNox.Services.Authentication.Infrastructure.Data
{
    public class IdentitySagaDatabaseContext(DbContextOptions<IdentitySagaDatabaseContext> options) : SagaDbContext(options)
    {
        #region [ Properties ]

        protected override IEnumerable<ISagaClassMap> Configurations
        {
            get { yield return new UserCreationSagaStateMap(); }
        }

        #endregion

        #region [ Db Sets ]

        public DbSet<UserCreationSagaState> UserCreationSagaStates { get; set; }

        #endregion
    }
}
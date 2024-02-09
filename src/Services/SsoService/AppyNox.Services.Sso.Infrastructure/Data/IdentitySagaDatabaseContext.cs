using AppyNox.Services.Sso.Infrastructure.Data.SagaStateMaps;
using AppyNox.Services.Sso.Infrastructure.MassTransit.Sagas;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;

namespace AppyNox.Services.Sso.Infrastructure.Data
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
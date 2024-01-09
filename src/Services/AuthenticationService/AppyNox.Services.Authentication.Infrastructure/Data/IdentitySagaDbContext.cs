using AppyNox.Services.Authentication.Infrastructure.Data.SagaStateMaps;
using AppyNox.Services.Authentication.Infrastructure.MassTransit.Sagas;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;

namespace AppyNox.Services.Authentication.Infrastructure.Data
{
    public class IdentitySagaDbContext : SagaDbContext
    {
        public IdentitySagaDbContext(DbContextOptions<IdentitySagaDbContext> options)
        : base(options)
        {
        }

        protected override IEnumerable<ISagaClassMap> Configurations
        {
            get { yield return new UserCreationSagaStateMap(); }
        }

        #region [ Db Sets ]

        public DbSet<UserCreationSagaState> UserCreationSagaStates { get; set; }

        #endregion
    }
}

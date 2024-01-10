using AppyNox.Services.Authentication.Infrastructure.MassTransit.Sagas;
using MassTransit;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace AppyNox.Services.Authentication.Infrastructure.Data.SagaStateMaps
{
    public class UserCreationSagaStateMap : SagaClassMap<UserCreationSagaState>
    {
        #region [ Protected Methods ]

        protected override void Configure(EntityTypeBuilder<UserCreationSagaState> entity, ModelBuilder model)
        {
            entity.ToTable("UserCreationSagaState");

            entity.Property(x => x.CorrelationId).HasColumnName("CorrelationId");
            entity.HasKey(x => x.CorrelationId);

            entity.Property(x => x.CurrentState).HasMaxLength(64).HasColumnName("CurrentState");
            entity.Property(x => x.LicenseKey).HasColumnName("LicenseKey");
            entity.Property(x => x.UserName).HasColumnName("UserName");
            entity.Property(x => x.Email).HasColumnName("Email");
            entity.Property(x => x.Password).HasColumnName("Password");
            entity.Property(x => x.ConfirmPassword).HasColumnName("ConfirmPassword");
            entity.Property(x => x.UserId).HasColumnName("UserId");
            entity.Property(x => x.LicenseId).HasColumnName("LicenseId");
        }

        #endregion
    }
}
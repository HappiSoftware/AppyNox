﻿using AppyNox.Services.Sso.Infrastructure.MassTransit.Sagas;
using MassTransit;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace AppyNox.Services.Sso.Infrastructure.Data.SagaStateMaps
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
            entity.Property(x => x.Name).HasColumnName("Name");
            entity.Property(x => x.Surname).HasColumnName("Surname");
            entity.Property(x => x.Code).HasColumnName("Code");
            entity.Property(x => x.CompanyId).HasColumnName("CompanyId");
        }

        #endregion
    }
}
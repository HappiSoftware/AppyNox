using AppyNox.Services.Base.Domain.Outbox;
using Microsoft.EntityFrameworkCore;

namespace AppyNox.Services.Base.Infrastructure.Data;

/// <summary>
/// Marker interface. Do not use.
/// </summary>
public interface INoxDatabaseContext
{
    DbSet<OutboxMessage> OutboxMessages { get; set; }
}
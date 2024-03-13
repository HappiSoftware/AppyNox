using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Base.Domain;
using AppyNox.Services.Base.Domain.Outbox;
using AppyNox.Services.Base.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Quartz;

namespace AppyNox.Services.Base.Infrastructure.BackgroundJobs;

[DisallowConcurrentExecution]
public class ProcessOutboxMessagesJob<TContext> : IJob where TContext : NoxDatabaseContext
{
    private readonly TContext _dbContext;

    private readonly IPublisher _publisher;

    private readonly INoxInfrastructureLogger _logger;

    public ProcessOutboxMessagesJob(TContext dbContext, IPublisher publisher, INoxInfrastructureLogger logger)
    {
        _dbContext = dbContext;
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        List<OutboxMessage> messages = await _dbContext.Set<OutboxMessage>()
            .Where(m => m.ProcessedOnUtc == null && m.RetryCount < 3)
            .Take(10)
            .ToListAsync(context.CancellationToken);

        foreach (var outboxMessage in messages)
        {
            IDomainEvent? domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(outboxMessage.Content, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
            outboxMessage.RetryCount += 1;
            if (domainEvent is null)
            {
                _logger.LogError(new Exception("Deserialization Error"), $"Couldn't deserialize message with ID: {outboxMessage.Id}. It's like trying to read a book in the dark.");
                continue;
            }

            try
            {
                await _publisher.Publish(domainEvent, context.CancellationToken);
                outboxMessage.ProcessedOnUtc = DateTime.UtcNow;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Attempt to publish message ID: {outboxMessage.Id} failed. This isn't a catastrophe yet, we'll retry. Retry count: {outboxMessage.RetryCount}");
            }
        }

        await _dbContext.SaveChangesAsync(context.CancellationToken);
    }
}
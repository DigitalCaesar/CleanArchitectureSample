using Data;
using Data.Models;
using Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using Quartz;

namespace Infrastructure.BackgroundJobs;

[DisallowConcurrentExecution]
public class ProcessOutboxMessagesJob : IJob
{
    private readonly ApplicationDbContext dbContext;
    private readonly IPublisher mPublisher;

    public ProcessOutboxMessagesJob(ApplicationDbContext dbContext, IPublisher publisher)
    {
        this.dbContext = dbContext;
        mPublisher = publisher;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        List<DomainEventData> messages = await dbContext
            .Set<DomainEventData>()
            .Where(m => m.ProcessedOnUtc == null)
            .Take(20)
            .ToListAsync(context.CancellationToken);

        foreach(DomainEventData message in messages)
        {
            DomainEvent? domainEvent = JsonConvert
                .DeserializeObject<DomainEvent>(
                    message.Content, 
                    new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });

            if (domainEvent is null)
                continue;

            AsyncRetryPolicy policy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(3, attempt => TimeSpan.FromMilliseconds(50 * attempt));

            PolicyResult policyResult = await policy.ExecuteAndCaptureAsync(() => 
                mPublisher.Publish(domainEvent, context.CancellationToken));

            message.Error = policyResult.FinalException?.ToString();
            message.ProcessedOnUtc = DateTime.UtcNow;
        }

        await dbContext.SaveChangesAsync();
    }
    public async Task ExecuteTryCatch(IJobExecutionContext context)
    {
        List<DomainEventData> messages = await dbContext
            .Set<DomainEventData>()
            .Where(m => m.ProcessedOnUtc == null)
            .Take(20)
            .ToListAsync(context.CancellationToken);

        foreach (DomainEventData message in messages)
        {
            DomainEvent? domainEvent = JsonConvert
                .DeserializeObject<DomainEvent>(
                    message.Content,
                    new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });

            if (domainEvent is null)
                continue;

            try
            {
                await mPublisher.Publish(domainEvent, context.CancellationToken);
                message.ProcessedOnUtc = DateTime.UtcNow;

            }
            catch (Exception ex)
            {
                message.Error = ex.ToString();
                message.ProcessedOnUtc = DateTime.UtcNow;

            }

        }

        await dbContext.SaveChangesAsync();
    }
}

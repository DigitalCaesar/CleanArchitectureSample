using Data;
using Data.Models;
using Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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

        // TODO: Wrap in try/catch.  If it fails, increment a failure counter, after x failures notify the admin, after x+y failures stop executing
        foreach(DomainEventData message in messages)
        {
            IDomainEvent? domainEvent = JsonConvert
                .DeserializeObject<IDomainEvent>(message.Content, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });

            if (domainEvent is null)
                continue; //TODO: Need to handle null better than this, log 

            await mPublisher.Publish(domainEvent, context.CancellationToken);

            message.ProcessedOnUtc = DateTime.UtcNow;
        }

        await dbContext.SaveChangesAsync();
    }
}

using Data;
using Data.Models;
using Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Quartz;
using System.Reflection.Metadata.Ecma335;

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
        List<OutboxMessage> messages = await dbContext
            .Set<OutboxMessage>()
            .Where(m => m.ProcessedOnUtc == null)
            .Take(20)
            .ToListAsync(context.CancellationToken);

        foreach(OutboxMessage message in messages)
        {
            var domainEvent = JsonConvert
                .DeserializeObject<IDomainEvent>(message.Content);

            if (domainEvent is null)
                continue; //TODO: Need to handle null better than this, log 

            await mPublisher.Publish(domainEvent, context.CancellationToken);

            message.ProcessedOnUtc = DateTime.UtcNow;
        }

        await dbContext.SaveChangesAsync();
    }
}

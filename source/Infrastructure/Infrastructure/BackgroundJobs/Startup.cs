using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Infrastructure.BackgroundJobs;
public static class BackgroundJobStartup
{
    public static IServiceCollection AddScheduler(this IServiceCollection services)
    {
        services.AddQuartz(configure =>
        {
            var jobKey = new JobKey(nameof(ProcessOutboxMessagesJob));

            configure
                .AddJob<ProcessOutboxMessagesJob>(jobKey)
                .AddTrigger(
                    trigger =>
                    trigger.ForJob(jobKey)
                        .WithSimpleSchedule(
                            schedule =>
                            schedule.WithIntervalInSeconds(10)  // Repeat the job every 10 seconds
                                    .RepeatForever()));         // Never stop repeating
            configure.UseMicrosoftDependencyInjectionJobFactory();
        });
        services.AddQuartzHostedService();

        return services;
    }
}


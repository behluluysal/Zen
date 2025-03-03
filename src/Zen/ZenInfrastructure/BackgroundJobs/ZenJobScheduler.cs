using Hangfire;
using Microsoft.EntityFrameworkCore;
using Zen.Infrastructure.Data;

namespace Zen.Infrastructure.BackgroundJobs;

public class ZenJobScheduler<TDbContext>(IRecurringJobManager recurringJobManager) where TDbContext : DbContext, IZenDbContext
{
    private readonly IRecurringJobManager _recurringJobManager = recurringJobManager;

    public void ScheduleRecurringJobs()
    {
        _recurringJobManager.AddOrUpdate<ProcessOutboxMessagesJob<TDbContext>>(
            "process-outbox-messages",
            job => job.Execute(),
            "*/1 * * * *",
            new RecurringJobOptions
            {
                TimeZone = TimeZoneInfo.Local
            }
        );

    }
}

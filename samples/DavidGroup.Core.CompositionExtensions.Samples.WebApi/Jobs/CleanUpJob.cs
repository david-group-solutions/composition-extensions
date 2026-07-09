using Quartz;

namespace DavidGroup.Core.CompositionExtensions.Samples.WebApi.Jobs;

public sealed class CleanupJob(ILogger<CleanupJob> logger) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        if (logger.IsEnabled(LogLevel.Information))
        {
            logger.LogInformation(
                "Cleanup job started at {StartedAt}. Fire instance: {FireInstanceId}",
                DateTimeOffset.UtcNow,
                context.FireInstanceId);
        }

        // Simulate cleanup work.
        await Task.Delay(TimeSpan.FromSeconds(2), context.CancellationToken);

        if (logger.IsEnabled(LogLevel.Information))
        {
            logger.LogInformation(
                "Cleanup job completed at {CompletedAt}.",
                DateTimeOffset.UtcNow);
        }
    }
}

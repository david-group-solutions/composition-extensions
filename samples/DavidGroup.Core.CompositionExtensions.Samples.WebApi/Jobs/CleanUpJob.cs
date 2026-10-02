using Quartz;

namespace DavidGroup.Core.CompositionExtensions.Samples.WebApi.Jobs;

public sealed class CleanupJob(ILogger<CleanupJob> logger) : IJob
{
    public async ValueTask Execute(IJobExecutionContext context, CancellationToken cancellationToken)
    {
        if (logger.IsEnabled(LogLevel.Information))
        {
            logger.LogInformation(
                "Cleanup job started at {StartedAt}. Fire instance: {FireInstanceId}",
                DateTimeOffset.UtcNow,
                context.FireInstanceId);
        }

        // Simulate cleanup work.
        await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);

        if (logger.IsEnabled(LogLevel.Information))
        {
            logger.LogInformation(
                "Cleanup job completed at {CompletedAt}",
                DateTimeOffset.UtcNow);
        }
    }
}

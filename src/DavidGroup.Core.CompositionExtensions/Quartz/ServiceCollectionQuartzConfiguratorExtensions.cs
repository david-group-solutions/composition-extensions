using Quartz;

namespace DavidGroup.Core.CompositionExtensions.Quartz;

/// <summary>
/// Provides extension methods for registering and configuring Quartz.NET jobs and triggers with default settings in an ASP.NET Core application.
/// </summary>
public static class ServiceCollectionQuartzConfiguratorExtensions
{
    /// <summary>
    /// Registers a Cron job of type <typeparamref name="T"/> and adds a corresponding trigger based on the specified Cron expression.
    /// </summary>
    /// <typeparam name="T">The type of the job implementing <see cref="IJob"/>.</typeparam>
    /// <param name="quartz">The <see cref="IServiceCollectionQuartzConfigurator"/> used to configure the job.</param>
    /// <param name="cronSchedule">The Cron expression to define the job's schedule.</param>
    /// <param name="action">An optional <see cref="Action{CronScheduleBuilder}"/> to further customize the trigger schedule.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="cronSchedule"/> is <c>null</c>.</exception>
    /// <remarks>
    /// This method automatically:
    /// <list type="bullet">
    /// <item>Creates a <see cref="JobKey"/> based on the job type's name.</item>
    /// <item>Adds the job to the Quartz configuration.</item>
    /// <item>Adds a trigger for the job using the provided Cron schedule.</item>
    /// </list>
    /// </remarks>
    public static void AddCronJobAndTrigger<T>(this IServiceCollectionQuartzConfigurator quartz,
        string? cronSchedule,
        Action<CronScheduleBuilder>? action = null)
        where T : IJob
    {
        ArgumentNullException.ThrowIfNull(cronSchedule);

        string jobName = typeof(T).Name;
        JobKey jobKey = new(jobName);

        quartz.AddJob<T>(opts => opts.WithIdentity(jobKey));

        quartz.AddTrigger(opts => opts
            .ForJob(jobKey)
            .WithIdentity(jobName + "-trigger")
            .WithCronSchedule(cronSchedule, action)
        );
    }
}

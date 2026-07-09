using Microsoft.Extensions.DependencyInjection;

using Quartz;
using Quartz.Impl.AdoJobStore;

namespace DavidGroup.Core.CompositionExtensions.Quartz;

/// <summary>
/// Provides extension methods for configuring common services and libraries in an ASP.NET Core application.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds Quartz.NET services with default configuration, including optional persistent store, clustering, JSON serialization, and hosted service support.
    /// </summary>
    /// <param name="services">
    /// The <see cref="IServiceCollection"/> to add Quartz services to.
    /// </param>
    /// <param name="configurePersistentStore">
    /// Configures the persistent store provider (SQL Server, PostgreSQL, etc.).
    /// </param>
    /// <param name="jobs">
    /// An optional list of job registration delegates to configure individual jobs.
    /// </param>
    /// <returns>The <see cref="IServiceCollection"/> for chaining.</returns>
    /// <remarks>
    /// This method configures Quartz.NET with:
    /// <list type="bullet">
    /// <item>Persistent store using configuration action.</item>
    /// <item>Clustering enabled for distributed job execution.</item>
    /// <item>JSON serialization for job data using Newtonsoft.Json.</item>
    /// <item>Automatic registration of jobs and triggers provided via <paramref name="jobs"/> delegates.</item>
    /// <item>A hosted service that waits for jobs to complete on application shutdown.</item>
    /// </list>
    /// </remarks>
    public static IServiceCollection AddQuartzDefaults(this IServiceCollection services,
        Action<SchedulerBuilder.PersistentStoreOptions>? configurePersistentStore = null,
        params QuartzJobRegistration[] jobs)
    {
        services.AddQuartz(quartz =>
        {
            if (configurePersistentStore is not null)
            {
                quartz.UsePersistentStore(store =>
                {
                    store.UseProperties = false;

                    store.UseNewtonsoftJsonSerializer();

                    configurePersistentStore.Invoke(store);

                    store.UseClustering();
                });
            }

            foreach (QuartzJobRegistration job in jobs)
                job(quartz);
        });

        services.AddQuartzHostedService(opt => { opt.WaitForJobsToComplete = true; });

        return services;
    }

    /// <summary>
    /// Adds default Quartz.NET with SQL Server persistant store.
    /// </summary>
    /// <param name="services">
    /// The <see cref="IServiceCollection"/> to add Quartz services to.
    /// </param>
    /// <param name="connectionString">
    /// The database connection string for Quartz's persistent job store.
    /// </param>
    /// <param name="jobs">
    /// An optional list of job registration delegates to configure individual jobs.
    /// </param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="connectionString"/> is <c>null</c>.</exception>
    /// <returns>The <see cref="IServiceCollection"/> for chaining.</returns>
    /// <remarks>
    /// This method configures Quartz.NET with:
    /// <list type="bullet">
    /// <item>Persistent store using SQL Server.</item>
    /// <item>Clustering enabled for distributed job execution.</item>
    /// <item>JSON serialization for job data using Newtonsoft.Json.</item>
    /// <item>Automatic registration of jobs and triggers provided via <paramref name="jobs"/> delegates.</item>
    /// <item>A hosted service that waits for jobs to complete on application shutdown.</item>
    /// </list>
    /// </remarks>
    public static IServiceCollection AddQuartzDefaultsWithSqlServer(this IServiceCollection services,
        string? connectionString,
        params QuartzJobRegistration[] jobs)
    {
        ArgumentNullException.ThrowIfNull(connectionString);

        services.AddQuartzDefaults(
            store => store.UseSqlServer(sqlServerOptions =>
            {
                sqlServerOptions.UseDriverDelegate<SqlServerDelegate>();
                sqlServerOptions.ConnectionString = connectionString;
                sqlServerOptions.TablePrefix = "quartz.QRTZ_";
            }),
            jobs);

        return services;
    }

    /// <summary>
    /// Delegate type for registering Quartz jobs using an <see cref="IServiceCollectionQuartzConfigurator"/>.
    /// </summary>
    /// <param name="quartz">The <see cref="IServiceCollectionQuartzConfigurator"/> used to configure jobs and triggers.</param>
    public delegate void QuartzJobRegistration(IServiceCollectionQuartzConfigurator quartz);
}

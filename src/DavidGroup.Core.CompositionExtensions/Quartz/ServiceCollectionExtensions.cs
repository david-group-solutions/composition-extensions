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
    /// Adds Quartz.NET services with default configuration, including persistent store, clustering, JSON serialization, and hosted service support.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add Quartz services to.</param>
    /// <param name="connectionString">The database connection string for Quartz's persistent job store.</param>
    /// <param name="jobs">An optional list of job registration delegates to configure individual jobs.</param>
    /// <returns>The <see cref="IServiceCollection"/> for chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="connectionString"/> is <c>null</c>.</exception>
    /// <remarks>
    /// This method configures Quartz.NET with:
    /// <list type="bullet">
    /// <item>Persistent store using SQL Server with table prefix "quartz.QRTZ_".</item>
    /// <item>Clustering enabled for distributed job execution.</item>
    /// <item>JSON serialization for job data using Newtonsoft.Json.</item>
    /// <item>Automatic registration of jobs and triggers provided via <paramref name="jobs"/> delegates.</item>
    /// <item>A hosted service that waits for jobs to complete on application shutdown.</item>
    /// </list>
    /// </remarks>
    public static IServiceCollection AddQuartzDefaults(this IServiceCollection services,
        string? connectionString,
        params QuartzJobRegistration[] jobs)
    {
        ArgumentNullException.ThrowIfNull(connectionString);

        services.AddQuartz(quartz =>
        {
            quartz.UsePersistentStore(store =>
            {
                store.UseProperties = false;

                store.UseNewtonsoftJsonSerializer();

                store.UseSqlServer(sqlServerOptions =>
                {
                    sqlServerOptions.UseDriverDelegate<SqlServerDelegate>();
                    sqlServerOptions.ConnectionString = connectionString;
                    sqlServerOptions.TablePrefix = "quartz.QRTZ_";
                });

                store.UseClustering();
            });

            foreach (QuartzJobRegistration job in jobs)
                job(quartz);
        });

        services.AddQuartzHostedService(opt => { opt.WaitForJobsToComplete = true; });

        return services;
    }

    /// <summary>
    /// Delegate type for registering Quartz jobs using an <see cref="IServiceCollectionQuartzConfigurator"/>.
    /// </summary>
    /// <param name="quartz">The <see cref="IServiceCollectionQuartzConfigurator"/> used to configure jobs and triggers.</param>
    public delegate void QuartzJobRegistration(IServiceCollectionQuartzConfigurator quartz);
}

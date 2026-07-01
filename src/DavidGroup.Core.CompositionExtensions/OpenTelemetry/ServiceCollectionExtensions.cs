using System.Reflection;

using Microsoft.Extensions.DependencyInjection;

using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace DavidGroup.Core.CompositionExtensions.OpenTelemetry;

/// <summary>
/// Provides extension methods for configuring common services and libraries in an ASP.NET Core application.
/// </summary>
public static class ServiceCollectionExtensions
{

    /// <summary>
    /// Registers default OpenTelemetry tracing and metrics services for ASP.NET Core applications.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add OpenTelemetry services to.</param>
    /// <param name="serviceName">Optional service name to identify telemetry. Defaults to the executing assembly name.</param>
    /// <returns>The <see cref="IServiceCollection"/> for chaining.</returns>
    /// <remarks>
    /// This method configures:
    /// <list type="bullet">
    /// <item>Resource information using serviceName or executed assembly name if serviceName is not specified.</item>
    /// <item>Tracing for ASP.NET Core, HTTP client, Entity Framework Core, and gRPC clients.</item>
    /// <item>Metrics for ASP.NET Core and HTTP client.</item>
    /// <item>OTLP exporters for both tracing and metrics.</item>
    /// </list>
    /// </remarks>
    public static IServiceCollection AddDefaultOpenTelemetry(this IServiceCollection services, string? serviceName = null)
    {
        services.AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService(serviceName ?? Assembly.GetExecutingAssembly().GetName().Name!))
            .WithTracing(tracing =>
            {
                tracing.AddAspNetCoreInstrumentation();
                tracing.AddHttpClientInstrumentation();
                tracing.AddEntityFrameworkCoreInstrumentation();
                tracing.AddGrpcClientInstrumentation();

                tracing.AddOtlpExporter();
            })
            .WithMetrics(metrics =>
            {
                metrics.AddAspNetCoreInstrumentation();
                metrics.AddHttpClientInstrumentation();

                metrics.AddOtlpExporter();
            });

        return services;
    }
}

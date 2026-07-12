using Asp.Versioning;

using Microsoft.Extensions.DependencyInjection;

namespace DavidGroup.Core.CompositionExtensions.ApiVersioning;

/// <summary>
/// Provides extension methods for configuring common services and libraries in an ASP.NET Core application.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Configures default API versioning and versioned API explorer for the application.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add API versioning services to.</param>
    /// <returns><see cref="IServiceCollection"/> for chaining.</returns>
    /// <remarks>
    /// Sets up:
    /// <list type="bullet">
    /// <item>Default API version 1.0.</item>
    /// <item>Assumes the default version when unspecified by the client.</item>
    /// <item>Reports supported API versions in responses.</item>
    /// <item>Reads API version from URL segments, query string, headers, or media type.</item>
    /// <item>Configures the versioned API explorer to generate documentation groups with format "v{major}.{minor}".</item>
    /// </list>
    /// </remarks>
    public static IServiceCollection AddDefaultApiVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(opt =>
            {
                opt.DefaultApiVersion = new ApiVersion(1, 0);
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.ReportApiVersions = true;
                opt.ApiVersionReader = ApiVersionReader.Combine(
                    new UrlSegmentApiVersionReader(),
                    new QueryStringApiVersionReader(),
                    new HeaderApiVersionReader("x-api-version"),
                    new MediaTypeApiVersionReader("x-api-version"));
            })
            .AddMvc()
            .AddApiExplorer(opt =>
            {
                opt.GroupNameFormat = "'v'VVV";
                opt.SubstituteApiVersionInUrl = true;
            });

        return services;
    }
}

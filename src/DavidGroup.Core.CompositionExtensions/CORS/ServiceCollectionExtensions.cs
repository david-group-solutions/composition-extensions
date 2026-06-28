using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DavidGroup.Core.CompositionExtensions.CORS;

/// <summary>
/// Provides extension methods for configuring common services and libraries in an ASP.NET Core application.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Configures Cross-Origin Resource Sharing (CORS) based on application configuration.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add CORS services to.</param>
    /// <param name="configuration">The <see cref="IConfiguration"/> instance containing CORS settings.</param>
    /// <remarks>
    /// Reads <see cref="ApplicationCorsOptions"/> from configuration and sets up a default CORS policy:
    /// <list type="bullet">
    /// <item>Allows any header or only those specified in AllowedHeaders.</item>
    /// <item>Allows any method or only those specified in AllowedMethods.</item>
    /// <item>Restricts origins if AllowedOrigins are specified.</item>
    /// <item>Always allows credentials.</item>
    /// </list>
    /// </remarks>
    /// <exception cref="InvalidOperationException">Thrown if the CORS configuration section is missing or invalid.</exception>
    public static void AddCorsFromConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        ApplicationCorsOptions corsOptions = configuration
                                                 .GetSection(nameof(ApplicationCorsOptions))
                                                 .Get<ApplicationCorsOptions>()
                                             ?? throw new InvalidOperationException("Cors options not found in configuration.");

        services.AddCors(options =>
            options.AddDefaultPolicy(policy =>
            {
                if (corsOptions.AllowedHeaders is null)
                    policy.AllowAnyHeader();
                else
                    policy.WithHeaders(corsOptions.AllowedHeaders.Split(';'));

                if (corsOptions.AllowedMethods is null)
                    policy.AllowAnyMethod();
                else
                    policy.WithMethods(corsOptions.AllowedMethods.Split(';'));

                if (corsOptions.AllowedOrigins is not null)
                    policy.WithOrigins(corsOptions.AllowedOrigins.Split(';'));

                policy.AllowCredentials();
            })
        );
    }
}

using Microsoft.Extensions.Configuration;

namespace DavidGroup.Core.CompositionExtensions.CORS;

/// <summary>
/// Binds values from <see cref="IConfiguration"/> in order to configure CORS.
/// </summary>
public class ApplicationCorsOptions
{
    /// <summary>
    /// Which origins are allowed to access resource.
    /// </summary>
    /// <remarks>Use ';' to separate multiple values.</remarks>
    public string? AllowedOrigins { get; init; }

    /// <summary>
    /// Which http methods are allowed to be called.
    /// </summary>
    /// <remarks>Use ';' to separate multiple values.</remarks>
    public string? AllowedMethods { get; init; }

    /// <summary>
    /// Which headers are allowed to be used.
    /// </summary>
    /// <remarks>Use ';' to separate multiple values.</remarks>
    public string? AllowedHeaders { get; init; }
}

using System.Text.Json.Serialization;

using Microsoft.Extensions.DependencyInjection;

namespace DavidGroup.Core.CompositionExtensions.Shared;

/// <summary>
/// Provides extension methods for <see cref="IMvcBuilder"/>.
/// </summary>
public static class MvcBuilderExtensions
{
    /// <summary>
    /// Configures JSON serialization to represent enum values as strings
    /// instead of numeric values.
    /// </summary>
    /// <param name="builder">The MVC builder to configure.</param>
    /// <returns>The same <see cref="IMvcBuilder"/> instance for fluent configuration.</returns>
    public static IMvcBuilder AddJsonStringEnumConverter(this IMvcBuilder builder)
    {
        builder.AddJsonOptions(options =>
            options.JsonSerializerOptions.Converters.Add(
                new JsonStringEnumConverter()));

        return builder;
    }
}

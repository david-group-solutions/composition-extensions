using System.Net;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DavidGroup.Core.CompositionExtensions.ForwardedHeadersOptions;

/// <summary>
/// Provides extension methods for configuring common services and libraries in an ASP.NET Core application.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Configures <see cref="ForwardedHeadersOptions"/> from application configuration,
    /// including trusted proxy IP addresses and networks.
    ///
    /// The method binds the base options from configuration, then explicitly clears and
    /// repopulates <c>KnownProxies</c> and <c>KnownNetworks</c>/<c>KnownIPNetworks</c>
    /// to ensure only explicitly configured proxies are trusted.
    ///
    /// Supports both legacy and newer .NET versions via conditional compilation.
    /// </summary>
    public static IServiceCollection ConfigureForwardedHeadersOptionsFromConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<Microsoft.AspNetCore.Builder.ForwardedHeadersOptions>(options =>
        {
            configuration.GetSection(nameof(Microsoft.AspNetCore.Builder.ForwardedHeadersOptions)).Bind(options);

            IConfigurationSection knownProxiesSection =
                configuration.GetSection($"{nameof(Microsoft.AspNetCore.Builder.ForwardedHeadersOptions)}:{nameof(Microsoft.AspNetCore.Builder.ForwardedHeadersOptions.KnownProxies)}");

            options.KnownProxies.Clear();

            foreach (IConfigurationSection proxyAddress in knownProxiesSection.GetChildren())
            {
                if (IPAddress.TryParse(proxyAddress.Value, out IPAddress? ipAddress))
                {
                    options.KnownProxies.Add(ipAddress);
                }
            }

#if NET10_0_OR_GREATER
            IConfigurationSection knownIpNetworksSection =
                configuration.GetSection($"{nameof(Microsoft.AspNetCore.Builder.ForwardedHeadersOptions)}:{nameof(Microsoft.AspNetCore.Builder.ForwardedHeadersOptions.KnownIPNetworks)}");

            options.KnownIPNetworks.Clear();

            foreach (IConfigurationSection networkConfig in knownIpNetworksSection.GetChildren())
            {
                string? prefix = networkConfig.GetValue<string>("Prefix");
                int prefixLength = networkConfig.GetValue<int>("PrefixLength");

                if (prefix != null && IPAddress.TryParse(prefix, out IPAddress? ipPrefix))
                {
                    options.KnownIPNetworks.Add(new IPNetwork(ipPrefix, prefixLength));
                }
            }
#else
            IConfigurationSection knownIpNetworksSection =
                configuration.GetSection($"{nameof(Microsoft.AspNetCore.Builder.ForwardedHeadersOptions)}:{nameof(Microsoft.AspNetCore.Builder.ForwardedHeadersOptions.KnownNetworks)}");

            options.KnownNetworks.Clear();

            foreach (IConfigurationSection networkConfig in knownIpNetworksSection.GetChildren())
            {
                string? prefix = networkConfig.GetValue<string>("Prefix");
                int prefixLength = networkConfig.GetValue<int>("PrefixLength");

                if (prefix != null && IPAddress.TryParse(prefix, out IPAddress? ipPrefix))
                {
                    options.KnownNetworks.Add(new Microsoft.AspNetCore.HttpOverrides.IPNetwork(ipPrefix, prefixLength));
                }
            }
#endif
        });

        return services;
    }
}

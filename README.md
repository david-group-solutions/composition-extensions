# DavidGroup.Core.CompositionExtensions

#### [![Release](https://github.com/david-group-solutions/composition-extensions/actions/workflows/release.yml/badge.svg)](https://github.com/david-group-solutions/composition-extensions/actions/workflows/release.yml) [![Nuget](https://img.shields.io/nuget/v/DavidGroup.Core.CompositionExtensions)](https://www.nuget.org/packages/DavidGroup.Core.CompositionExtensions/)

Common and helpful extensions to be used in application composition root for .NET applications.

---

## 🚀 Getting Started

### Install NuGet Package

Using the .NET CLI:

```bash
dotnet add package DavidGroup.Core.CompositionExtensions
```

Or via the Package Manager Console:

```bash
Install-Package DavidGroup.Core.CompositionExtensions
```

### How to use it?

Feel free to explore the [samples](https://github.com/david-group-solutions/composition-extensions/tree/main/samples) to
find
practical examples for each feature.
New samples are added continuously as more features are developed.

## 📦 Key Features

### CORS Configuration

```json
{
  "CorsOptions": {
    "AllowedOrigins": "https://app.example.com;https://admin.example.com",
    "AllowedMethods": "GET;POST;PUT;DELETE",
    "AllowedHeaders": "Content-Type;Authorization"
  }
}
```

```csharp
builder.Services.AddCorsFromConfiguration(builder.Configuration);

app.UseCors();
```

### Forwarded Headers

```json
{
  "ForwardedHeadersOptions": {
    "ForwardedHeaders": "XForwardedFor,XForwardedProto",
    "KnownProxies": [
      "10.0.0.100"
    ],
    "KnownNetworks": [
      {
        "Prefix": "10.0.0.0",
        "PrefixLength": 24
      }
    ]
  }
}
```

```csharp
builder.Services.ConfigureForwardedHeadersOptionsFromConfiguration(builder.Configuration);

app.UseForwardedHeaders();
```

### API Versioning

```csharp
builder.Services.AddDefaultApiVersioning();

[ApiVersion(1.0)]
[Route("api/v{version:apiVersion}/users")]
public sealed class UsersController : ControllerBase
{
}
```

### Serilog

```json
{
  "Serilog": {
    "MinimumLevel": "Information",
    "WriteTo": [
      {
        "Name": "Console"
      }
    ]
  }
}
```

```csharp
builder.Host.UseSerilogFromConfiguration();
```

### OpenTelemetry

```csharp
builder.Services.AddDefaultOpenTelemetry("BookstoreService");
```

### Quartz.NET

```csharp
builder.Services.AddQuartzDefaults(
    builder.Configuration.GetConnectionString("DefaultConnection"),
    quartz => quartz.AddCronJobAndTrigger<CleanupJob>("0 0 * * * ?"));
```

```csharp
public sealed class CleanupJob : IJob
{
    public Task Execute(IJobExecutionContext context)
    {
        // Execute scheduled work.
        return Task.CompletedTask;
    }
}
```

## 🤝 Contributing

Found a bug? Have an idea? Want to contribute?

* Submit an issue:
  https://github.com/david-group-solutions/composition-extensions/issues
* Create a pull request:
  https://github.com/david-group-solutions/composition-extensions/pulls

Contributions of any size are appreciated!

## 📝 License

Distributed under the **MIT license**.
See [License](https://github.com/david-group-solutions/composition-extensions/blob/main/LICENSE.txt) for more
information.

Copyright © 2025-2026 David Khachatryan (David Group Solutions)

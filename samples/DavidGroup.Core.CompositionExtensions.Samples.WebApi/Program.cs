using DavidGroup.Core.CompositionExtensions.ApiVersioning;
using DavidGroup.Core.CompositionExtensions.CORS;
using DavidGroup.Core.CompositionExtensions.ForwardedHeadersOptions;
using DavidGroup.Core.CompositionExtensions.OpenTelemetry;
using DavidGroup.Core.CompositionExtensions.Quartz;
using DavidGroup.Core.CompositionExtensions.Samples.WebApi.Data;
using DavidGroup.Core.CompositionExtensions.Samples.WebApi.Jobs;
using DavidGroup.Core.CompositionExtensions.Serilog;

using Microsoft.EntityFrameworkCore;

using Quartz;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilogFromConfiguration();

builder.Services.ConfigureForwardedHeadersOptionsFromConfiguration(builder.Configuration);
builder.Services.AddCorsFromConfiguration(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddDefaultApiVersioning();

builder.Services.AddDefaultOpenTelemetry("DavidGroup.Core.CompositionExtensions.Samples.WebApi");

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddQuartzDefaultsWithSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection"),
    quartz => quartz.AddCronJobAndTrigger<CleanupJob>("*/30 * * * * ?"));

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using IServiceScope scope = app.Services.CreateScope();
    AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

app.UseForwardedHeaders();
app.UseCors();

app.MapControllers();

app.Run();

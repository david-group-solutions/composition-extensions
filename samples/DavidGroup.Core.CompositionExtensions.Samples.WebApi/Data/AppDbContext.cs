using AppAny.Quartz.EntityFrameworkCore.Migrations;
using AppAny.Quartz.EntityFrameworkCore.Migrations.SqlServer;

using Microsoft.EntityFrameworkCore;

namespace DavidGroup.Core.CompositionExtensions.Samples.WebApi.Data;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.AddQuartz(config => config.UseSqlServer());
    }
}

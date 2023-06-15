using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var configuration = CreateConfiguration();
using var serviceProvider = CreateServices(configuration);
using var scope = serviceProvider.CreateScope();

UpdateDatabase(scope.ServiceProvider);

static IConfiguration CreateConfiguration()
{
    return new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", true)
        .AddEnvironmentVariables()
        .Build();
}

static ServiceProvider CreateServices(IConfiguration configuration)
{
    var connectionString = configuration.GetConnectionString("DefaultConnection");

    return new ServiceCollection()
        .AddFluentMigratorCore()
        .ConfigureRunner(rb => rb
            .AddPostgres()
            .WithGlobalConnectionString(connectionString)
            .ScanIn(typeof(Program).Assembly).For.Migrations())
        .AddLogging(lb => lb
            .AddFluentMigratorConsole())
        .BuildServiceProvider();
}

static void UpdateDatabase(IServiceProvider serviceProvider)
{
    var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
    var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

    if (!runner.HasMigrationsToApplyUp())
    {
        logger.LogInformation("No migrations were applied. The database is already up to date");
        return;
    }

    runner.MigrateUp();
}
using Serilog;
using AutoMapper;
using FW.EntityFramework;
using FW.BusinessLogic.Services.Abstractions;
using FW.BusinessLogic.Services;
using FW.RabbitMQOptions;
using FW.Management.Configurations;
using Microsoft.EntityFrameworkCore;

try
{
    int SleepStart = 5;

    Console.WriteLine($"Waiting start RabbitMQ for {SleepStart} sec...");
    Thread.Sleep(TimeSpan.FromSeconds(SleepStart));

    var builder = WebApplication.CreateBuilder(args);
    builder.Environment.ApplicationName = typeof(Program).Assembly.FullName;
    builder.Services.ConfigureLogger();
    builder.Services.ConfigureMapper();
    builder.Services.AddDbContext<ApplicationContext>(options =>
    {
        options.UseNpgsql(builder.Configuration.GetConnectionString("ApplicationContext"));
        options.UseNpgsql(x => x.MigrationsAssembly("FW.Migrations"));
    });
    builder.Services.ConfigureRabbitMQ();
    builder.Services.ConfigureEventHandlers();
    builder.Services.ConfigureMassTransit(builder.Configuration);
    builder.Services.AddServices();   
    var app = WebApplicationConfiguration.Configure(builder);

    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<ApplicationContext>();
        if (context.Database.GetPendingMigrations().Any())
        {
            context.Database.Migrate();
        }
    }
    Log.Logger.Information($"The {app.Environment.ApplicationName} started...");
    app.Run();

    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly!");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}

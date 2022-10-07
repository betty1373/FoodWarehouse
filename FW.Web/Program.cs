using Serilog;
using FW.Web.Configurations;


try
{   
    int SleepStart = 5;

    Console.WriteLine($"Waiting start RabbitMQ for {SleepStart} sec...");
    Thread.Sleep(TimeSpan.FromSeconds(SleepStart));

    var builder = WebApplication.CreateBuilder(args);
    builder.Environment.ApplicationName = typeof(Program).Assembly.FullName;

    builder.Services.ConfigureLogger(builder.Configuration);
    builder.Services.ConfigureMapper();
    builder.Services.AddConnectionRabbitMQ();
    builder.Services.ConfigureMassTransit(builder.Configuration);
    builder.Services.AddRequestClients();
    builder.Services.AddRpcClients();
    builder.Services.ConfigureAuthentication(builder.Configuration);
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.ConfigureSwagger();
    builder.Services.AddAuthorization();
    builder.Services.AddControllers();

    var app = WebApplicationConfiguration.Configure(builder);
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

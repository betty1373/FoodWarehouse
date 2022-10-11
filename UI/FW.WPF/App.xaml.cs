using System;
using System.Threading;
using System.Windows;
using FW.WebAPI.Infrastructure;
using FW.WPF.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FW.WPF.Models;
namespace FW.WPF;

public partial class App
{
    public static bool IsDesignMode { get; private set; } = true;

    private static IHost? __Host;

    public static IHost Hosting => __Host ??= CreateHostBuilder(Environment.GetCommandLineArgs()).Build();

    public static IServiceProvider Services => Hosting.Services;

    public static IConfiguration Configuration => Services.GetRequiredService<IConfiguration>();

    public static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args)
       .ConfigureServices(ConfigureServices);

    private static void ConfigureServices(HostBuilderContext host, IServiceCollection services)
    {
        services.AddSingleton<MainWindowViewModel>();
        services.AddSingleton<IngredientViewModel>();
        services.AddSingleton<DishViewModel>();
        services.AddSingleton<RecipeViewModel>();
        services.AddSingleton<LoginModel>();
        services.AddWebAPI(host.Configuration["WebAPI"]);
        services.AddIdentityAPI(host.Configuration["IdentityAPI"]);
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        Thread.Sleep(TimeSpan.FromSeconds(5));
        IsDesignMode = false;
        var host = Hosting;

        base.OnStartup(e);

        await host.StartAsync();
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        using var host = Hosting;

        base.OnExit(e);

        await host.StopAsync();
    }
}

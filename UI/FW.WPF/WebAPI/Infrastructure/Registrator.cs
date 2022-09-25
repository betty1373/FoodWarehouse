
using FW.WPF.WebAPI.Clients;
using FW.WPF.WebAPI.Interfaces;
using FW.Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using Polly.Extensions.Http;
using Polly;
using System;
using System.Net.Http;
using FW.WPF.Identity.Interfaces;
using FW.WPF.Identity.Clients;
using FW.WPF.Identity.Clients.Base;
using FW.WPF.ViewModels;

namespace FW.WebAPI.Infrastructure;

public static class Registrator
{
    public static IServiceCollection AddWebAPI(this IServiceCollection services, string Address)
    {
        services.AddHttpClient("FW.WebApi", client => client.BaseAddress = new(Address))
           //.AddTypedClient<IProductsRepository, ProductsClient>()
           //.AddTypedClient<ICustomersRepository, CustomersClient>()
           //.AddTypedClient<IOrdersRepository, OrdersClient>()
           //.AddTypedClient<IRepository<Product>, ProductsClient>()
           //.AddTypedClient<IRepository<Order>, OrdersClient>()
           //.AddTypedClient<IRepository<Customer>, CustomersClient>()
           .AddTypedClient<IClientBase<DishResponseVM, DishVM>, DishesClient>()
           .AddTypedClient<IRecipesClient, RecipesClient>()
        //    .AddTypedClient<IClientIdentity, ClientIdentity>()
           //   .AddTypedClient<ImagesClient>()
           //.AddPolicyHandler(GetRetryPolicy())
           //.AddPolicyHandler(GetCircuitBreakerPolicy())
           .SetHandlerLifetime(TimeSpan.FromMinutes(15));
        return services;
    }
    public static IServiceCollection AddIdentityAPI(this IServiceCollection services, string Address)
    {
        services.AddHttpClient("FW.Identity", client => client.BaseAddress = new(Address))
          .AddTypedClient<IClientIdentity<LoginModel>, ClientIdentity>()
        .SetHandlerLifetime(TimeSpan.FromMinutes(15));
       // services.AddTransient<IClientIdentity<LoginModel>, ClientIdentity>();
        return services;
    }
    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(int MaxRetryCount = 5, int MaxJitterTime = 1000)
    {
        var jitter = new Random();
        return HttpPolicyExtensions
           .HandleTransientHttpError()
           .WaitAndRetryAsync(
                MaxRetryCount, RetryAttempt =>
                    TimeSpan.FromSeconds(Math.Pow(2, RetryAttempt)) +
                    TimeSpan.FromMilliseconds(jitter.Next(0, MaxJitterTime)));
    }

    private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy() =>
        HttpPolicyExtensions
           .HandleTransientHttpError()
           .CircuitBreakerAsync(handledEventsAllowedBeforeBreaking: 5, TimeSpan.FromSeconds(30));
}

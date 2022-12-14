using System.Net;
using System.Net.Http.Json;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;
using FW.WPF.Identity.Interfaces;
using FW.Models;
using FW.WPF.ViewModels;
using IdentityModel.Client;
using FW.WPF.Domain.Exceptions;

namespace FW.WPF.Identity.Clients.Base;

public abstract class ClientIdentityBase<T> : IClientIdentity<T> where T : class,IUserIdentity
{
    protected HttpClient Http { get; }

    protected string Address { get; }

    protected ClientIdentityBase(HttpClient Client, string Address)
    {
        Http = Client;
        this.Address = Address;
    }

    public async Task<string?> RequestPasswordTokenAsync(T model,  CancellationToken Cancel = default)
    {
        var disco = await Http.GetDiscoveryDocumentAsync(Address, Cancel).ConfigureAwait(false);

        if (disco.IsError) throw new IdentityServerNotFoundException(disco.Exception.Message);
        
        var response = await Http.RequestPasswordTokenAsync(new PasswordTokenRequest
        {
            // Эндпоинт выдачи токена
            Address = disco.TokenEndpoint,

            // указываем параметры зарегестрированного клиента в микросервисе FW.Identity
            ClientId = "clientWpf",
            ClientSecret = "45467dee-f65d-481c-9508-74891854ddaa",
            Scope = "scopeWebAPI",

            // указываем параметры зарегестрированного пользователя
            UserName = model.UserName,
            Password = model.Password
        }, Cancel).ConfigureAwait(false);
        if (response.IsError) throw new UserNotFoundException(response.ErrorDescription);
        return response.AccessToken;
    }
}

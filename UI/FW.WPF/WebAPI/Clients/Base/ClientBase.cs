using System.Net;
using System.Net.Http.Json;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;
using FW.WPF.WebAPI.Interfaces;
using IdentityModel.Client;
using FW.Models;
using FW.WPF.WebAPI;

namespace FW.WPF.WebAPI.Clients.Base;

public abstract class ClientBase<T,K> : IClientBase<T,K>
    where T : class, IEntity
    where K : class

{ 
    protected HttpClient Http { get; }

    protected string Address { get; }

    protected ClientBase(HttpClient Client, string Address)
    {
        Http = Client;
        this.Address = Address;
    }

    public async Task<int> Count(CancellationToken Cancel = default)
    {
        var count = await Http
           .GetFromJsonAsync<int>($"{Address}/count", Cancel)
           .ConfigureAwait(false);
        return count;
    }

    public async Task<IEnumerable<T>> GetAllAsync(string? token,CancellationToken Cancel = default)
    {
        Http.SetBearerToken(token);
        var response = await Http.GetAsync($"{Address}/GetAll", Cancel).ConfigureAwait(false);

        if (response.StatusCode == HttpStatusCode.NoContent) return Enumerable.Empty<T>();

        var items = await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadFromJsonAsync<IEnumerable<T>>(cancellationToken: Cancel)
            ?? throw new InvalidOperationException($"Не удалось получить список {typeof(T).Name}");
        
        return items;
    }

    public async Task<IEnumerable<T>> GetAsync(int Skip, int Take, string? token, CancellationToken Cancel = default)
    {
        Http.SetBearerToken(token);
        var response = await Http.GetAsync($"{Address}/GetPage/({Skip}:{Take})", Cancel).ConfigureAwait(false);

        if (response.StatusCode == HttpStatusCode.NoContent) return Enumerable.Empty<T>();

        var items = await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadFromJsonAsync<IEnumerable<T>>(cancellationToken: Cancel)
            ?? throw new InvalidOperationException($"Не удалось получить список {typeof(T).Name}");

        return items;
    }

    public async Task<T?> GetByIdAsync(Guid Id, string? token, CancellationToken Cancel = default)
    {
        Http.SetBearerToken(token);
        var item = await Http.GetFromJsonAsync<T>($"{Address}/{Id}", Cancel).ConfigureAwait(false);
        return item;
    }

    public async Task<Guid?> AddAsync(K Item, string? token, CancellationToken Cancel = default)
    {
        Http.SetBearerToken(token);
        var response = await Http.PostAsJsonAsync($"{Address}/Add", Item, Cancel).ConfigureAwait(false);

        var result = await response
           .EnsureSuccessStatusCode()
           .Content
           .ReadFromJsonAsync<ResponseStatusResult>(cancellationToken: Cancel);

        return result?.Id;
    }
    public async Task<bool> UpdateAsync(Guid Id, K Item, string? token, CancellationToken Cancel = default)
    {
        Http.SetBearerToken(token);
        var response = await Http.PutAsJsonAsync($"{Address}/Edit/{Id}", Item, Cancel).ConfigureAwait(false);

        if (response.StatusCode == HttpStatusCode.NotFound)
            return false;

        return response.EnsureSuccessStatusCode().StatusCode == HttpStatusCode.OK;
    }

    public async Task<Guid?> RemoveAsync(Guid Id, string? token, CancellationToken Cancel = default)
    {
        Http.SetBearerToken(token);
        var response = await Http.DeleteAsync($"{Address}/Delete/{Id}", Cancel).ConfigureAwait(false);

        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;

        var result = await response
           .EnsureSuccessStatusCode()
           .Content
           .ReadFromJsonAsync<ResponseStatusResult>(cancellationToken: Cancel);

        return result?.Id;
    }
}

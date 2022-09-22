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
using FW.Domain;

namespace FW.WPF.WebAPI.Clients.Base;

public abstract class ClientBase<T> : IClientBase<T> where T : class,IEntity
{ 
    protected HttpClient Http { get; }

    protected string Address { get; }

    protected ClientBase(HttpClient Client, string Address)
    {
        Http = Client;
        this.Address = Address;
    }

    //public async Task<int> Count(CancellationToken Cancel = default)
    //{
    //    var count = await Http
    //       .GetFromJsonAsync<int>($"{Address}/count", Cancel)
    //       .ConfigureAwait(false);
    //    return count;
    //}

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
        var response = await Http.GetAsync($"{Address}/({Skip}:{Take})", Cancel).ConfigureAwait(false);

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

    public async Task<Guid> AddAsync(T Item, string? token, CancellationToken Cancel = default)
    {
        Http.SetBearerToken(token);
        var response = await Http.PostAsJsonAsync(Address, Item, Cancel).ConfigureAwait(false);

        var created_item = await response
           .EnsureSuccessStatusCode()
           .Content
           .ReadFromJsonAsync<T>(cancellationToken: Cancel);

        Item.Id = created_item!.Id;

        return Item.Id;
    }

    public async Task<bool> UpdateAsync(T Item, string? token, CancellationToken Cancel = default)
    {
        Http.SetBearerToken(token);
        var response = await Http.PutAsJsonAsync(Address, Item, Cancel).ConfigureAwait(false);

        if (response.StatusCode == HttpStatusCode.NotFound)
            return false;

        return response.EnsureSuccessStatusCode().StatusCode == HttpStatusCode.OK;
    }

    public async Task<T?> RemoveAsync(Guid Id, string? token, CancellationToken Cancel = default)
    {
        Http.SetBearerToken(token);
        var response = await Http.DeleteAsync($"{Address}/{Id}", Cancel).ConfigureAwait(false);

        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;

        var result = await response
           .EnsureSuccessStatusCode()
           .Content
           .ReadFromJsonAsync<T>(cancellationToken: Cancel);

        return result;
    }
}

using FW.WPF.WebAPI.Clients.Base;
using FW.Domain.Models;
using System.Net.Http;
using FW.WPF.Identity.Clients.Base;
using IdentityModel.Client;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Net.Http.Json;
using FW.WPF.WebAPI.Interfaces;

namespace FW.WPF.WebAPI.Clients;

public class DishesClient : ClientBase<DishResponseVM, DishVM>,IDishesClient
{
    public DishesClient(HttpClient Client) : base(Client, WebAPIAddress.Dishes) { }
    public async Task<IEnumerable<DishResponseVM>> GetByParentIdAsync(string token, CancellationToken Cancel = default)
    {
        Http.SetBearerToken(token);
        var response = await Http.GetAsync($"{Address}/GetByParentId", Cancel).ConfigureAwait(false);

        if (response.StatusCode == HttpStatusCode.NoContent) return Enumerable.Empty<DishResponseVM>();
        if (response.StatusCode == HttpStatusCode.NotFound) return Enumerable.Empty<DishResponseVM>();

        var items = await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadFromJsonAsync<IEnumerable<DishResponseVM>>(cancellationToken: Cancel)
            ?? throw new InvalidOperationException("Не удалось загрузить рецепт");

        return items!;
    }

    public async Task<bool> CookAsync(Guid Id, Guid WarehouseId, int NumPortions, string? token, CancellationToken Cancel = default) 
    {
        Http.SetBearerToken(token);
        var response = await Http.PutAsJsonAsync($"{Address}/Cook/{Id}:{WarehouseId}:{NumPortions}", new StringContent(""), Cancel).ConfigureAwait(false);
        // PutAsync($"https://localhost:2001/api/Dishes/Cook/{dishes[0].Id}?numPortions={numPortions}"
        if (response.StatusCode == HttpStatusCode.NotFound)
            return false;

        return response.EnsureSuccessStatusCode().StatusCode == HttpStatusCode.OK;
    }
}
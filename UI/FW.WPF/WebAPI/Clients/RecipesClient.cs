using FW.WPF.WebAPI.Clients.Base;
using FW.Domain.Models;
using System.Collections.Generic;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using FW.WPF.WebAPI.Interfaces;
using System.Threading;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using IdentityModel.Client;

namespace FW.WPF.WebAPI.Clients;

public class RecipesClient : ClientBase<RecipeResponseVM, RecipeVM> , IRecipesClient
{
    public RecipesClient(HttpClient Client) : base(Client, WebAPIAddress.Recipes) { }
    public async Task<IEnumerable<RecipeResponseVM>> GetByParentIdAsync(Guid ParentId, string token, CancellationToken Cancel = default)
    {
        Http.SetBearerToken(token);
        var response = await Http.GetAsync($"{Address}/dish/{ParentId}", Cancel).ConfigureAwait(false);

        if (response.StatusCode == HttpStatusCode.NoContent) return Enumerable.Empty<RecipeResponseVM>();
        if (response.StatusCode == HttpStatusCode.NotFound) return Enumerable.Empty<RecipeResponseVM>();

        var items = await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadFromJsonAsync<IEnumerable<RecipeResponseVM>>(cancellationToken: Cancel)
            ?? throw new InvalidOperationException("Не удалось загрузить список рецептов");

        return items!;
    }
}
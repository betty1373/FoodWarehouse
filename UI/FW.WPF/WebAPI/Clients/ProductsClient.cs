using FW.WPF.WebAPI.Clients.Base;
using FW.Models;
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

public class ProductsClient : ClientBase<ProductResponseVM,ProductVM> , IProductsClient
{
    public ProductsClient(HttpClient Client) : base(Client, WebAPIAddress.Products) { }
    public async Task<IEnumerable<ProductResponseVM>> GetByParentIdAsync(Guid ParentId, string token, CancellationToken Cancel = default)
    {
        Http.SetBearerToken(token);
        var response = await Http.GetAsync($"{Address}/GetByParentId/{ParentId}", Cancel).ConfigureAwait(false);

        if (response.StatusCode == HttpStatusCode.NoContent) return Enumerable.Empty<ProductResponseVM>();
        if (response.StatusCode == HttpStatusCode.NotFound) return Enumerable.Empty<ProductResponseVM>();

        var items = await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadFromJsonAsync<IEnumerable<ProductResponseVM>>(cancellationToken: Cancel)
            ?? throw new InvalidOperationException("Не удалось загрузить список продуктов");

        return items!;
    }
}
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

public class ChangesProductsClient : ClientBase<ChangesProductResponseVM, ChangesProductVM> , IChangesProductsClient
{
    public ChangesProductsClient(HttpClient Client) : base(Client, WebAPIAddress.ChangesProducts) { }
    public async Task<IEnumerable<ChangesProductResponseVM>> GetByParentIdAsync(Guid ParentId, string token, CancellationToken Cancel = default)
    {
        Http.SetBearerToken(token);
        var response = await Http.GetAsync($"{Address}/GetByParentId/{ParentId}", Cancel).ConfigureAwait(false);

        if (response.StatusCode == HttpStatusCode.NoContent) return Enumerable.Empty<ChangesProductResponseVM>();
        if (response.StatusCode == HttpStatusCode.NotFound) return Enumerable.Empty<ChangesProductResponseVM>();

        var items = await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadFromJsonAsync<IEnumerable<ChangesProductResponseVM>>(cancellationToken: Cancel)
            ?? throw new InvalidOperationException("Не удалось загрузить движение продукта");

        return items!;
    }
}
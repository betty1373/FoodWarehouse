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

public class WarehouseClient : ClientBase<WarehouseResponseVM, WarehouseVM> , IWarehouseClient
{
    public WarehouseClient(HttpClient Client) : base(Client, WebAPIAddress.Warehause) { }
    public async Task<WarehouseResponseVM> GetByParentIdAsync(string token, CancellationToken Cancel = default)
    {
        Http.SetBearerToken(token);
        var response = await Http.GetFromJsonAsync<WarehouseResponseVM>($"{Address}/GetByParentId", Cancel).ConfigureAwait(false);
        return response;
    }
}
using FW.WPF.WebAPI.Clients.Base;
using FW.Domain.Models;
using System.Net.Http;
using FW.WPF.Identity.Clients.Base;

namespace FW.WPF.WebAPI.Clients;

public class CategoriesClient : ClientBase<CategoryResponseVM>
{
    public CategoriesClient(HttpClient Client) : base(Client, WebAPIAddress.Categories) { }
}
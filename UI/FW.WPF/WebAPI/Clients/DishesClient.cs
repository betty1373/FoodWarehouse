using FW.WPF.WebAPI.Clients.Base;
using FW.Domain.Models;
using System.Net.Http;
using FW.WPF.Identity.Clients.Base;

namespace FW.WPF.WebAPI.Clients;

public class DishesClient : ClientBase<DishResponseVM>
{
    public DishesClient(HttpClient Client) : base(Client, WebAPIAddress.Dishes) { }
}
using FW.WPF.WebAPI.Clients.Base;
using FW.Models;
using System.Net.Http;
using FW.WPF.Identity.Clients.Base;

namespace FW.WPF.WebAPI.Clients;

public class IngredientsClient : ClientBase<IngredientResponseVM, IngredientVM>
{
    public IngredientsClient(HttpClient Client) : base(Client, WebAPIAddress.Ingredients) { }
}
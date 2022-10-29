using FW.Models;
using FW.ResponseStatus;
using IdentityModel.Client;
using System.Net.Http.Json;
Thread.Sleep(TimeSpan.FromSeconds(5));

var identityClient = new HttpClient();

// Получаем документ обнаружения
var disco = await identityClient.GetDiscoveryDocumentAsync("https://fwidentity:10001");
if (disco.IsError)
{
    // Note: Если вы получаете сообщение об ошибке при подключении, возможно, вы используете https, а сертификат разработки localhostне является доверенным.
    // Вы можете запустить , чтобы доверять сертификату разработки. Это нужно сделать только один раз: .dotnet dev-certs https --trust
    PrintLn(disco.Error,ConsoleColor.Red);
    return;
}

#region Регистрация пользователя
/*
// Регистрация нового пользователя в микросервисе FW.Identity
PrintLn("Регистрация нового пользователя:", ConsoleColor.Blue);
var newUser = new RegisterViewModel();

Print("Введите login: ");
newUser.Username = Console.ReadLine();

Print("Введите email: ");
newUser.Email = Console.ReadLine();

Print("Введите пароль: ");
newUser.Password = Console.ReadLine();

Print("Повторно введите пароль: ");
newUser.ConfirmPassword = Console.ReadLine();

if(newUser.Password != newUser.ConfirmPassword)
{
PrintLn("Пароли не совпадают!", ConsoleColor.Red);
goto auth;
}

var signup = await identityClient.PostAsJsonAsync("https://localhost:10001/api/auth/Register", newUser);
if (signup.IsSuccessStatusCode)
{
Print("Пользователь ");
Print(newUser.Username, ConsoleColor.Green);
PrintLn(" зарегестрирован!");
}
else
PrintLn(signup.StatusCode.ToString(), ConsoleColor.Red);

PrintLn();
*/
#endregion


PrintLn("Аутентификация пользователя:", ConsoleColor.Blue);
/*Print("Введите login: ");
var login = Console.ReadLine();

Print("Введите пароль: ");
var password = Console.ReadLine();*/

var login = "Ivan";
var password = "i123456789";

#region Аутентификуация пользователя
// Аутентификуация пользователя в микросервисе FW.Identity
var tokenResponse = await identityClient.RequestPasswordTokenAsync(new PasswordTokenRequest
{
    // Эндпоинт выдачи токена
    Address = disco.TokenEndpoint,

    // указываем параметры зарегестрированного клиента в микросервисе FW.Identity
    ClientId = "clientConsole",
    ClientSecret = "42467dee-f65d-481c-9508-74891854ddaa",
    Scope = "scopeWebAPI",

    // указываем параметры зарегестрированного пользователя
    UserName = login,
    Password = password
});
if (tokenResponse.IsError)
{
    PrintLn(tokenResponse.Error, ConsoleColor.Red);
    return;
}

Print("Привет ");
Print($"{login}", ConsoleColor.Green);
PrintLn("!\r\n");
#endregion


// Получаем информацию из микросервиса FW.Managment
var apiClient = new HttpClient();
// Устанавливаем токен для авторизации
apiClient.SetBearerToken(tokenResponse.AccessToken);

// Запрос списка складов пользователя
var listWarehouse = await apiClient.GetAsync("https://localhost:2001/api/Warehouses/Get");

PrintLn("Информация о складе", ConsoleColor.Blue);
if (listWarehouse.IsSuccessStatusCode)
{
    var warehouse = await listWarehouse.Content.ReadFromJsonAsync<WarehouseResponseVM>();

    PrintLn($"\tНаименование: {warehouse.Name}");
    PrintLn($"\tАдрес: {warehouse.Address}");

    PrintLn();
}
else
    PrintLn(listWarehouse.StatusCode.ToString(), ConsoleColor.Red);

// Запрос списка блюд пользователя
var listDishes = await apiClient.GetAsync("https://localhost:2001/api/Dishes/GetAll");
List<DishResponseVM> dishes = null;
PrintLn("Список блюд:", ConsoleColor.Blue);
if (listDishes.IsSuccessStatusCode)
{
    dishes = await listDishes.Content.ReadFromJsonAsync<List<DishResponseVM>>();

    int i = 0;
    foreach (var dish in dishes)
    {
        PrintLn($"{++i}.");
        PrintLn($"\tНаименование: {dish.Name}");
        PrintLn($"\tОписание: {dish.Description}");
    }
    PrintLn();
}
else
    PrintLn(listDishes.StatusCode.ToString(), ConsoleColor.Red);


// Запрос списка продуктов на складе
await GetListProducts(apiClient);

// Приготовление блюда
var numPortions = 3;
var cookDish = await apiClient.PutAsync($"https://localhost:2001/api/Dishes/Cook/{dishes[0].Id}?numPortions={numPortions}", new StringContent(""));

PrintLn($"Приготовление блюда \"{dishes[0].Name}\"...", ConsoleColor.Blue);
PrintLn($"Кол-во порций: {numPortions}", ConsoleColor.Blue);
if (cookDish.IsSuccessStatusCode)
{
    var result = await cookDish.Content.ReadFromJsonAsync<ResponseStatusResult>();

    PrintLn($"Результат: {result.Title}");
    PrintLn();
}
else
    PrintLn(cookDish.StatusCode.ToString(), ConsoleColor.Red);
PrintLn();

await GetListProducts(apiClient);

Console.ReadLine();



static async Task GetListProducts(HttpClient apiClient)
{
    // Запрос списка продуктов на складе
    var listProducts = await apiClient.GetAsync("https://localhost:2001/api/Products/GetAll");
    PrintLn("Список продуктов на складе:", ConsoleColor.Blue);
    if (listProducts.IsSuccessStatusCode)
    {
        var products = await listProducts.Content.ReadFromJsonAsync<List<ProductResponseVM>>();
        var sortedProducts = products?.OrderBy(x => x.IngredientName);

        int i = 0;
        foreach (var pr in sortedProducts)
        {
            PrintLn($"{++i}.");
            PrintLn($"\tНаименование: {pr.IngredientName}");
            PrintLn($"\tКоличество: {pr.Quantity} гр.");
            PrintLn($"\tГоден до: {pr.ExpirationDate.Date:d}");
        }
        PrintLn();
    }
    else
        PrintLn(listProducts.StatusCode.ToString(), ConsoleColor.Red);
}

static void Print(string text, ConsoleColor color = ConsoleColor.White)
{
    Console.ForegroundColor = color;
    Console.Write(text);
    Console.ForegroundColor = ConsoleColor.White;
}

static void PrintLn(string text = "", ConsoleColor color = ConsoleColor.White)
{
    Print($"{text}\r\n", color);
}
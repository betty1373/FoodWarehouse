using ConsoleClient.ViewModels;
using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System.Net.Http.Json;

Thread.Sleep(TimeSpan.FromSeconds(5));

var identityClient = new HttpClient();

// Получаем документ обнаружения
var disco = await identityClient.GetDiscoveryDocumentAsync("https://localhost:10001");
if (disco.IsError)
{
    // Note: Если вы получаете сообщение об ошибке при подключении, возможно, вы используете https, а сертификат разработки localhost не является доверенным.
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


auth:
while (true)
{
    PrintLn("Аутентификация пользователя:", ConsoleColor.Blue);
    Print("Введите login: ");
    var login = Console.ReadLine();

  Print("Введите пароль: ");
    var password = Console.ReadLine();

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
    var listWarehouse = await apiClient.GetAsync("https://localhost:2001/api/Warehouses/GetByParentId");

    PrintLn("Информация о складе", ConsoleColor.Blue);
    if (listWarehouse.IsSuccessStatusCode)
    {
        var content = await listWarehouse.Content.ReadAsStringAsync();
        var warehouse = (JObject.Parse(content)).ToObject<WarehouseVM>();

        PrintLn($"\tНаименование: {warehouse.Name}");
        PrintLn($"\tАдрес: {warehouse.Address}");

        PrintLn();
    }
    else
        PrintLn(listWarehouse.StatusCode.ToString(), ConsoleColor.Red);

    // Запрос списка блюд пользователя
    var listDishes = await apiClient.GetAsync("https://localhost:2001/api/Dishes/GetAll");

    PrintLn("Список блюд:", ConsoleColor.Blue);
    if (listDishes.IsSuccessStatusCode)
    {
        var content = await listDishes.Content.ReadAsStringAsync();
        var dishes = (JArray.Parse(content)).ToObject<List<DishVM>>();

        int i = 0;
        foreach (DishVM dish in dishes)
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
    var listProducts = await apiClient.GetAsync("https://localhost:2001/api/Products/GetAll");
    PrintLn("Список продуктов на складе:", ConsoleColor.Blue);
    if (listProducts.IsSuccessStatusCode)
    {
        var content = await listProducts.Content.ReadAsStringAsync();
        var products = (JArray.Parse(content)).ToObject<List<ProductVM>>();

        int i = 0;
        foreach (ProductVM pr in products)
        {
            PrintLn($"{++i}.");
            PrintLn($"\tНаименование: {pr.Name}");
            PrintLn($"\tКоличество: {pr.Quantity} гр.");
        }
        PrintLn();
    }
    else
        PrintLn(listProducts.StatusCode.ToString(), ConsoleColor.Red);

    PrintLn();
}
Console.ReadLine();


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
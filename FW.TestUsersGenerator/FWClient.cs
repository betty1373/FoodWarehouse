
using FW.TestUsersGenerator.Models;
using IdentityModel.Client;
using Serilog;
using System.Net.Http.Json;
using FW.ResponseStatus;
using FW.Models;

namespace FW.TestUsersGenerator
{
    public class FWClient : IDisposable
    {
        private UserData _user;
        private readonly HttpClient _httpClient;
        private readonly string _tokenEndpoint;

        /// <summary>
        /// Показать сообщения
        /// </summary>
        public event Action<string> DisplayMessage;
        public event Action<string> DisplayError;

        public int NumUser;
        public UserData User { get { return _user; } }

        public FWClient(string tokenEndpoint)
        {
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromMinutes(5);

            _tokenEndpoint = tokenEndpoint;
        }
        public FWClient(UserData user, string tokenEndpoint):this(tokenEndpoint)
        {
            _user = user;
        }

        // Регистрация нового пользователя в микросервисе FW.Identity
        public async Task<StatusResult> RegisterUserAsync(UserData user)
        {
            _user = user;

            var uri = "https://localhost:10001/api/auth/Register";
            var textMsg = $"{NumUser}. [{_user.Username}]. ";

            var newUser = new RegisterViewModel()
            {
                Username = _user.Username,
                Email = _user.Email,
                Password = _user.Password,
                ConfirmPassword = _user.Password
            };

            for (int a = 1; a < 6; a++)
            {
                try
                {
                    var signup = await _httpClient.PostAsJsonAsync(uri, newUser);
                    if (signup.IsSuccessStatusCode)
                    {
                        var text = textMsg + $"Попытка {a}. Регистрация выполнена!";
                        DisplayMessage?.Invoke(text);
                        Log.Information(text);
                        return StatusResult.Ok;
                    }
                    else
                    {
                        var text = textMsg + $"Попытка {a}. Регистрация. Статус: {signup.StatusCode}";
                        DisplayError?.Invoke(text);
                        Log.Error(text);
                        return StatusResult.Conflict;
                    }
                }
                catch (Exception ex)   //The request was canceled due to the configured HttpClient.Timeout of 60 seconds elapsing.
                {
                    var text = textMsg + $"Попытка {a}. Ошибка регистрации: {ex.Message + ex.StackTrace}";
                    DisplayError?.Invoke(text);
                    Log.Error(text);
                }
            }

            return StatusResult.Error;
        }

        // Аутентификация нового пользователя в микросервисе FW.Identity
        public async Task<bool> AuthentificationUserAsync()
        {
            var textMsg = $"{NumUser}. [{_user.Username}]. ";

            for (int a = 1; a < 6; a++)
            {
                try
                {
                    var tokenResponse = await _httpClient.RequestPasswordTokenAsync(
                        new PasswordTokenRequest
                        {
                            // Эндпоинт выдачи токена
                            Address = _tokenEndpoint,

                            // указываем параметры зарегестрированного клиента в микросервисе FW.Identity
                            ClientId = "clientConsole",
                            ClientSecret = "42467dee-f65d-481c-9508-74891854ddaa",
                            Scope = "scopeWebAPI",

                            // указываем параметры зарегестрированного пользователя
                            UserName = _user.Username,
                            Password = _user.Password
                        });

                    if (tokenResponse.IsError)
                    {
                        var text = textMsg + $"Попытка {a}. Аутентификуации. Статус: {tokenResponse.Error}";
                        DisplayError?.Invoke(text);
                        Log.Error(text);
                    }

                    _httpClient.SetBearerToken(tokenResponse.AccessToken);
                    DisplayMessage?.Invoke(textMsg + $"Попытка {a}. Аутентификуация выполнена.");
                    return true;
                }
                catch (Exception ex)
                {
                    var text = textMsg + $"Попытка {a}. Ошибка аутентификуации: {ex.Message + ex.StackTrace}";
                    DisplayError?.Invoke(text);
                    Log.Error(text);
                }
            }

            return false;
        }

        // Добавление склада
        public async Task<bool> WarehouseAddAsync()
        {
            var uri = "https://localhost:2001/api/Warehouses/Add/";
            var textMsg = $"{NumUser}. [{_user.Username}]. ";

            for (int a = 1; a < 6; a++)
            {
                try
                {
                    var createWarehouse = await _httpClient.PostAsJsonAsync(uri, 
                        new WarehouseVM
                        {
                            Name = _user.WarehouseName,
                            Address = _user.WarehouseAddress
                        });

                    if (createWarehouse.IsSuccessStatusCode)
                    {
                        var result = await createWarehouse.Content.ReadFromJsonAsync<ResponseStatusResult>();

                        DisplayMessage?.Invoke(textMsg + $"Попытка {a}. Добавлен склад [{_user.WarehouseName}]!");
                        return true;
                    }

                    var text = textMsg + $"Попытка {a}. Добавление склада. Статус: {createWarehouse.StatusCode}";
                    DisplayError?.Invoke(text);
                    Log.Error(text);
                }
                catch (Exception ex)
                {
                    var text = textMsg + $"Попытка {a}. Ошибка добавления склада: {ex.Message + ex.StackTrace}";
                    DisplayError?.Invoke(text);
                    Log.Error(text);
                }
            }

            return false;
        }    

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}

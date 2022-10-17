using FW.TestUsersGenerator.Models;
using FW.TestUsersGenerator.Generators;
using FW.TestUsersGenerator.Infrastructure;
using Serilog;
using Serilog.Formatting.Compact;
using FW.ResponseStatus;

using IdentityModel.Client;
using System.Net.Http.Json;
using System.Diagnostics;

namespace FW.TestUsersGenerator
{
    internal class Program
    {
        private static int _numUsers = 1000;
        private static string _filePath = $"{AppDomain.CurrentDomain.BaseDirectory}";

        /// <summary>
        /// Генерация тестовых пользователей, регистрация их в IdentityServer FoodWarwhouse, добавление склада
        /// </summary>
        /// <param name="args">
        /// args[0] - numUsers - количество пользователей, 
        /// args[1] - filePath путь для сохранения файла с данными зарегестрированных пользователей
        /// </param>
        static void Main(string[] args)     
        {
            try
            {
                Log.Logger = new LoggerConfiguration()
                .WriteTo.File(new CompactJsonFormatter(), "Log_testUsersGenerator.clef")
                .CreateLogger();

                var text = $"Started test user generator.";
                Log.Information(text);
                Console.WriteLine(text);

                // проверка входных аргументов
                TryValidateAndParseArgs(args);

                // Получение документа обнаружения IdentityServer4
                var discoDoc = GetDiscoveryDocument();

                // регистрация случайного пользователя и создание склада продуктов через Api
                var users = new List<UserData>();
                var countdownEvent = new CountdownEvent(_numUsers);
                var num = 0;

                var stopWatch = new Stopwatch();
                stopWatch.Start();
                Parallel.For(0, _numUsers, new ParallelOptions { MaxDegreeOfParallelism = 10 }, x =>
                {
                    var fwClient = new FWClient(discoDoc.TokenEndpoint);
                    fwClient.DisplayMessage += FwClient_DisplayMessage;
                    fwClient.DisplayError += FwClient_DisplayError;
                    fwClient.NumUser = ++num;

                    for (int a = 0; a < 5; a++)
                    {
                        var fakeUser = RandomUserGenerator.Generate();
                        var isRegistred = fwClient.RegisterUserAsync(fakeUser).GetAwaiter().GetResult();
                        if (isRegistred == StatusResult.Ok)
                        {
                            if (!fwClient.AuthentificationUserAsync().GetAwaiter().GetResult())
                                break;

                            if(fwClient.WarehouseAddAsync().GetAwaiter().GetResult())
                                users.Add(fwClient.User);

                            break;
                        }
                        else if (isRegistred == StatusResult.Conflict)    // если пользователь уже существует, генерируем другого
                            continue;
                        else if (isRegistred == StatusResult.Error)
                            break;
                    }

                    fwClient.Dispose();
                    countdownEvent.Signal();
                });

                countdownEvent.Wait();

                stopWatch.Stop();
                countdownEvent.Dispose();


                var csvFile = new CsvGenerator();
                _filePath += $"fakeUsers_{DateTime.Now:yyyy.MM.dd_HH-mm-ss}.csv";
                csvFile.Generate(_filePath, users);

                ConsoleHelper.WriteLine($"Время выполнения: [{stopWatch.Elapsed:hh\\:mm\\:ss}]");
                ConsoleHelper.WriteLine($"Количество зарегистрированых пользователей: [{users.Count()}]");
                ConsoleHelper.WriteLine($"Файл с данными пользователей: [{_filePath}]");
            }
            catch(Exception ex)
            {
                ConsoleHelper.WriteLineError(ex.Message);
            }
            finally
            {
                Log.CloseAndFlush();
                Console.ReadKey();
            }
        }

        private static void FwClient_DisplayError(string text)
        {
            ConsoleHelper.WriteLineError(text);
        }

        private static void FwClient_DisplayMessage(string text)
        {
            ConsoleHelper.WriteLine(text);
        }

        // проверка входящих аргументов
        private static void TryValidateAndParseArgs(string[] args)
        {
            if (args == null || args.Length == 0)
                return;

            if (!int.TryParse(args[0], out _numUsers))
            {
                var text = $"Invalid configuration parametr \"numUsers\". It's must be from \"1\" to \"{int.MaxValue}\"";
                Console.WriteLine(text);
                Log.Information(text);
                return;
            }

            if (args.Length > 1)
                _filePath = $"{args[1]}";
        }

        // Получение документ обнаружения IdentityServer4
        private static DiscoveryDocumentResponse GetDiscoveryDocument()
        {
            using var identityClient = new HttpClient();

            var disco = identityClient.GetDiscoveryDocumentAsync("https://localhost:10001");
            if (disco.Result.IsError)
            {
                var text = disco.Result.Error;
                ConsoleHelper.WriteLineError(text);
                Log.Error(text);
                throw new Exception(text);
            }

            return disco.Result;
        }
    }
}
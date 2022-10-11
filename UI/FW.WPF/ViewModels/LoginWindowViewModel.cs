using System;
using System.Windows.Input;
using FW.WPF.Commands;
using FW.WPF.Infrastructure;
using FW.WPF.ViewModels.Base;
using FW.WPF.Identity.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using FW.WPF.Domain.Exceptions;
using FW.WPF.WebAPI.Interfaces;
using FW.WPF.Models;
namespace FW.WPF.ViewModels;

public class LoginWindowViewModel : ViewModel
{
    private readonly IWarehouseClient _WarehouseClient;
    private readonly IClientIdentity<LoginModel> _ClientIdentity;
    public event EventHandler<EventArgs<ViewModel>>? Login;
    protected virtual void OnLoging(ViewModel model) => Login?.Invoke(this, model);
    public LoginWindowViewModel()//IClientIdentity<LoginModel> clientIdentity)
    {
        _ClientIdentity = App.Services.GetRequiredService<IClientIdentity<LoginModel>>();
        _WarehouseClient = App.Services.GetRequiredService<IWarehouseClient>();
        ErrorMessageViewModel = new MessageViewModel();
        LoginModel = App.Services.GetRequiredService<LoginModel>();
    }
    #region Title : string - Заголовок главного окна

    /// <summary>Заголовок главного окна</summary>
    private string _Title = "Вход в систему";

    /// <summary>Заголовок главного окна</summary>
    public string Title { get => _Title; set => Set(ref _Title, value); }

    #endregion
    private LoginModel? _LoginModel;

    /// <summary>Данные пользователя</summary>
    public LoginModel? LoginModel
    {
        get => _LoginModel;
        set
        {
            if (!Set(ref _LoginModel, value)) return;
        }
    }
    public MessageViewModel ErrorMessageViewModel { get; }

    public string ErrorMessage
    {
        set => ErrorMessageViewModel.Message = value;
    }
    #region Command LoginCommand - Вход в систему

    /// <summary>Вход в систему</summary>
    private LambdaCommand? _LoginCommand;

    /// <summary>Вход в систему</summary>
    public ICommand LoginCommand => _LoginCommand
        ??= new(OnLoginCommandExecuted, p => p is LoginModel);

    /// <summary>Логика выполнения - Вход в систему</summary>
    private async void OnLoginCommandExecuted(object? p)
    {
        var login = (LoginModel?)p;
        if (login?.UserName is not string { Length: > 0 } user_name) return;
        if (login?.Password is not string { Length: > 0 } password) return;

        try
        {
            var disco = await _ClientIdentity.GetDiscoveryDocumentAsync();
         //   if (disco is not string { Length: > 0 }) throw new IdentityServerNotFoundException(_ClientIdentity.);

            var token = await _ClientIdentity.RequestPasswordTokenAsync(login, disco);
            //if (token is not string { Length: > 0 }) return;
            login.AccessToken = token;
            if (login?.AccessToken is not string { Length: > 0 } accesstoken) return;

            var warehouse = await _WarehouseClient.GetByParentIdAsync(login?.AccessToken ?? "");
            if (warehouse?.Id is not Guid id) return; 
            login.WarehouseId = warehouse.Id;
            login.WarehouseName = warehouse.Name;
            login.WarehouseAddress = warehouse.Address;
            
        }
        catch (IdentityServerNotFoundException ex)
        {
            ErrorMessage = $"Can't connect IdentityServer {ex.IdentityServer}.";
            return;
        }
        catch (UserNotFoundException)
        {
            ErrorMessage = "Username does not exist.";
            return;
        }
        catch (InvalidPasswordException ex)
        {
            ErrorMessage = $"Incorrect password {ex.Username}";
            return;
        }
        catch (Exception)
        {
            ErrorMessage = "Login failed.";
            return;
        }
        login.IsLogin = true;
        OnLoging(login);
    }

    #endregion
}

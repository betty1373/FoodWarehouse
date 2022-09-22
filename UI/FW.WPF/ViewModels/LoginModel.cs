using System;
using System.Windows.Input;
using FW.WPF.Commands;
using FW.WPF.Infrastructure;
using FW.WPF.ViewModels.Base;
using FW.WPF.Identity.Interfaces;

namespace FW.WPF.ViewModels;

public class LoginModel:ViewModel,IUserIdentity
{
    #region UserName : string? - Имя пользователя
    /// <summary>Имя пользователя</summary>
    private string? _UserName = "Ivan";
    /// <summary>Имя пользователя</summary>
    public string? UserName { get => _UserName; set => Set(ref _UserName, value); }
    private string? _Password = "i123456789";
    public string? Password { get => _Password; set => Set(ref _Password, value); }
    private string? _AccessToken;
    public string? AccessToken{ get => _AccessToken; set => Set(ref _AccessToken, value); }
    #endregion
}
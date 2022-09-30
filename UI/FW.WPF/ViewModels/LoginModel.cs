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
    private Guid? _WarehauseId;
    public Guid? WarehauseId { get => _WarehauseId; set => Set(ref _WarehauseId, value); }
    private string? _WarehauseName;
    public string? WarehauseName { get => _WarehauseName; set => Set(ref _WarehauseName, value); }
    private string? _WarehauseAddress;
    public string? WarehauseAddress { get => _WarehauseAddress; set => Set(ref _WarehauseAddress, value); }
    public override string ToString()
    {
        return $"User: {UserName} Склад: {WarehauseName},{WarehauseAddress}";
    }

    #endregion
}
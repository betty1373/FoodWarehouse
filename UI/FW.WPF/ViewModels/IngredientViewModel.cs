using System;
using System.Windows.Input;
using FW.WPF.Commands;
using FW.WPF.Infrastructure;
using FW.WPF.ViewModels.Base;
using FW.WPF.Identity.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using FW.WPF.Domain.Exceptions;
using FW.WPF.WebAPI.Interfaces;
using FW.Models;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Xml.Linq;
using System.Linq;
using FW.WPF.Models;
using FW.WPF.Views.Windows;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Net.Http;
using FW.WPF.WebAPI.Clients;
namespace FW.WPF.ViewModels;

public class IngredientViewModel : ViewModel
{
    private bool _isFormVisible=false;
    public bool IsFormVisible { get => _isFormVisible; set => Set(ref _isFormVisible, value); }

    private LoginModel? LoginModel { get; } = null!;
    private readonly IClientBase<IngredientResponseVM, IngredientVM> _IngredientsClient;
    
    public IngredientViewModel(LoginModel? loginModel)
    {
        LoginModel = loginModel;
        _IngredientsClient = App.Services.GetRequiredService<IClientBase<IngredientResponseVM, IngredientVM>>();
        RefreshCommand.Execute(this);   
        ErrorMessageViewModel = new MessageViewModel();
       
    }
    private IEnumerable<IngredientResponseVM>? _Ingredients;
    public IEnumerable<IngredientResponseVM>? Ingredients
    {
        get => _Ingredients;

        private set
        {
            if (!Set(ref _Ingredients, value)) return;
        }
    }
    public MessageViewModel ErrorMessageViewModel { get; }
    private IngredientModel? _IngredientModel = new();

    /// <summary>Данные пользователя</summary>
    public IngredientModel? IngredientModel
    {
        get => _IngredientModel;

        set
        {
            if (!Set(ref _IngredientModel, value)) return;
        }
    }
    private LambdaCommand _RefreshCommand;
    public ICommand RefreshCommand => _RefreshCommand ?? (_RefreshCommand = new(ExecuteRefreshCommand));

    private void ExecuteRefreshCommand()
    {
       GetIngredientsAsync(LoginModel?.AccessToken).Await(Completed, HandleEror);
    }

    private void Completed()
    {
        ErrorMessage = "Recipes loaded";
    }
    private void HandleEror(Exception ex)
    {
        ErrorMessage = ex.ToString();
    }
    public async Task GetIngredientsAsync(string token)
    {
        var items = await _IngredientsClient.GetAllAsync(token);
        Ingredients = items;
    }
    public string ErrorMessage
    {
        set => ErrorMessageViewModel.Message = value;
    }
    /// <summary>Заголовок главного окна</summary>
    private string _Title = "Вход в систему";

    /// <summary>Заголовок главного окна</summary>
    public string Title { get => _Title; set => Set(ref _Title, value); }

    private ICommand _saveCommand;
    private ICommand _resetCommand;
    private ICommand _editCommand;
    private ICommand _deleteCommand;
    public ICommand ResetCommand
    {
        get
        {
            if (_resetCommand == null)
                _resetCommand = new RelayCommand(param => Reset(), null);

            return _resetCommand;
        }
    }

    public ICommand SaveCommand
    {
        get
        {
            if (_saveCommand == null)
                _saveCommand = new RelayCommand(param => Save().Await(Reset,HandleEror), null);

            return _saveCommand;
        }
    }

    public ICommand EditCommand
    {
        get
        {
            if (_editCommand == null)
                _editCommand = new RelayCommand(param => Edit((Guid)param), null);

            return _editCommand;
        }
    }

    public ICommand DeleteCommand
    {
        get
        {
            if (_deleteCommand == null)
                _deleteCommand = new RelayCommand((param) => Delete((Guid)param), null);

            return _deleteCommand;
        }
    }
    public void Reset()
    {
       IngredientModel.Id = Guid.Empty;
       IngredientModel.Name = String.Empty;
    }
    public void Edit(Guid id)
    {
        var model = Ingredients?.Where(x => x.Id.Equals(id)).FirstOrDefault();
        IngredientModel.Id = model.Id;
        IngredientModel.Name = model.Name;
    }
    public async Task Save()
    {
        if (IngredientModel != null)
        {
            var item = new IngredientVM
            {
                Name = IngredientModel.Name
            };
            if (IngredientModel.Id.Equals(Guid.Empty))
            {
                var result = await _IngredientsClient.AddAsync(item, LoginModel?.AccessToken);
                Ingredients = new List<IngredientResponseVM>(Ingredients!) { new IngredientResponseVM
                 {
                       Id = result ?? System.Guid.Empty,
                       Name = item.Name
                 } };
            }
            else
            {
                var result = await _IngredientsClient.UpdateAsync(IngredientModel.Id, item, LoginModel?.AccessToken);
                Ingredients = Ingredients?.Where(c => !c.Id.Equals(IngredientModel.Id)).ToArray();
                Ingredients = new List<IngredientResponseVM>(Ingredients!) { new IngredientResponseVM
                 {
                       Id = IngredientModel.Id,
                       Name = item.Name
                }
                };
            }
        }
    }
    public async void Delete(Guid id)
    {
        if (MessageBox.Show("Confirm delete of this record?", "Recipe", MessageBoxButton.YesNo)
            == MessageBoxResult.Yes)
        {
            try
            {
                var result = await _IngredientsClient.RemoveAsync(id, LoginModel?.AccessToken);
                MessageBox.Show("Record successfully deleted.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occured while saving. " + ex.InnerException);
            }
            finally
            {
                Ingredients = Ingredients?.Where(c => !c.Id.Equals(id)).ToArray();
            }
        }
    }
}
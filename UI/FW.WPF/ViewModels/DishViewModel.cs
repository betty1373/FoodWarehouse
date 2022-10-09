using System;
using System.Windows.Input;
using FW.WPF.Commands;
using FW.WPF.Infrastructure;
using FW.WPF.ViewModels.Base;
using FW.WPF.Identity.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using FW.WPF.Domain.Exceptions;
using FW.WPF.WebAPI.Interfaces;
using FW.Domain.Models;
using System.Collections.Generic;
using FW.Domain;
using System.Threading;
using System.Windows;
using System.Xml.Linq;
using System.Linq;
using FW.WPF.Models;
using FW.WPF.Views.Windows;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Net.Http;

namespace FW.WPF.ViewModels;

public class DishViewModel : ViewModel
{
    #region SelectedDish : DishViewModel? - Выбранное блюдо
    /// <summary>Выбранное блюдо</summary>
    private DishModel? _SelectedDish;
    /// <summary>Выбранное блюдо</summary>
    public DishModel? SelectedDish
    {
        get => _SelectedDish;
        set
        {
            if (!Set(ref _SelectedDish, value)) return;
            if (RecipesModel is  null)
            {
                RecipesModel = new RecipeViewModel(LoginModel, this);
                RecipesModel.GetRecipesAsync(LoginModel.AccessToken).Await(Completed, HandleEror);
            }
        }
    }
    #endregion
    private LambdaCommand _RefreshCommand;
    public ICommand RefreshCommand => _RefreshCommand ?? (_RefreshCommand = new (ExecuteRefreshCommand,p => p is string { Length: > 0 }));

    private void ExecuteRefreshCommand(object? p)
    {
            if (p is not string {Length: > 0} token)
            {
                Dishes =  null;
                return;
            }
            GetDishesAsync(token).Await(Completed, HandleEror);
    }
    private RecipeViewModel? _RecipesModel;
    public RecipeViewModel? RecipesModel { get => _RecipesModel; set => Set(ref _RecipesModel, value); }
    private LoginModel? LoginModel { get; } = null!;
    private readonly IDishesClient _DishesClient;
   
    public DishViewModel(LoginModel? loginModel)//IClientIdentity<LoginModel> clientIdentity)
    {
        this.LoginModel = loginModel;
        _DishesClient = App.Services.GetRequiredService<IDishesClient>();
        ErrorMessageViewModel = new MessageViewModel();
       
    }
    private IEnumerable<DishModel>? _Dishes;
    public IEnumerable<DishModel>? Dishes
    {
        get => _Dishes;

        private set
        {
            if (!Set(ref _Dishes, value)) return;
        }
    }
    public MessageViewModel ErrorMessageViewModel { get; }
    private DishModel? _DishModel = new();

    /// <summary>Данные пользователя</summary>
    public DishModel? DishModel
    {
        get => _DishModel;

        set
        {
            if (!Set(ref _DishModel, value)) return;
        }
    }
    private void Completed()
    {
        ErrorMessage = "Dishes loaded";
    }
    private void HandleEror(Exception ex)
    {
        ErrorMessage = ex.ToString();
    }
    private async Task GetDishesAsync(string token)
    {
        var items = await _DishesClient.GetByParentIdAsync(token);
        Dishes = items.Select(dish => new DishModel
        {
            Id = dish.Id,
            Name = dish.Name,
            Description = dish.Description,
        }).ToArray();
        SelectedDish = Dishes.FirstOrDefault();
        OnPropertyChanged(nameof(SelectedDish));
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
        DishModel.Name = string.Empty;
        DishModel.Id = Guid.Empty;
        DishModel.Description = string.Empty;
    }
    public void Edit(Guid id)
    {
        var model = Dishes?.Where(x => x.Id.Equals(id)).FirstOrDefault();
        DishModel.Id = model.Id;
        DishModel.Name = model.Name;
        DishModel.Description = model.Description;
    }
    public async Task Save()
    {
        if (DishModel!= null)
        {
            var new_Dish = new DishVM
            {
                Name = DishModel.Name,
                Description = DishModel.Description
            };
            
            var result = await _DishesClient.AddAsync(new_Dish, LoginModel.AccessToken);

          //   Dishes = Dishes?.Where(c => !c.Id.Equals(DishModel.Id)).ToArray();
            Dishes = new List<DishModel>(Dishes!) { new DishModel
             {
                   Id = result ?? System.Guid.Empty,
                   Name = new_Dish.Name,
                   Description = new_Dish.Description,
             } };             
        }
    }
    public async void Delete(Guid id)
    {
        if (MessageBox.Show("Confirm delete of this record?", "Dish", MessageBoxButton.YesNo)
            == MessageBoxResult.Yes)
        {
            try
            {
                var result = await _DishesClient.RemoveAsync(id, LoginModel.AccessToken);
                MessageBox.Show("Record successfully deleted.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occured while saving. " + ex.InnerException);
            }
            finally
            {
                Dishes = Dishes?.Where(c => !c.Id.Equals(id)).ToArray();
            }
        }
    }
}
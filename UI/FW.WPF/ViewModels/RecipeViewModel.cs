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

public class RecipeViewModel : ViewModel
{
    private bool _isFormVisible;
    public bool IsFormVisible { get => _isFormVisible; set => Set(ref _isFormVisible, value); }
    private LoginModel? LoginModel { get; } = null!;
  
  
    private readonly IRecipesClient _RecipesClient;
    public RecipeViewModel(LoginModel? loginModel)
    {
        LoginModel = loginModel;
        _RecipesClient = App.Services.GetRequiredService<IRecipesClient>();    
        ErrorMessageViewModel = new MessageViewModel();
      
    }

    public RecipeResponseVM? _SelectedRecipe;
    public RecipeResponseVM? SelectedRecipe
    {
        get => _SelectedRecipe;
        set
        {
            if (!Set(ref _SelectedRecipe, value)) return;
                  
        }
    }
    private IEnumerable<RecipeResponseVM>? _Recipes;
    public IEnumerable<RecipeResponseVM>? Recipes
    {
        get
        {         
            return _Recipes;
        }

        private set
        {
            if (!Set(ref _Recipes, value)) return;
        }
    }
    public MessageViewModel ErrorMessageViewModel { get; }
    private RecipeModel? _RecipeModel = new();

    /// <summary>Данные пользователя</summary>
    public RecipeModel? RecipeModel
    {
        get => _RecipeModel;

        set
        {
            if (!Set(ref _RecipeModel, value)) return;
        }
    }
    private LambdaCommand _RefreshCommand;
    public ICommand RefreshCommand => _RefreshCommand ?? (_RefreshCommand = new(ExecuteRefreshCommand,p=> p is Guid { }));

    private void ExecuteRefreshCommand(object? p)
    {
        if (p is not Guid { } id)
        {
            Recipes = null;
            return;
        }
        GetRecipesAsync(id).Await(Completed, HandleEror);
    }

    private void Completed()
    {
        ErrorMessage = "Recipes loaded";
    }
    private void HandleEror(Exception ex)
    {
        ErrorMessage = ex.ToString();
    }
    public async Task GetRecipesAsync(Guid id)
    {
        var items = await _RecipesClient.GetByParentIdAsync(id,LoginModel?.AccessToken);
        Recipes = items;
        SelectedRecipe = Recipes?.FirstOrDefault();
        OnPropertyChanged(nameof(SelectedRecipe));
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
    private ICommand _addCommand;
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

    public ICommand AddCommand
    {
        get
        {
            if (_addCommand == null)
                _addCommand = new RelayCommand(param => Add((Guid)param), null);

            return _addCommand;
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
        RecipeModel.Id = Guid.Empty;
        RecipeModel.Quantity = 0;
        RecipeModel.DishesId = Guid.Empty;
        RecipeModel.IngredientId = Guid.Empty;
        IsFormVisible = !IsFormVisible;
    }
    public void Add(Guid id)
    {
        RecipeModel.DishesId = id;
        IsFormVisible = !IsFormVisible;
    }
    public void Edit(Guid id)
    {
        var model = Recipes?.Where(x => x.Id.Equals(id)).FirstOrDefault();
        RecipeModel.Id = model.Id;
        RecipeModel.DishesId = model.DishesId;
        RecipeModel.IngredientId = model.IngredientId;
        RecipeModel.Quantity = model.Quantity;
        IsFormVisible = !IsFormVisible;
    }
    public async Task Save()
    {
        if (RecipeModel != null)
        {
            var item = new RecipeVM
            {
                DishesId = RecipeModel.DishesId,
                IngredientId = RecipeModel.IngredientId,
                Quantity = RecipeModel.Quantity
            };
            if (RecipeModel.Id.Equals(Guid.Empty))
            {
                var result = await _RecipesClient.AddAsync(item, LoginModel?.AccessToken);

                Recipes = new List<RecipeResponseVM>(Recipes!) { new RecipeResponseVM
                 {
                       Id = result ?? System.Guid.Empty,
                       DishesId = item.DishesId,
                       IngredientId = item.IngredientId,
                       Quantity = item.Quantity
                 } };
                SelectedRecipe = Recipes?.Where(c => c.Id.Equals(result)).FirstOrDefault();
            }
            else
            {
                var result = await _RecipesClient.UpdateAsync(RecipeModel.Id, item, LoginModel?.AccessToken);
                Recipes = Recipes?.Where(c => !c.Id.Equals(RecipeModel.Id)).ToArray();
                Recipes = new List<RecipeResponseVM>(Recipes!) { new RecipeResponseVM
                 {
                       Id = RecipeModel.Id,
                       DishesId = item.DishesId,
                       IngredientId = item.IngredientId,
                       Quantity = item.Quantity
                 } };
                SelectedRecipe = Recipes?.Where(c => c.Id.Equals(RecipeModel.Id)).FirstOrDefault();
            }
            OnPropertyChanged(nameof(SelectedRecipe));
        }
    }
    public async void Delete(Guid id)
    {
        if (MessageBox.Show("Confirm delete of this record?", "Recipe", MessageBoxButton.YesNo)
            == MessageBoxResult.Yes)
        {
            try
            {
                var result = await _RecipesClient.RemoveAsync(id, LoginModel?.AccessToken);
                MessageBox.Show("Record successfully deleted.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occured while saving. " + ex.InnerException);
            }
            finally
            {
                Recipes = Recipes?.Where(c => !c.Id.Equals(id)).ToArray();
            }
        }
    }
}
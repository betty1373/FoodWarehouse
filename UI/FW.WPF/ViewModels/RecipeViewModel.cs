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

public class RecipeViewModel : ViewModel
{
    private LoginModel? LoginModel { get; } = null!;
    private DishViewModel? DishViewModel { get; } = null!;
    private readonly IClientBase<IngredientResponseVM, IngredientVM> _IngredientsClient;
    private readonly IRecipesClient _RecipesClient;
    public RecipeViewModel(LoginModel? loginModel,DishViewModel dishViewModel)//IClientIdentity<LoginModel> clientIdentity)
    {
        LoginModel = loginModel;
        _RecipesClient = App.Services.GetRequiredService<IRecipesClient>();
       DishViewModel = dishViewModel;
        ErrorMessageViewModel = new MessageViewModel();
       
    }
    private IEnumerable<RecipeModel>? _Recipes;
    public IEnumerable<RecipeModel>? Recipes
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
    private void Completed()
    {
        ErrorMessage = "Recipes loaded";
    }
    private void HandleEror(Exception ex)
    {
        ErrorMessage = ex.ToString();
    }
    public async Task GetRecipesAsync()
    {
        var items = await _RecipesClient.GetByParentIdAsync(DishViewModel.SelectedDish.Id,LoginModel?.AccessToken);
        Recipes = items.Select(item => new RecipeModel
        {
            Id = item.Id,
            IngredientId = item.IngredientId,
            Quantity = item.Quantity,
        }).ToArray();
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
        
        RecipeModel.Id = Guid.Empty;
        RecipeModel.Quantity = 0;
        RecipeModel.IngredientId = Guid.Empty;
    }
    public void Edit(Guid id)
    {
        var model = Recipes?.Where(x => x.Id.Equals(id)).FirstOrDefault();
        RecipeModel.Id = model.Id;
        RecipeModel.IngredientId = model.IngredientId;
        RecipeModel.Quantity = model.Quantity;
    }
    public async Task Save()
    {
        if (RecipeModel != null)
        {
            var new_Recipe = new RecipeVM
            {
                IngredientId = RecipeModel.IngredientId,
                Quantity = RecipeModel.Quantity
            };
            
            var result = await _RecipesClient.UpdateAsync(RecipeModel.Id, new_Recipe, LoginModel.AccessToken);

            Recipes = Recipes?.Where(c => !c.Id.Equals(RecipeModel.Id)).ToArray();
            Recipes = new List<RecipeModel>(Recipes!) { new RecipeModel
             {
                   Id = RecipeModel.Id,
                   IngredientId = new_Recipe.IngredientId,
                   Quantity = new_Recipe.Quantity,
             } };             
        }
    }
    public async void Delete(Guid id)
    {
        if (MessageBox.Show("Confirm delete of this record?", "Recipe", MessageBoxButton.YesNo)
            == MessageBoxResult.Yes)
        {
            try
            {
                var result = await _RecipesClient.RemoveAsync(id, LoginModel.AccessToken);
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
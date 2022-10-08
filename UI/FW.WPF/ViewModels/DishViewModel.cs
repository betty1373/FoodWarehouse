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
    private LoginModel? LoginModel { get; } = null!;
    private readonly IDishesClient _DishesClient;
    private readonly IClientBase<IngredientResponseVM, IngredientVM> _IngredientsClient;
    private readonly IRecipesClient _RecipesClient;
    public DishViewModel(LoginModel? loginModel)//IClientIdentity<LoginModel> clientIdentity)
    {
        this.LoginModel = loginModel;
        _DishesClient = App.Services.GetRequiredService<IDishesClient>();
        _RecipesClient = App.Services.GetRequiredService<IRecipesClient>();
        _IngredientsClient = App.Services.GetRequiredService<IClientBase<IngredientResponseVM, IngredientVM>>();
        ErrorMessageViewModel = new MessageViewModel();
        GetDishesAsync(LoginModel.AccessToken).Await(Completed, HandleEror);
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
    }
    public string ErrorMessage
    {
        set => ErrorMessageViewModel.Message = value;
    }
    /// <summary>Заголовок главного окна</summary>
    private string _Title = "Вход в систему";

    /// <summary>Заголовок главного окна</summary>
    public string Title { get => _Title; set => Set(ref _Title, value); }

    private IEnumerable<IngredientModel>? _Ingredients;
    /// <summary>Список блюд</summary>
    public IEnumerable<IngredientModel>? Ingredients
    {
        get => _Ingredients;
        private set
        {
            if (!Set(ref _Ingredients, value)) return;
            // SelectedDish = null;
        }
    }
    #region Command UpdateDataCommand - Загрузка данных
    /// <summary>Загрузка данных</summary>
    private LambdaCommand? _UpdateDataCommand;
    /// <summary>Загрузка данных</summary>
    public ICommand UpdateDataCommand => _UpdateDataCommand
        ??= new(OnUpdateDataCommandExecuted, CanUpdateDataCommandExecute);
    /// <summary>Проверка возможности выполнения - Загрузка данных</summary>
    private bool CanUpdateDataCommandExecute() => _UpdateDataCancellation is null;
    private CancellationTokenSource? _UpdateDataCancellation;
    /// <summary>Логика выполнения - Загрузка данных</summary>
    private async void OnUpdateDataCommandExecuted()
    {
        var cancellation = new CancellationTokenSource();
        _UpdateDataCancellation = cancellation;
        try
        {
            var items = await _IngredientsClient.GetAllAsync(LoginModel?.AccessToken, cancellation.Token);
            Ingredients = items.Select(p => new IngredientModel
            {
                Id = p.Id,
                Name = p.Name
            })
               .ToArray();
        }
        catch (OperationCanceledException e) when (e.CancellationToken == cancellation.Token)
        {
        }
        catch (Exception e)
        {
            MessageBox.Show(
                $"Ошибка при получении данных:\r\n{e.Message}", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
        }
        _UpdateDataCancellation = null;
    }
    #endregion
    #region Command AddRecipeCommand - Увеличить количество

    /// <summary>Увеличить количество</summary>
    private LambdaCommand? _AddRecipeCommand;

    /// <summary>Увеличить количество</summary>
    public ICommand AddRecipeCommand => _AddRecipeCommand ??= new(OnAddRecipeCommandExecuted, p => p is RecipeModel);

    /// <summary>Логика выполнения - Увеличить количество</summary>
    private static void OnAddRecipeCommandExecuted(object? p)
    {
        if (p is RecipeModel item) item.Quantity++;
        var message_model = new TextDialogViewModel
        {
            Title = "Add new dish",
            Message = "Название блюда",
            Value = "Описание"
        };

        var message_dlg = new TextDialogWindow
        {
            Owner = Application.Current.MainWindow,
            DataContext = message_model,
        };

        message_model.Completed += (_, e) =>
        {
            message_dlg.DialogResult = e.Arg;
            message_dlg.Close();
        };

        if (message_dlg.ShowDialog() != true) return;

        //var new_item = new RecipeVM
        //{
        //    IngredientName = message_model.Message,
        //    Description = message_model.Value
        //};

        //try
        //{
        //    var result = await _RecipesClient.AddAsync(new_Dish, LoginModel?.AccessToken);

        //    Dishes = new List<DishModel>(Dishes!) { new DishModel
        //       {
        //           Id = result ?? System.Guid.Empty,
        //           Name = new_Dish.Name,
        //           Description = new_Dish.Description,
        //       }
        //    };
        //    SelectedDish = Dishes?.Where(c => c.Id.Equals(result)).FirstOrDefault();
        //    OnPropertyChanged(nameof(SelectedDish));
        //}
        //catch (Exception e)
        //{
        //    MessageBox.Show($"Ошибка добавления блюда {e.Message}", "Ошибка!",
        //        MessageBoxButton.OK, MessageBoxImage.Error);
        //}
    }

    #endregion
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
        DishModel.Recipe.Clear();
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
            
            var result = await _DishesClient.UpdateAsync(DishModel.Id, new_Dish, LoginModel.AccessToken);

             Dishes = Dishes?.Where(c => !c.Id.Equals(DishModel.Id)).ToArray();
            Dishes = new List<DishModel>(Dishes!) { new DishModel
             {
                   Id = DishModel.Id,
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using FW.WPF.WebAPI.Interfaces;
using FW.Domain.Models;
using FW.WPF.Commands;
using FW.WPF.ViewModels.Base;
using FW.WPF.Views;
using FW.WPF.Views.Windows;
using FW.WPF.Identity.Interfaces;
using FW.Domain;

namespace FW.WPF.ViewModels;

public class MainWindowViewModel : ViewModel
{
    private readonly IClientBase<DishResponseVM, DishVM> _DishesClient;
    private readonly IRecipesClient _RecipesClient;
    private readonly IProductsClient _ProductsClient;
    //  private readonly IClientIdentity<LoginModel> _ClientIdentity;
    //  private readonly ImagesClient _ImagesClient;

    //    public IOrdersRepository OrdersRepository { get; }
    //  public ICustomersRepository CustomersRepository { get; }

    public MainWindowViewModel(
        //  IProductsRepository ProductsRepository,
        //   IOrdersRepository OrdersRepository,
        //    ICustomersRepository CustomersRepository,
        IRecipesClient RecipesClient,
        IProductsClient ProductsClient,
        IClientBase<DishResponseVM, DishVM> DishesClient
//,
  //      IClientIdentity<LoginModel> clientIdentity
             //   IClientIdentity clientIdentity
        //   ImagesClient ImagesClient
        )
    {
        //_ProductsRepository = ProductsRepository;
        //this.OrdersRepository = OrdersRepository;
        //this.CustomersRepository = CustomersRepository;
        _DishesClient = DishesClient;
        _RecipesClient = RecipesClient;
        _ProductsClient = ProductsClient;
        //  _ClientIdentity = clientIdentity;
        // _ClientIdentity = clientIdentity;
        //   _ImagesClient = ImagesClient;

        //   Cart = new(this);
    }

    //  public CartOrderViewModel Cart { get; }

    #region Title : string - Заголовок главного окна
    /// <summary>Заголовок главного окна</summary>
    private string _Title = "Склад продуктов";
    /// <summary>Заголовок главного окна</summary>
    public string Title { get => _Title; set => Set(ref _Title, value); }
    #endregion

    #region UserName : string - Имя пользователя
    /// <summary>Имя пользователя</summary>
    private LoginModel? _LoginModel;
    /// <summary>Имя пользователя</summary>
    public LoginModel? LoginModel { get => _LoginModel; set => Set(ref _LoginModel, value); }
    #endregion

    #region Dishes : IEnumerable<DishViewModel>? - Список блюд
    /// <summary>Список блюд</summary>
    private IEnumerable<DishViewModel>? _Dishes;
    /// <summary>Список блюд</summary>
    public IEnumerable<DishViewModel>? Dishes
    {
        get => _Dishes;
        private set
        {
            if (!Set(ref _Dishes, value)) return;
            SelectedDish = null;
        }
    }
    #endregion

    #region SelectedDish : DishViewModel? - Выбранное блюдо
    /// <summary>Выбранное блюдо</summary>
    private DishViewModel? _SelectedDish;
    /// <summary>Выбранное блюдо</summary>
    public DishViewModel? SelectedDish
    {
        get => _SelectedDish;
        set
        {
            if (!Set(ref _SelectedDish, value)) return;
            LoadDishRecipes(value);
        }
    }

    private async void LoadDishRecipes(DishViewModel? Dish)
    {
        if (Dish is not { Id: var dish_id })
        {
            Recipes = null;
            return;
        }

        try
        {
            var recipes = await _RecipesClient.GetByParentIdAsync(dish_id,LoginModel?.AccessToken??"");

            var recipe_view_models = new List<RecipeViewModel>();
            foreach (var recipe in recipes)
            {
                var recipe_model = new RecipeViewModel
                {
                    Id = recipe.Id,               
                    Quantity = recipe.Quantity,                   
                    DishesId = recipe.DishesId,
                    IngredientId = recipe.IngredientId,
                    IngredientName = recipe.IngredientName,
                };
                recipe_view_models.Add(recipe_model);
            }

            Recipes = recipe_view_models;
        }
        catch (OperationCanceledException) { }
        catch (Exception e)
        {
            MessageBox.Show(
                $"Ошибка при получении рецептов блюда:\r\n{e.Message}", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    #endregion

    #region  Recipes  : IEnumerable<ProductViewModel>? - Список товаров
    /// <summary>Список товаров</summary>
    private IEnumerable<RecipeViewModel>? _Recipes;
    /// <summary>Список товаров</summary>
    public IEnumerable<RecipeViewModel>? Recipes 
    {
        get => _Recipes;
        private set
        {
            if (!Set(ref _Recipes, value)) return;
            SelectedRecipe = null;
        }
    }
    #endregion

    #region SelectedRecipe : RecipeViewModel? - Выбранный рецепт
    /// <summary>Выбранный рецепт</summary>
    private RecipeViewModel? _SelectedRecipe;
    /// <summary>Выбранный рецепт</summary>
    public RecipeViewModel? SelectedRecipe
    {
        get => _SelectedRecipe;
        set => Set(ref _SelectedRecipe, value);
    }
    #endregion

    #region Command LoginCommand - Вход в систему
    /// <summary>Вход в систему</summary>
    private LambdaCommand? _LoginCommand;
    /// <summary>Вход в систему</summary>
    public ICommand LoginCommand => _LoginCommand ??= new(OnLoginCommandExecuted);
    /// <summary>Логика выполнения - Вход в систему</summary>
    private void OnLoginCommandExecuted()
    {
        var login_view_model = new LoginWindowViewModel();//_ClientIdentity);
        var main_window = Application.Current.MainWindow;
        var login_window = new LoginView
        {
            Owner = main_window,
            DataContext = login_view_model
        };
        login_view_model.Login += (_, e) =>
        {
            login_window.DialogResult = true;
            login_window.Close();
        };

        if (login_window.ShowDialog() != true) return;
        LoginModel = login_view_model.LoginModel;
        if (Dishes is null) 
        {
            OnUpdateDataCommandExecuted();          
        }
        SelectedDish = Dishes?.FirstOrDefault();
        OnPropertyChanged(nameof(SelectedDish));
        LoadDishRecipes(SelectedDish);
    }
    #endregion

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
            var dishes = await _DishesClient.GetAllAsync(LoginModel?.AccessToken, cancellation.Token);
            Dishes = dishes
               .Select(dish => new DishViewModel
                {
                    Id = dish.Id,
                    Name = dish.Name,
                    Description = dish.Description,
                })
               .ToArray();
        }
        catch (OperationCanceledException e) when(e.CancellationToken == cancellation.Token)
        {

        }
        catch (Exception e)
        {
            MessageBox.Show(
                $"Ошибка при получении данных:\r\n{e.Message}", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }

        _UpdateDataCancellation = null;
    }
    #endregion

    #region Command AddToCartCommand - Добавить в корзину
    /// <summary>Добавить в корзину</summary>
    private LambdaCommand? _AddToCartCommand;
    /// <summary>Добавить в корзину</summary>
   // public ICommand AddToCartCommand => _AddToCartCommand ??= new(OnAddToCartCommandExecuted, p => p is ProductViewModel);
    /// <summary>Логика выполнения - Добавить в корзину</summary>
    private void OnAddToCartCommandExecuted(object? p)
    {
      //  if (p is ProductViewModel product) Cart.Add(product);
    }
    #endregion

    #region Command ShowCartCommand - Показать корзину
    /// <summary>Показать корзину</summary>
    private LambdaCommand? _ShowCartCommand;
    /// <summary>Показать корзину</summary>
   // public ICommand ShowCartCommand => _ShowCartCommand 
   //     ??= new(OnShowCartCommandExecuted, () => _UserName is { Length: > 0 } && Cart.Items.Count > 0);
    /// <summary>Логика выполнения - Показать корзину</summary>
    private void OnShowCartCommandExecuted()
    {
        //var cart_window = new CartWindow
        //{
        //    Owner = Application.Current.MainWindow,
        //    DataContext = Cart,
        //};

        //cart_window.ShowDialog();
    }
    #endregion

    #region Command ShowOrdersCommand - Показать заказы
    /// <summary>Показать заказы</summary>
    private LambdaCommand? _ShowOrdersCommand;
    /// <summary>Показать заказы</summary>
    public ICommand ShowOrdersCommand => _ShowOrdersCommand
        ??= new(OnShowOrdersCommandExecuted, () => LoginModel?.UserName is { Length: > 0 });
    /// <summary>Логика выполнения - Показать заказы</summary>
    private void OnShowOrdersCommandExecuted()
    {
        //var window = new CustomerOrdersWindow
        //{
        //    Owner = Application.Current.MainWindow,
        //    DataContext = new CustomerOrdersViewModel(this)
        //};

        //window.ShowDialog();
    }
    #endregion
    #region Command CreateDishCommand - Создать категорию

    /// <summary>Создать категорию</summary>
    private LambdaCommand? _CreateDishCommand;

    /// <summary>Создать категорию</summary>
    public ICommand CreateDishCommand => _CreateDishCommand ??= new(OnCreateDishCommandExecuted);

    /// <summary>Логика выполнения - Создать категорию</summary>
    private async void OnCreateDishCommandExecuted()
    {
        var message_model = new TextDialogViewModel
        {
            Title = "Новое блюдо",
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

        var new_Dish = new DishVM
        {
            Name = message_model.Value
        };
        //try
        //{
        //    var dishes = await _DishesClient.GetAllAsync(LoginModel?.AccessToken);
        //    Dishes = dishes
        //       .Select(dish => new DishViewModel
        //       {
        //           Id = dish.Id,
        //           Name = dish.Name,
        //           Description = dish.Description,
        //       })
        //       .ToArray();
        //}
        try
        {
            var result = await _DishesClient.AddAsync(new_Dish, LoginModel?.AccessToken);
           
            Dishes = new List<DishViewModel>(Dishes!) { new DishViewModel
               {
                   Id = result,
                   Name = new_Dish.Name,
                   Description = new_Dish.Description,
               }
            };
            SelectedDish = Dishes.FirstOrDefault();
        }
        catch (Exception e)
        {
            MessageBox.Show($"Ошибка при создании нового блюда {e.Message}", "Ошибка!",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    #endregion

    #region Command RemoveDishCommand - Удаление категории

    /// <summary>Удаление категории</summary>
    private LambdaCommand? _RemoveDishCommand;

    /// <summary>Удаление категории</summary>
    public ICommand RemoveDishCommand => _RemoveDishCommand ??= new(OnRemoveDishCommandExecuted, p => p is DishViewModel);

    /// <summary>Логика выполнения - Удаление категории</summary>
    private async void OnRemoveDishCommandExecuted(object? p)
    {
        if (p is not DishViewModel { Id: var Dish_id } Dish) return;
        if (MessageBox.Show(
                $"Подтверждаете удаление блюда {Dish.Name}",
                "Удаление блюда",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question,
                MessageBoxResult.No) != MessageBoxResult.Yes)
            return;

        try
        {
            var result = await _DishesClient.RemoveAsync(Dish_id, LoginModel?.AccessToken);
            if (result is null)
            {
                MessageBox.Show(
                    $"Не удалось удалить блюдо {Dish.Name}", "Ошибка!",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Dishes = Dishes?.Where(c => c.Id != Dish_id).ToArray();
            SelectedDish = null;
        }
        catch (Exception e)
        {
            MessageBox.Show($"Ошибка в процессе удаления категории {e.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    #endregion
}

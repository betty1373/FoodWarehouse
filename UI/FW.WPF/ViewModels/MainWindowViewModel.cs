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
using FW.WPF.Models;
using FW.WPF.Views;
using FW.WPF.Views.Windows;
using FW.WPF.Identity.Interfaces;
using FW.Domain;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Linq;

namespace FW.WPF.ViewModels;

public class MainWindowViewModel : ViewModel
{
    public GridView GridView { get; set; }

    private readonly IDishesClient _DishesClient;
    private readonly IRecipesClient _RecipesClient;
    private readonly IProductsClient _ProductsClient;

    public MainWindowViewModel(
        IRecipesClient RecipesClient,
        IProductsClient ProductsClient,
        IDishesClient DishesClient
        )
    {
        _DishesClient = DishesClient;
        _RecipesClient = RecipesClient;
        _ProductsClient = ProductsClient;
    }

    int _tabItem;
    public int TabItem
    {
        get => _tabItem;
        set
        {
            if (!Set(ref _tabItem, value)) return;
            ResetGridView();
            Task.Run(async () => await OnTabItemCommandExecuted(TabItem));
          
            Console.WriteLine("Ogogo");
        }
       // set => Set(ref _tabItem, value); 
    }
    #region Command TabItemCommand - Выбор данных
    /// <summary>Выбор данных</summary>
    private LambdaCommand? _TabItemCommand;
    /// <summary>Выбор данных</summary>
    public ICommand TabItemCommand => _TabItemCommand ??= new (async ()=> await OnTabItemCommandExecuted(TabItem));
    /// <summary>Логика выполнения - Выбор данных</summary>
    private Task OnTabItemCommandExecuted(object p,CancellationToken Cancel = default)
    {
        var selectedTab = (int) p;
        switch (selectedTab)
        {
            case 0:
                if (Dishes is null)
                {
                    OnUpdateDataCommandExecuted();
                }
               
              //  LoadDishRecipes(SelectedDish); 
                break;
            case 1:
          
                if (LoginModel?.WarehouseName is string && Products is null)
                {
                    OnLoadProductsCommandExecuted(LoginModel);
                    Console.WriteLine("Egege");
                }
                SelectedProduct = Products?.FirstOrDefault();
                 break;
            default: break;
        }
      
        return Task.CompletedTask;
        
       // OnPropertyChanged(nameof(SelectedTab));
        
    }
    #endregion
    #region Products : IEnumerable<ProductViewModel>? - Список продуктов
    /// <summary>Список продуктов</summary>
    private IEnumerable<ProductViewModel>? _Products;
    /// <summary>Список продуктов</summary>
    public IEnumerable<ProductViewModel>? Products
    {
        get => _Products;
        private set
        {
            if (!Set(ref _Products, value)) return;
        }
    }
    #endregion
    private async void OnLoadProductsCommandExecuted(object? p)
    {
        if (p is not LoginModel { WarehouseId: var warehouse_id,
                                  AccessToken: var access_token
                                } login)
        {
            Products = null;
            return;
        }

        try
        {
            var items = await _ProductsClient.GetByParentIdAsync(warehouse_id ?? Guid.Empty, access_token ?? "");
            Products = items.Select(product =>
                    new ProductViewModel
                    {
                        Id = product.Id,
                        WarehouseId = product.WarehouseId,
                        CategoryId = product.CategoryId,
                        CategoryName = product.CategoryName,
                        IngredientId = product.IngredientId,
                        IngredientName = product.IngredientName,
                     //   Name = product.Name,
                        ExpirationDate = product.ExpirationDate,
                        Quantity = product.Quantity
                    }).ToArray();
        }
        catch (OperationCanceledException) { }
        catch (Exception e)
        {
            MessageBox.Show(
                $"Ошибка при получении списка продуктов:\r\n{e.Message}", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    #region SelectedProduct : ProductViewModel? - Выбранный продукт
    /// <summary>Выбранный склад</summary>
    private ProductViewModel? _SelectedProduct;
    /// <summary>Выбранный склад</summary>
    public ProductViewModel? SelectedProduct
    {
        get => _SelectedProduct;
        set => Set(ref _SelectedProduct, value);
    }
    #endregion
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
    private IEnumerable<DishModel>? _Dishes;
    /// <summary>Список блюд</summary>
    public IEnumerable<DishModel>? Dishes
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
    private DishModel? _SelectedDish;
    /// <summary>Выбранное блюдо</summary>
    public DishModel? SelectedDish
    {
        get => _SelectedDish;
        set
        {
            if (!Set(ref _SelectedDish, value)) return;
            if (_SelectedDish?.Recipe is null)
            {
                OnLoadRecipeCommandExecuted(SelectedDish);
            }
        }
    }
    #endregion

    private LambdaCommand _DishCommand;
    public ICommand DishCommand => _DishCommand ?? (_DishCommand = new (ExecuteDishCommand,p => p is object));

    private void ExecuteDishCommand(object? obj)
    {
        if ((obj as SelectionChangedEventArgs)?.AddedItems.Count > 0)
        {
            SelectedDish = ((obj as SelectionChangedEventArgs)?.AddedItems[0] is DishModel) ? (DishModel) (obj as SelectionChangedEventArgs)?.AddedItems[0] : null;
        }
    }
    #region Command LoadRecipeCommand - Выбор данных
    /// <summary>Выбор данных</summary>
    private LambdaCommand? _LoadRecipeCommand;
    /// <summary>Выбор данных</summary>
    public ICommand LoadRecipeCommand => _LoadRecipeCommand ??= new(OnLoadRecipeCommandExecuted, p => p is DishModel);
    /// <summary>Логика выполнения - Выбор данных</summary>
    private async void OnLoadRecipeCommandExecuted(object? p)
    {
        if (p is not DishModel { Id: var dish_id } Dish)
        {
           // Dish.Recipes = null;
            return;
        }

        try
        {
            var recipes = await _RecipesClient.GetByParentIdAsync(dish_id,LoginModel?.AccessToken??"");
            SelectedDish.Recipe = recipes.Select(recipe=>new RecipeModel
                {
                    Id = recipe.Id,               
                    Quantity = recipe.Quantity,                   
                    DishesId = recipe.DishesId,
                    IngredientId = recipe.IngredientId,
                    IngredientName = recipe.IngredientName,
                }).ToList();
        }
        catch (OperationCanceledException) { }
        catch (Exception e)
        {
            MessageBox.Show(
                $"Ошибка при получении рецептов блюда:\r\n{e.Message}", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
        
        OnPropertyChanged(nameof(SelectedDish));
    }

    #endregion

    //#region  Recipes  : IEnumerable<ProductViewModel>? - Список товаров
    ///// <summary>Список товаров</summary>
    //private IEnumerable<RecipeViewModel>? _Recipes;
    ///// <summary>Список товаров</summary>
    //public IEnumerable<RecipeViewModel>? Recipes 
    //{
    //    get => _Recipes;
    //    private set
    //    {
    //        if (!Set(ref _Recipes, value)) return;
    //        SelectedRecipe = null;
    //    }
    //}
    //#endregion

    #region SelectedRecipe : RecipeViewModel? - Выбранный рецепт
    /// <summary>Выбранный рецепт</summary>
    private RecipeModel? _SelectedRecipe;
    /// <summary>Выбранный рецепт</summary>
    public RecipeModel? SelectedRecipe
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
        OnPropertyChanged(nameof(LoginModel));
        Task.Run(async ()=> await OnTabItemCommandExecuted(TabItem)); 
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
            var items = await _DishesClient.GetByParentIdAsync(LoginModel?.AccessToken, cancellation.Token);
            Dishes = items
               .Select(dish => new DishModel
               {
                   Id = dish.Id,
                   Name = dish.Name,
                   Description = dish.Description,
               })
               .ToArray();
            Console.WriteLine("1");
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
            SelectedDish = Dishes?.FirstOrDefault();
            //OnPropertyChanged(nameof(SelectedDish));
            Console.WriteLine("2");
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
    #region Command CreateDishCommand - Создать блюдо
    /// <summary>Создать блюдо</summary>
    private LambdaCommand? _CreateDishCommand;
    /// <summary>Создать блюдо</summary>
    public ICommand CreateDishCommand => _CreateDishCommand ??= new(OnCreateDishCommandExecuted);
    /// <summary>Логика выполнения - Создать блюдо</summary>
    private async void OnCreateDishCommandExecuted()
    {
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

        var new_Dish = new DishVM
        {
            Name = message_model.Message,
            Description = message_model.Value
        };
      
        try
        {
            var result = await _DishesClient.AddAsync(new_Dish, LoginModel?.AccessToken);
           
            Dishes = new List<DishModel>(Dishes!) { new DishModel
               {
                   Id = result ?? System.Guid.Empty,
                   Name = new_Dish.Name,
                   Description = new_Dish.Description,
               }
            };
            SelectedDish = Dishes?.Where(c => c.Id.Equals(result)).FirstOrDefault();
            OnPropertyChanged(nameof(SelectedDish));
        }
        catch (Exception e)
        {
            MessageBox.Show($"Ошибка добавления блюда {e.Message}", "Ошибка!",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    #endregion
    #region Command UpdateDishCommand - Создать блюдо
    /// <summary>Создать блюдо</summary>
    private LambdaCommand? _UpdateDishCommand;
    /// <summary>Создать блюдо</summary>
    public ICommand UpdateDishCommand => _UpdateDishCommand ??= new(OnUpdateDishCommandExecuted, p => p is DishModel);
    /// <summary>Логика выполнения - Создать блюдо</summary>
    private async void OnUpdateDishCommandExecuted(object? p)
    {
        if (p is not DishModel { Id: var Dish_id } Dish) return;
        var message_model = new TextDialogViewModel
        {
            Title = "Edit dish",
            Message = Dish.Name,
            Value = Dish.Description
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
            Name = message_model.Message,
            Description = message_model.Value
        };

        try
        {
            var result = await _DishesClient.UpdateAsync(Dish.Id,new_Dish, LoginModel?.AccessToken);
         
            Dishes = Dishes?.Where(c => !c.Id.Equals(Dish_id)).ToArray();
            Dishes = new List<DishModel>(Dishes!) { new DishModel
               {
                   Id = Dish.Id,
                   Name = new_Dish.Name,
                   Description = new_Dish.Description,
               }
            };
            SelectedDish = Dishes?.Where(x=> x.Id.Equals(Dish.Id)).FirstOrDefault();
            OnPropertyChanged(nameof(SelectedDish));
        }
        catch (Exception e)
        {
            MessageBox.Show($"Ошибка редактирования блюда {e.Message}", "Ошибка!",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    #endregion
    #region Command RemoveDishCommand - Удаление блюда
    /// <summary>Удаление категории</summary>
    private LambdaCommand? _RemoveDishCommand;
    /// <summary>Удаление категории</summary>
    public ICommand RemoveDishCommand => _RemoveDishCommand ??= new(OnRemoveDishCommandExecuted, p => p is DishModel);
    /// <summary>Логика выполнения - Удаление категории</summary>
    private async void OnRemoveDishCommandExecuted(object? p)
    {
        if (p is not DishModel { Id: var Dish_id } Dish) return;
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
                    $"Ошибка удаления блюда {Dish.Name}", "Ошибка!",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Dishes = Dishes?.Where(c => !c.Id.Equals(Dish_id)).ToArray();
            SelectedDish = null;
            OnPropertyChanged(nameof(SelectedDish));
        }
        catch (Exception e)
        {
            MessageBox.Show($"Ошибка в процессе удаления блюда {e.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    #endregion
    public void ResetGridView()
    {
        if (GridView != null)
        {
            foreach (GridViewColumn c in GridView.Columns)
            {
                if (double.IsNaN(c.Width))
                {
                    c.Width = c.ActualWidth;

                }
                c.Width = double.NaN;

            }
        }
    }
}

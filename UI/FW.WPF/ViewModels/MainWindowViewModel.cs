using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using FW.WPF.WebAPI.Interfaces;
using FW.Models;
using FW.WPF.Commands;
using FW.WPF.ViewModels.Base;
using FW.WPF.Models;
using FW.WPF.Views;
using FW.WPF.Views.Windows;
using FW.WPF.Identity.Interfaces;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Linq;
using FW.WPF.WebAPI.Clients;

namespace FW.WPF.ViewModels;

public class MainWindowViewModel : ViewModel
{
    public GridView GridView { get; set; }
    private readonly IProductsClient _ProductsClient;
    private readonly IChangesProductsClient _ChangesProductsClient;

    public MainWindowViewModel(
        LoginModel loginModel,       
        IProductsClient productsClient,
        IChangesProductsClient changesProductsClient        
        )
    {
      
        _LoginModel = loginModel;
        _ProductsClient = productsClient;
        _ChangesProductsClient = changesProductsClient;
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
                if (DishesModel is null)
                {
                    DishesModel = new DishViewModel(LoginModel);
                    DishesModel.RefreshCommand.Execute(LoginModel?.AccessToken);
                }
                if (IngredientsModel is null)
                {
                    IngredientsModel = new IngredientViewModel(LoginModel);
                    IngredientsModel.RefreshCommand.Execute(null);
                }
             
                break;
            case 1:
          
                if (LoginModel?.WarehouseName is string && (Products is null || DishesModel.DishCooked))
                {
                    OnLoadProductsCommandExecuted(LoginModel);
                    DishesModel.DishCooked = false;
                }
                SelectedProduct = Products?.FirstOrDefault();
                 break;
            default: break;
        }
      
        return Task.CompletedTask;
        
    }
    #endregion
    #region Products : IEnumerable<ProductViewModel>? - Список продуктов
    /// <summary>Список продуктов</summary>
    private IEnumerable<ProductResponseVM>? _Products;
    /// <summary>Список продуктов</summary>
    public IEnumerable<ProductResponseVM>? Products
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
            Products = items;
        }
        catch (OperationCanceledException) { }
        catch (Exception e)
        {
            MessageBox.Show(
                $"Ошибка при получении списка продуктов:\r\n{e.Message}", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    private IEnumerable<ChangesProductResponseVM>? _ChangesProduct;
    /// <summary>Список продуктов</summary>
    public IEnumerable<ChangesProductResponseVM>? ChangesProduct
    {
        get => _ChangesProduct;
        private set
        {
            if (!Set(ref _ChangesProduct, value)) return;
        }
    }
    private async void OnChangesProductCommandExecuted(object? p)
    {
        if (p is not Guid { } id)
        { 
            ChangesProduct = null;
            return;
        }

        try
        {
            var items = await _ChangesProductsClient.GetByParentIdAsync(id, LoginModel?.AccessToken ?? "");
            ChangesProduct = items;
        }
        catch (OperationCanceledException) { }
        catch (Exception e)
        {
            MessageBox.Show(
                $"Ошибка при получении движения продукта:\r\n{e.Message}", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    #region SelectedProduct : ProductViewModel? - Выбранный продукт
    /// <summary>Выбранный склад</summary>
    private ProductResponseVM? _SelectedProduct;
    /// <summary>Выбранный склад</summary>
    public ProductResponseVM? SelectedProduct
    {
        get => _SelectedProduct;
        set
        {
            Set(ref _SelectedProduct, value);
            OnChangesProductCommandExecuted(SelectedProduct?.Id);
        }
    }
    #endregion

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

   
    private DishViewModel? _DishesModel;
    public DishViewModel? DishesModel { get => _DishesModel; set => Set(ref _DishesModel, value); }

    private IngredientViewModel? _IngredientsModel;
    public IngredientViewModel? IngredientsModel { get => _IngredientsModel; set => Set(ref _IngredientsModel, value); }


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

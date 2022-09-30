using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using FW.WPF.ViewModels.Base;
namespace FW.WPF.ViewModels;
public class ProductViewModel : ViewModel
{
    public Guid Id { get; init; }
    public Guid WarehouseId { get; init; }
    public Guid CategoryId { get; init; }
    public string CategoryName { get; init; } = null!;
    public Guid IngredientId { get; init; }
    public string IngredientName { get; init; } = null!;
    public string Name { get; init; } = null!;
    public DateTime ExpirationDate { get; init; }
    public double Quantity { get; init; }
}


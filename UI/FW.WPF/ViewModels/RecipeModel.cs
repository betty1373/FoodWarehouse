using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using FW.WPF.ViewModels.Base;
namespace FW.WPF.Models;

public class RecipeModel 
{
    public Guid Id { get; init; }
    public Guid IngredientId { get; init; }
    public string IngredientName { get; init; } = null!;
    public Guid DishesId { get; set; }
    public double Quantity { get; set; }
}
   

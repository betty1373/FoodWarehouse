using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using FW.WPF.ViewModels;
using FW.WPF.ViewModels.Base;
namespace FW.WPF.Models;

public class RecipeModel : ViewModel, IEditableObject
{
   
    public string IngredientName { get; init; } = null!;

    private Guid _Id;
    public Guid Id
    {
        get => _Id;
        set => Set(ref _Id, value);
    }
    private Guid _DishesId;
    public Guid DishesId
    {
        get => _DishesId;
        set => Set(ref _DishesId, value);
    }
    private Guid _IngredientId;
    public Guid IngredientId
    {
        get => _IngredientId;
        set => Set(ref _IngredientId, value);
    }
    private double _Quantity;
    public double Quantity
    {
        get => _Quantity;
        set => Set(ref _Quantity, value);
    }

    private RecipeModel backupCopy;

    private bool inEdit;

    public void BeginEdit()
    {
        if (inEdit) return;
        inEdit = true;
        backupCopy = MemberwiseClone() as RecipeModel;
    }

    public void CancelEdit()
    {
        if (!inEdit) return;
        inEdit = false;
        DishesId = backupCopy.DishesId;
        Id = backupCopy.Id;
       IngredientId = backupCopy.IngredientId;
    }

    public void EndEdit()
    {
        if (!inEdit) return;
        inEdit = false;
        backupCopy = null;
    }
}
   

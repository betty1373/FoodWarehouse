using FW.WPF.ViewModels.Base;
using FW.WPF.Models;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;

namespace FW.WPF.Models;

public class IngredientModel : ViewModel
{
    private Guid _Id;
    public Guid Id
    {
        get => _Id;
        set => Set(ref _Id, value);
    }
    private string _Name;
    public string Name
    {
        get => _Name;
        set => Set(ref _Name, value);
    }
}
 //   public IEnumerable<RecipeModel>? Recipe { get; set; }
    //private DishModel backupCopy;
    
    //private bool inEdit;

    //public void BeginEdit()
    //{
    //    if (inEdit) return;
    //    inEdit = true;
    //    backupCopy = MemberwiseClone() as DishModel;
    //}

    //public void CancelEdit()
    //{
    //    if (!inEdit) return;
    //    inEdit = false;
    //    Name = backupCopy.Name;
    //    Id = backupCopy.Id;
    //    Description = backupCopy.Description;
    //}

    //public void EndEdit()
    //{
    //    if (!inEdit) return;
    //    inEdit = false;
    //    backupCopy = null;
    //}
//}


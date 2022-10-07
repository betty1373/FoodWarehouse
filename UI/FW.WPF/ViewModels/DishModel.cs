using FW.WPF.ViewModels.Base;
using FW.WPF.Models;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;

namespace FW.WPF.ViewModels;

public class DishModel : ViewModel,IEditableObject
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
    private string _Description;
    public string Description
    {
        get => _Description;
        set => Set(ref _Description, value);
    }

    public List<RecipeModel>? Recipe { get; set; }
    private DishModel backupCopy;
    
    private bool inEdit;

    public void BeginEdit()
    {
        if (inEdit) return;
        inEdit = true;
        backupCopy = MemberwiseClone() as DishModel;
    }

    public void CancelEdit()
    {
        if (!inEdit) return;
        inEdit = false;
        Name = backupCopy.Name;
        Id = backupCopy.Id;
        Description = backupCopy.Description;
    }

    public void EndEdit()
    {
        if (!inEdit) return;
        inEdit = false;
        backupCopy = null;
    }
}
public class DishValidationRule : ValidationRule
{
    public override ValidationResult Validate(object value,
        System.Globalization.CultureInfo cultureInfo)
    {
        DishModel item = (value as BindingGroup).Items[0] as DishModel;
        if (item.Description.Equals(String.Empty))
        {
            return new ValidationResult(false,
                "Start Date must be earlier than End Date.");
        }
        else
        {
            return ValidationResult.ValidResult;
        }
    }
}

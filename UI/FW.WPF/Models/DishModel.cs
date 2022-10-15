using FW.WPF.ViewModels.Base;
using FW.WPF.Models;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;

namespace FW.WPF.Models;

public class DishModel : ViewModel
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

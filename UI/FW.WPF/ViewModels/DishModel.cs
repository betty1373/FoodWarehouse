using FW.WPF.ViewModels.Base;
using FW.WPF.Models;
using System;
using System.Collections.Generic;

namespace FW.WPF.ViewModels;

public class DishModel : ViewModel
{
    public Guid Id { get; init; }
    public string Name { get; init; } = null!;
    public string Description { get; init; } = null!;

    public List<RecipeModel>? Recipe { get; set; }

}

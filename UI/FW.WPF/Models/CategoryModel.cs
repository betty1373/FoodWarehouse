using FW.WPF.ViewModels.Base;
using System;
namespace FW.WPF.Models;

public class CategoryModel 
{
    public Guid Id { get; init; }
    public string Name { get; init; } = null!;
}

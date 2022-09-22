using FW.WPF.ViewModels.Base;
using System;
namespace FW.WPF.ViewModels;

public class CategoryViewModel : ViewModel
{
    public Guid Id { get; init; }
    public string Name { get; init; } = null!;
}

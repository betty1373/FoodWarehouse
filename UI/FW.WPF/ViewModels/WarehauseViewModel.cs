﻿using FW.WPF.ViewModels.Base;
using System;
namespace FW.WPF.ViewModels;

public class WarehauseViewModel : ViewModel
{
    public Guid Id { get; init; }
    public string Name { get; init; } = null!;
    public string Address { get; init; } = null!;
}

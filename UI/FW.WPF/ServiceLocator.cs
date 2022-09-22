using FW.WPF.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace FW.WPF;

public class ServiceLocator
{
    public MainWindowViewModel MainModel => App.Services.GetRequiredService<MainWindowViewModel>();
}

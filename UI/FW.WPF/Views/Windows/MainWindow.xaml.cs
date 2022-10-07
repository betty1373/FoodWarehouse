
using FW.WPF.ViewModels;
using System.Windows.Controls;
namespace FW.WPF.Views.Windows;

public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();
        recipes.SelectionChanged += Recipes_SelectionChanged;
        (DataContext as MainWindowViewModel).GridView = gridView; 
    }

    private void Recipes_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        var listBox = sender as ListBox;
        if (listBox != null) 
        {
            listBox.ScrollIntoView(listBox.SelectedItem);
        }
    }
}

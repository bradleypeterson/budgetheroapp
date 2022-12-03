using DesktopApplication.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace DesktopApplication.Views;

public sealed partial class BudgetPage : Page
{
  
    public BudgetViewModel ViewModel { get; }

    public BudgetPage()
    {
        ViewModel = App.GetService<BudgetViewModel>();
        InitializeComponent();
    }

    private async void Page_Loaded(object sender, RoutedEventArgs e)
    {
        await ViewModel.LoadAsync();
    }
}

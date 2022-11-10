using DesktopApplication.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace DesktopApplication.Views;

public sealed partial class ExpensesPage : Page
{
    public ExpensesViewModel ViewModel { get; }

    public ExpensesPage()
    {
        ViewModel = App.GetService<ExpensesViewModel>();
        InitializeComponent();
    }

    private async void Page_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        await ViewModel.LoadAsync();
    }
}

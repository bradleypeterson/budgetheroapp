using DesktopApplication.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace DesktopApplication.Views;

public sealed partial class ExpensesPage : Page
{
    public ExpensesViewModel ViewModel
    {
        get;
    }

    public ExpensesPage()
    {
        ViewModel = App.GetService<ExpensesViewModel>();
        InitializeComponent();
    }
}

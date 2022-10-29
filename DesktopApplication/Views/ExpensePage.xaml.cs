using DesktopApplication.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace DesktopApplication.Views;

public sealed partial class ExpensePage : Page
{
    public ExpenseViewModel ViewModel
    {
        get;
    }

    public ExpensePage()
    {
        ViewModel = App.GetService<ExpenseViewModel>();
        InitializeComponent();
    }
}

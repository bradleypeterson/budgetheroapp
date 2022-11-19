using CommunityToolkit.WinUI.UI.Controls;
using DesktopApplication.ViewModels;

using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;

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

    private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        TextBox textBox = (TextBox)sender;

        ViewModel.filterList(textBox.Text);
        TransactionData.ItemsSource = null;
        TransactionData.ItemsSource = ViewModel.Transactions;
    }

}

using DesktopApplication.Commands;
using DesktopApplication.Contracts.Services;
using DesktopApplication.Helpers;
using DesktopApplication.Views.Forms;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace DesktopApplication.Services;
public class DialogService : IDialogService
{
    private readonly XamlRoot _root;

    public DialogService()
    {
        _root = MainWindowHelper.GetXamlRoot();
    }

    // Demo method only to prove functionality
    public async void ShowDialog()
    {
        ContentDialog dialog = new()
        {
            XamlRoot = _root,
            Style = App.Current.Resources["DefaultContentDialogStyle"] as Style,
            Title = "Add Account",
            PrimaryButtonText = "Save",
            CloseButtonText = "Cancel",
            DefaultButton = ContentDialogButton.Primary,
            PrimaryButtonCommand = new AddAccountCommand(),
            Content = new AccountForm()
        };
        await dialog.ShowAsync();
    }

    public async void ExpenseDialog()
    {
        ContentDialog dialog = new()
        {
            XamlRoot = _root,
            Style = App.Current.Resources["DefaultContentDialogStyle"] as Style,
            Title = "Add Expense",
            PrimaryButtonText = "Save",
            CloseButtonText = "Cancel",
            DefaultButton = ContentDialogButton.Primary,
            PrimaryButtonCommand = new AddAccountCommand(),
            Content = new ExpenseForm()
        };
        await dialog.ShowAsync();
    }
}

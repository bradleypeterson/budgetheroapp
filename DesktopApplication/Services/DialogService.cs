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

    // Demo method only to prove functionality---------
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
    //-------------------------------------------------

    public async void AddCategoryGroupDialog()
    {
        ContentDialog dialog = new()
        {
            XamlRoot = _root,
            Style = App.Current.Resources["DefaultContentDialogStyle"] as Style,
            Title = "Add Category Group",
            PrimaryButtonText = "Save",
            CloseButtonText = "Cancel",
            DefaultButton = ContentDialogButton.Primary,
            PrimaryButtonCommand = new AddCategoryGroupCommand(),
            Content = new AddCategoryGroupForm()
        };
        await dialog.ShowAsync();
    }

    public async void DeleteCategoryGroupDialog()
    {
        ContentDialog dialog = new()
        {
            XamlRoot = _root,
            Style = App.Current.Resources["DefaultContentDialogStyle"] as Style,
            Title = "Delete Category Group",
            PrimaryButtonText = "Save",
            CloseButtonText = "Cancel",
            DefaultButton = ContentDialogButton.Primary,
            PrimaryButtonCommand = new DeleteCategoryGroupCommand(),
            Content = new DeleteCategoryGroupForm()
        };
        await dialog.ShowAsync();
    }

    public async void AddAccountDialog()
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

    public async void AddExpenseDialog()
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

    public async void EditExpenseDialog()
    {
        ContentDialog dialog = new()
        {
            XamlRoot = _root,
            Style = App.Current.Resources["DefaultContentDialogStyle"] as Style,
            Title = "Edit Expense",
            PrimaryButtonText = "Save",
            CloseButtonText = "Cancel",
            DefaultButton = ContentDialogButton.Primary,
            PrimaryButtonCommand = new EditExpenseCommand(),
            Content = new ExpenseForm()
        };
        await dialog.ShowAsync();
    }

    public async void DeleteExpenseDialog()
    {
        ContentDialog dialog = new()
        {
            XamlRoot = _root,
            Style = App.Current.Resources["DefaultContentDialogStyle"] as Style,
            Title = "Delete Expense",
            PrimaryButtonText = "Confirm",
            CloseButtonText = "Cancel",
            DefaultButton = ContentDialogButton.Primary,
            PrimaryButtonCommand = new DeleteExpenseCommand(),
            Content = new DeleteExpenseForm()
        };
        await dialog.ShowAsync();
    }

    public void DeleteAccountDialog() => throw new NotImplementedException();
}

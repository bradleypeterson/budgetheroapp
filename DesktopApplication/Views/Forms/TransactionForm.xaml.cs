using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using DesktopApplication.Contracts.Services;
using DesktopApplication.ViewModels.Forms;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DesktopApplication.Views.Forms;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class TransactionForm : Page
{
    public TransactionFormViewModel ViewModel
    {
        get;
    }
    public TransactionForm()
    {
        ViewModel = App.GetService<TransactionFormViewModel>();
        InitializeComponent();
        ViewModel.OnInvalidNumber += addInvalidNumberError;
        ViewModel.OnValidNumber += removeInvalidNumberError;
        ViewModel.OnInvalidAccount += addInvalidAccountError;
        ViewModel.OnValidAccount += removeInvalidAccountError;
        ViewModel.OnInvalidCategory += addInvalidCategoryError;
        ViewModel.OnValidCategory += removeInvalidCategoryError;
        _dialogService = App.GetService<IDialogService>();

    }

    private readonly IDialogService _dialogService;

    private async void Page_Loaded(object sender, RoutedEventArgs e)
    {
        await ViewModel.LoadAsync();
        Debug.WriteLine($"Expense: {ExpenseRadButton.IsChecked} | Deposit: {DepositRadButton.IsChecked}");
    }

    private void addInvalidNumberError(object? sender, EventArgs e)
    {
        tbInvalidNumberError.Visibility = Visibility.Visible;
    }
    private void removeInvalidNumberError(object? sender, EventArgs e)
    {
        tbInvalidNumberError.Visibility = Visibility.Collapsed;
    }

    private void addInvalidAccountError(object? sender, EventArgs e)
    {
        tbInvalidAccountError.Visibility = Visibility.Visible;
    }
    private void removeInvalidAccountError(object? sender, EventArgs e)
    {
        tbInvalidAccountError.Visibility = Visibility.Collapsed;
    }

    private void addInvalidCategoryError(object? sender, EventArgs e)
    {
        tbInvalidCategoryError.Visibility = Visibility.Visible;
    }
    private void removeInvalidCategoryError(object? sender, EventArgs e)
    {
        tbInvalidCategoryError.Visibility = Visibility.Collapsed;
    }
}

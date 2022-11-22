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
    private readonly IDialogService _dialogService;

    public TransactionFormViewModel ViewModel
    {
        get;
    }
    public TransactionForm()
    {
        ViewModel = App.GetService<TransactionFormViewModel>();
        InitializeComponent();
        _dialogService = App.GetService<IDialogService>();
        
    }

    private async void Page_Loaded(object sender, RoutedEventArgs e)
    {
        await ViewModel.LoadAsync();
        Debug.WriteLine($"Expense: {ExpenseRadButton.IsChecked} | Deposit: {DepositRadButton.IsChecked}");
    }

    private void addInvalidNumberError()
    {
        tbInvalidNumberError.Visibility = Visibility.Visible;
    }
    private void removeInvalidNumberError()
    {
        tbInvalidNumberError.Visibility = Visibility.Collapsed;
    }

    private void addInvalidAccountError()
    {
        tbInvalidAccountError.Visibility = Visibility.Visible;
    }
    private void removeInvalidAccountError()
    {
        tbInvalidAccountError.Visibility = Visibility.Collapsed;
    }

    private void addInvalidCategoryError()
    {
        tbInvalidCategoryError.Visibility = Visibility.Visible;
    }
    private void removeInvalidCategoryError()
    {
        tbInvalidCategoryError.Visibility = Visibility.Collapsed;
    }
    private void addInvalidPayeeError()
    {
        tbInvalidPayeeError.Visibility = Visibility.Visible;
    }
    private void removeInvalidPayeeError()
    {
        tbInvalidPayeeError.Visibility = Visibility.Collapsed;
    }


    //Validation
    private void TransactionPayeeTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (TransactionPayeeTextBox.Text.Equals(""))
        {
            addInvalidPayeeError();
        }
        else
        {
            removeInvalidPayeeError();
        }
    }

    private void ExpenseAmount_TextChanged(object sender, TextChangedEventArgs e)
    {
        
        try
        {
            if (!Double.IsNaN(Decimal.ToDouble(Decimal.Parse(ExpenseAmount.Text))))
            {
                if (ExpenseAmount.Text.Split(".")[1].Length > 3)
                {
                    addInvalidNumberError();
                }
                else
                {
                    removeInvalidNumberError();
                }
            }
        }
        catch(Exception ex)
        {
            addInvalidNumberError();
        }
        
    }

    private void BankAccountComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (BankAccountComboBox.SelectedItem.ToString() == null)
        {
            addInvalidAccountError();
        }
        else
        {
            removeInvalidAccountError();
        }
    }

    private void ExpenseCategoryCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ExpenseCategoryCombo.SelectedItem.ToString() == null)
        {
            addInvalidCategoryError();
        }
        else
        {
            removeInvalidCategoryError();
        }
    }

    private void validData()
    {
        if (TransactionPayeeTextBox.Text.Equals("")) return;
        try
        {
            if (!Double.IsNaN(Decimal.ToDouble(Decimal.Parse(ExpenseAmount.Text))))
            {
                removeInvalidNumberError();
            }
        }
        catch (Exception ex)
        {
            return;
        }
        if (BankAccountComboBox.SelectedItem.ToString() == null) return;
        if (ExpenseCategoryCombo.SelectedItem.ToString() == null) return;


    }
}

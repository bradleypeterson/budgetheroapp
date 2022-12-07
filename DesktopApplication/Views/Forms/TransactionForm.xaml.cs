using DesktopApplication.Contracts.Views;
using DesktopApplication.Helpers;
using DesktopApplication.Models;
using DesktopApplication.ViewModels.Forms;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using ModelsLibrary;

namespace DesktopApplication.Views.Forms;

public sealed partial class TransactionForm : Page, IDialogForm
{

    public TransactionFormViewModel ViewModel{ get; }

    private bool isValidDate;
    private bool isValidPayee;
    private bool isValidExpenseAmount;
    private bool isValidDepositAmount;
    private bool isValidAccountSelection;
    private bool isValidCategorySelection;

    public TransactionForm()
    {
        ViewModel = App.GetService<TransactionFormViewModel>();
        InitializeComponent();
        ExpenseAmount.PlaceholderText = "Enter Amount";
        DepositAmount.PlaceholderText = "Enter Amount";
    }

    private async void Page_Loaded(object sender, RoutedEventArgs e)
    {
        await ViewModel.LoadAsync();
    }

    public void ValidateForm()
    {
        ValidateDatePickerValue();
        ValidatePayeeField();
        ValidateExpenseAmountField();

        if (!isValidExpenseAmount)
        {
            isValidExpenseAmount = true;
            ValidateDepositAmountField();
        }
        else
            isValidDepositAmount = true;
            
        ValidateAccountSelection();
        ValidateCategorySelection();
    }

    public bool IsValidForm()
        => isValidDate && isValidPayee && isValidExpenseAmount && isValidDepositAmount
        && isValidAccountSelection && isValidCategorySelection;

    public void SetModel(object model)
    {
        Transaction _model = (Transaction)model;
        ViewModel.ObservableTransaction = new ObservableTransaction(_model);
    }

    private void ValidateDatePickerValue()
    {
        isValidDate = FormValidator.ValidateDate(TransactionDatePicker.Date);
        if (isValidDate)
            HideInvalidDateError();
        else
            ShowInvalidDateError();
    }

    private void ValidatePayeeField()
    {
        isValidPayee = FormValidator.ValidateString(TransactionPayeeTextBox.Text);
        if (isValidPayee)
            HideInvalidPayeeError();
        else
            ShowInvalidPayeeError();
    }

    private void ValidateExpenseAmountField()
    {
        isValidExpenseAmount = FormValidator.ValidateDecimal(ExpenseAmount.Text);
        if (isValidExpenseAmount)
            HideInvalidAmountError();
        else
            ShowInvalidAmountError();
    }

    private void ValidateDepositAmountField()
    {
        isValidDepositAmount = FormValidator.ValidateDecimal(DepositAmount.Text);
        if (isValidDepositAmount) 
            HideInvalidAmountError();
        else
            ShowInvalidAmountError();
    }

    private void ValidateAccountSelection()
    {
        isValidAccountSelection = FormValidator.ValidateSelection(BankAccountComboBox.SelectedIndex);
        if (isValidAccountSelection)
            HideInvalidAccountError();
        else
            ShowInvalidAccountError();
    }

    private void ValidateCategorySelection()
    {
        isValidCategorySelection = FormValidator.ValidateSelection(ExpenseCategoryCombo.SelectedIndex);
        if (isValidCategorySelection) 
            HideInvalidCategoryError();
        else
            ShowInvalidCategoryError();
    }

    private void ShowInvalidDateError()
    {
        dpInvalidDateError.Visibility = Visibility.Visible;
    }

    private void HideInvalidDateError()
    {
        dpInvalidDateError.Visibility = Visibility.Collapsed;
    }

    private void ShowInvalidAmountError()
    {
        tbInvalidNumberError.Visibility = Visibility.Visible;
    }
    private void HideInvalidAmountError()
    {
        tbInvalidNumberError.Visibility = Visibility.Collapsed;
    }

    private void ShowInvalidAccountError()
    {
        tbInvalidAccountError.Visibility = Visibility.Visible;
    }
    private void HideInvalidAccountError()
    {
        tbInvalidAccountError.Visibility = Visibility.Collapsed;
    }

    private void ShowInvalidCategoryError()
    {
        tbInvalidCategoryError.Visibility = Visibility.Visible;
    }
    private void HideInvalidCategoryError()
    {
        tbInvalidCategoryError.Visibility = Visibility.Collapsed;
    }
    private void ShowInvalidPayeeError()
    {
        tbInvalidPayeeError.Visibility = Visibility.Visible;
    }
    private void HideInvalidPayeeError()
    {
        tbInvalidPayeeError.Visibility = Visibility.Collapsed;
    }

    private void TransactionType_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        TransactionPayeeTextBox.Focus(FocusState.Programmatic);
    }

    private void ExpenseRadButton_Checked(object sender, RoutedEventArgs e)
    {
        DepositAmount.Text = string.Empty;
    }

    private void DepositRadButton_Checked(object sender, RoutedEventArgs e)
    {
        ExpenseAmount.Text = string.Empty;
    }
}
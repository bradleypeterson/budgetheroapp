using DesktopApplication.ViewModels.Forms;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using DesktopApplication.Contracts.Views;
using ModelsLibrary;

namespace DesktopApplication.Views.Forms;

public sealed partial class BankAccountForm : Page, IDialogForm
{
    public BankAccountFormViewModel ViewModel { get; }

    private bool isValidAccountName;
    private bool isValidAccountSelection;
    private bool isValidBalance;

    public BankAccountForm()
    {
        ViewModel = App.GetService<BankAccountFormViewModel>();
        InitializeComponent();
    }

    public void ValidateForm()
    {
        ValidateAccountNameField();
        ValidateAccountSelection();
        ValidateAccountBalanceField();
    }

    private void ValidateAccountBalanceField()
    {
        string balance = txtAccountBalance.Text;

        if (balance == string.Empty)
        {
            tbAccountBalanceEmptyError.Visibility = Visibility.Visible;
            tbAccountBalanceInvalidError.Visibility = Visibility.Collapsed;
            isValidBalance = false;
        }
        else
        {
            tbAccountBalanceEmptyError.Visibility = Visibility.Collapsed;
            if (decimal.TryParse(balance, out decimal _))
            {
                tbAccountBalanceInvalidError.Visibility = Visibility.Collapsed;
                isValidBalance = true;
            }
            else
            {
                tbAccountBalanceInvalidError.Visibility = Visibility.Visible;
                isValidBalance = false;
            }
        }
    }

    private void ValidateAccountNameField()
    {
        if (txtAccountName.Text == string.Empty)
        {
            tbAccountNameEmptyError.Visibility = Visibility.Visible;
            isValidAccountName = false;
        }
        else
        {
            tbAccountNameEmptyError.Visibility = Visibility.Collapsed;
            isValidAccountName = true;
        }
    }

    private void ValidateAccountSelection()
    {
        int selection = txtAccountType.SelectedIndex;

        if (selection == -1)
        {
            tbAccountTypeEmptyError.Visibility = Visibility.Visible;
            isValidAccountSelection = false;
        }
        else
        {
            tbAccountTypeEmptyError.Visibility = Visibility.Collapsed;
            isValidAccountSelection = true;
        }
    }

    public bool IsValidForm() => isValidAccountName && isValidAccountSelection && isValidBalance;

    public void SetModel(object model)
    {
        ViewModel.BankAccount = (BankAccount)model;
    }
}

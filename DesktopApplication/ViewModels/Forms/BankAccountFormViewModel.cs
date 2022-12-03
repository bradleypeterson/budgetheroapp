using CommunityToolkit.Mvvm.ComponentModel;
using ModelsLibrary;

namespace DesktopApplication.ViewModels.Forms;
public class BankAccountFormViewModel : ObservableRecipient
{
    private BankAccount _bankAccount = new();
    public BankAccount BankAccount
    {
        get => _bankAccount!;
        set
        {
            _bankAccount = value;
            BankName = _bankAccount.BankName;
            AccountType = _bankAccount.AccountType;
            Balance = _bankAccount.Balance;
            BalanceString = _bankAccount.Balance.ToString();
        }
    }

    private string? _bankName;
    public string? BankName
    {
        get => _bankName;
        set
        {
            SetProperty(ref _bankName, value);
            _bankAccount.BankName = _bankName!;
        }
    }

    private string? _accountType;
    public string? AccountType
    {
        get => _accountType;
        set
        {
            SetProperty(ref _accountType, value);
            _bankAccount.AccountType = _accountType!;
        }
    }

    private decimal _balance;
    public decimal Balance
    {
        get => _balance;
        set
        {
            SetProperty(ref _balance, value);
            _bankAccount.Balance = _balance!;
        }
    }

    private string? _balanceString;
    public string? BalanceString
    {
        get => _balanceString;
        set
        {
            SetProperty(ref _balanceString, value);
            ConvertToDecimal(value!);
        }
    }

    private void ConvertToDecimal(string value)
    {
        decimal tempBalance;
        if (decimal.TryParse(value, out tempBalance))
            Balance = tempBalance;
    }
}       
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using DesktopApplication.Contracts.ViewModels;
using ModelsLibrary;

namespace DesktopApplication.ViewModels.Forms;
public class BankAccountFormViewModel : ObservableRecipient
{
    private BankAccount _bankAccount = new();

    public event EventHandler? OnValidForm;
    public event EventHandler? OnInvalidForm;
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
            if (ValidFormCompletion())
            {
                _bankAccount.BankName = _bankName!;
            }
        }
    }
    
    private string? _accountType;
    public string? AccountType
    {
        get => _accountType;
        set
        {
            SetProperty(ref _accountType, value);
            if (ValidFormCompletion())
            {
                _bankAccount.AccountType = _accountType!;
            }
        }
    }

    private decimal _balance;
    public decimal Balance
    {
        get => _balance;
        set
        {
            SetProperty(ref _balance, value);
            if (ValidFormCompletion())
            {
                _bankAccount.Balance = _balance!;
            }
        }
    }

    private string? _balanceString;
    public string? BalanceString
    {
        get => _balanceString;
        set
        {
            SetProperty(ref _balanceString, value);
            if (ValidFormCompletion())
            {
                ConvertToDecimal(value!);
            }
        }
    }

    private void ConvertToDecimal(string value)
    {
        decimal tempBalance;
        if (decimal.TryParse(value, out tempBalance))
        {
            Balance = tempBalance;
        }
        else
        {
            Debug.WriteLine("Error: You must provide a decimal value.");
        }
    }

    private bool ValidFormCompletion()
    {
        if (!string.IsNullOrEmpty(_bankName)
            && !string.IsNullOrEmpty(_accountType)
            && !string.IsNullOrEmpty(_balanceString))
        {
            OnValidForm?.Invoke(this, EventArgs.Empty);
            return true;
        }
        else
        {
            OnInvalidForm?.Invoke(this, EventArgs.Empty);
            return false;
        }
    }
}

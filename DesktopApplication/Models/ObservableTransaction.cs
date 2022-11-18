using CommunityToolkit.Mvvm.ComponentModel;
using DesktopApplication.Converters;
using ModelsLibrary;

namespace DesktopApplication.Models;
public class ObservableTransaction : ObservableObject
{
    public ObservableTransaction(Transaction transaction = null!)
    {
        Transaction = transaction ?? new Transaction() { TransactionDate = DateTime.Now };
    }

    private Transaction _transaction = new();
    public Transaction Transaction
    {
        get => _transaction;
        set
        {
            _transaction = value;
            TransactionDate = _transaction.TransactionDate;
            TransactionPayee = _transaction.TransactionPayee;
            TransactionMemo = _transaction.TransactionMemo;
            ExpenseAmount = _transaction.ExpenseAmount.ToString("G");
            DepositAmount = _transaction.DepositAmount.ToString("G");
            IsTransactionPaid = _transaction.IsTransactionPaid;
            IsHousehold = _transaction.IsHousehold;
            if (_transaction.BudgetCategory is not null)
                CategoryName = _transaction.BudgetCategory.CategoryName;
        }
    }

    private DateTime _transactionDate;
    public DateTime TransactionDate
    {
        get => _transactionDate;
        set
        {
            SetProperty(ref _transactionDate, value);
            _transaction.TransactionDate = _transactionDate;
        }
    }

    private string? _transactionPayee;
    public string? TransactionPayee
    {
        get => _transactionPayee;
        set
        {
            SetProperty(ref _transactionPayee, value);
            _transaction.TransactionPayee = _transactionPayee!;
        } 
    }

    private string? _transactionMemo;
    public string? TransactionMemo
    {
        get => _transactionMemo;
        set
        {
            SetProperty(ref _transactionMemo, value);
            _transaction.TransactionMemo = _transactionMemo!;
        }
    }

    private decimal _expenseAmount;
    public string? ExpenseAmount
    {
        get => (_expenseAmount == 0) ? string.Empty : _expenseAmount.ToString("G");
        set
        {
            var temp = PropertyConverter.ConvertToDecimal(value!);
            if (temp != -1)
            {
                SetProperty(ref _expenseAmount, temp);
                _transaction.ExpenseAmount = _expenseAmount;
            }
        }
    }

    private decimal _depositAmount;
    public string? DepositAmount
    {
        get => (_depositAmount == 0) ? string.Empty : _depositAmount.ToString("G");
        set
        {
            var temp = PropertyConverter.ConvertToDecimal(value!);
            if (temp != -1)
            {
                SetProperty(ref _depositAmount, temp);
                _transaction.DepositAmount = _depositAmount;
            }
        }
    }

    private bool? _isTransactionPaid;
    public bool? IsTransactionPaid
    {
        get => _isTransactionPaid;
        set
        {
            SetProperty(ref _isTransactionPaid, value);
            _transaction.IsTransactionPaid = _isTransactionPaid;
        }
    }

    private bool _isHousehold;
    public bool IsHousehold
    {
        get => _isHousehold;
        set
        {
            SetProperty(ref _isHousehold, value);
            _transaction.IsHousehold = _isHousehold;
        }
    }

    private string? _categoryName;
    public string? CategoryName
    {
        get => _categoryName;
        set
        {
            SetProperty(ref _categoryName, value);
            _transaction.BudgetCategory.CategoryName = _categoryName!;
        }
    }
}
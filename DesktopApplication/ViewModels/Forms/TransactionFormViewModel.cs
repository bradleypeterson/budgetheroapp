using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using DesktopApplication.Contracts.Data;
using DesktopApplication.Contracts.Services;
using DesktopApplication.Helpers;
using DesktopApplication.Models;
using ModelsLibrary;

namespace DesktopApplication.ViewModels.Forms;
public class TransactionFormViewModel : ObservableRecipient
{
    private readonly IDataStore _dataStore;
    private readonly ISessionService _sessionService;

    public TransactionFormViewModel(ObservableTransaction observableTransaction = null!)
    {
        ObservableTransaction = observableTransaction ?? new ObservableTransaction();
        _dataStore = App.GetService<IDataStore>();
        _sessionService = App.GetService<ISessionService>();
    }

    public ObservableTransaction ObservableTransaction { get; set; }
    public ObservableCollection<BankAccount> BankAccounts { get; } = new();
    public ObservableCollection<BudgetCategory> BudgetCategories { get; } = new();

    private BankAccount? _selectedBankAccount;
    public BankAccount? SelectedBankAccount
    {
        get => _selectedBankAccount;
        set
        {
            SetProperty(ref _selectedBankAccount, value);
            ObservableTransaction.Transaction.BankAccountId = value!.BankAccountId;
        }
    }

    private BudgetCategory? _selectedCategory;
    public BudgetCategory? SelectedCategory
    {
        get => _selectedCategory;
        set
        {
            SetProperty(ref _selectedCategory, value);
            ObservableTransaction.Transaction.BudgetCategoryId = value!.BudgetCategoryID;
        }
    }

    private bool _hasExpenseChecked;
    public bool HasExpenseChecked
    {
        get => _hasExpenseChecked;
        set
        {
            SetProperty(ref _hasExpenseChecked, value);
            if (ObservableTransaction.DepositAmount != string.Empty && 
                ObservableTransaction.ExpenseAmount != string.Empty)
                ObservableTransaction.DepositAmount = "0";
        }
    }

    private bool _hasDepositChecked;
    public bool HasDepositChecked
    {
        get => _hasDepositChecked;
        set
        {
            SetProperty(ref _hasDepositChecked, value);
            if (ObservableTransaction.ExpenseAmount != string.Empty &&
                ObservableTransaction.DepositAmount != string.Empty)
                ObservableTransaction.ExpenseAmount = "0";
        }
    }

    public async Task LoadAsync()
    {
        // Load Bank Accounts
        if (BankAccounts.Any()) return;

        IEnumerable<BankAccount?> _userBankAccounts = await _dataStore.BankAccount
            .ListAsync(a => a.UserId == _sessionService.GetSessionUserId());

        CollectionUtilities.LoadObservableCollection(_userBankAccounts, BankAccounts!);

        // Load Budget Categories
        if (BudgetCategories.Any()) return;

        IEnumerable<BudgetCategory> _userCategories = _dataStore.BudgetCategory.List();

        CollectionUtilities.LoadObservableCollection(_userCategories, BudgetCategories);
    }
}

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
        set => SetProperty(ref _hasExpenseChecked, value);
    }

    private bool _hasDepositChecked;
    public bool HasDepositChecked
    {
        get => _hasDepositChecked;
        set => SetProperty(ref _hasDepositChecked, value);
    }

    public async Task LoadAsync()
    {
        if (BankAccounts.Any()) return;

        var _usersBankAccounts = await _dataStore.BankAccount
            .ListAsync(a => a.UserId == _sessionService.GetSessionUserId());

        if (_usersBankAccounts is not null)
        {
            foreach (BankAccount? bankAccount in _usersBankAccounts)
            {
                BankAccounts.Add(bankAccount!);
            }
        }

        if (BudgetCategories.Any()) return;

        var _userCategories = _dataStore.BudgetCategory.List();

        if (_userCategories is not null)
        {
            foreach (var category in _userCategories)
            {
                //BudgetCategories.Add(category!);
                Debug.WriteLine("Item found!");
            }
        }
    }
}

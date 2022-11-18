using System.Collections.ObjectModel;
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

    public TransactionFormViewModel()
    {
        _dataStore = App.GetService<IDataStore>();
        _sessionService = App.GetService<ISessionService>();
    }

    public ObservableTransaction? ObservableTransaction { get; set; } = new();
    public ObservableCollection<BankAccount> BankAccounts { get; } = new();
    public ObservableCollection<BudgetCategory> BudgetCategories { get; } = new();

    private BankAccount? _selectedBankAccount;
    public BankAccount? SelectedBankAccount
    {
        get => _selectedBankAccount;
        set
        {
            SetProperty(ref _selectedBankAccount, value);
            if (value is not null)
                ObservableTransaction!.Transaction.BankAccountId = value.BankAccountId;
        }
    }

    private BudgetCategory? _selectedCategory;
    public BudgetCategory? SelectedCategory
    {
        get => _selectedCategory;
        set
        {
            SetProperty(ref _selectedCategory, value);
            if (value is not null)
                ObservableTransaction!.Transaction.BudgetCategoryId = value.BudgetCategoryID;
        }
    }

    private bool _hasExpenseChecked;
    public bool HasExpenseChecked
    {
        get => _hasExpenseChecked;
        set
        {
            SetProperty(ref _hasExpenseChecked, value);
            if (ObservableTransaction!.DepositAmount != string.Empty && 
                ObservableTransaction!.ExpenseAmount != string.Empty)
                ObservableTransaction!.DepositAmount = "0";
        }
    }

    private bool _hasDepositChecked;
    public bool HasDepositChecked
    {
        get => _hasDepositChecked;
        set
        {
            SetProperty(ref _hasDepositChecked, value);
            if (ObservableTransaction!.ExpenseAmount != string.Empty &&
                ObservableTransaction.DepositAmount != string.Empty)
                ObservableTransaction.ExpenseAmount = "0";
        }
    }

    public async Task LoadAsync()
    {
        await LoadBankAccountsCollectionAsync();
        LoadCategoriesCollection();
        OnCollectionsLoaded();
    }

    private async Task LoadBankAccountsCollectionAsync()
    {
        if (BankAccounts.Any()) return;

        IEnumerable<BankAccount?> _userBankAccounts = await _dataStore.BankAccount
            .ListAsync(a => a.UserId == _sessionService.GetSessionUserId());

        CollectionUtilities.LoadObservableCollection(_userBankAccounts, BankAccounts!);
    }

    private void LoadCategoriesCollection()
    {
        if (BudgetCategories.Any()) return;

        IEnumerable<BudgetCategory> _userCategories = _dataStore.BudgetCategory.List();

        CollectionUtilities.LoadObservableCollection(_userCategories, BudgetCategories);
    }

    private void OnCollectionsLoaded()
    {
        SetSelectedBankAccount();
        SetSelectedCategory();
    }

    private void SetSelectedBankAccount()
    {
        int bankAccountId = 0;

        if (ObservableTransaction is not null)
            if (ObservableTransaction.Transaction is not null)
                bankAccountId = ObservableTransaction.Transaction.BankAccountId;

        SelectedBankAccount = BankAccounts.FirstOrDefault(b => b.BankAccountId == bankAccountId);
    }

    private void SetSelectedCategory()
    {
        int categoryId = 0;

        if (ObservableTransaction is not null)
            if (ObservableTransaction.Transaction is not null)
                categoryId = ObservableTransaction.Transaction.BudgetCategoryId;

        SelectedCategory = BudgetCategories.FirstOrDefault(c => c.BudgetCategoryID == categoryId);
    }
}
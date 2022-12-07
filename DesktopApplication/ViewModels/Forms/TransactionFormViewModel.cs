using CommunityToolkit.Mvvm.ComponentModel;
using DesktopApplication.Contracts.Data;
using DesktopApplication.Contracts.Services;
using DesktopApplication.Helpers;
using DesktopApplication.Models;
using ModelsLibrary;
using System.Collections.ObjectModel;

namespace DesktopApplication.ViewModels.Forms;
public class TransactionFormViewModel : ObservableRecipient
{
    private readonly IDataStore _dataStore;
    private readonly ISessionService _sessionService;
    public IDialogService _dialogservice;

    public TransactionFormViewModel()
    {
        _dataStore = App.GetService<IDataStore>();
        _sessionService = App.GetService<ISessionService>();
        _dialogservice= App.GetService<IDialogService>();
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

    private int _transactionType = 0;
    public int TransactionType
    {
        get => _transactionType;
        set => SetProperty(ref _transactionType, value);
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

        User? _user = _dataStore.User.Get(u => u.UserId == _sessionService.GetSessionUserId(), false, "Budgets");

        if (_user is not null && _user.Budgets is not null)
        {
            foreach (Budget budget in _user.Budgets)
            {
                if(budget.BudgetType == "personal") 
                { 
                    Budget? _userBudget = _dataStore.Budget.Get(b => b.BudgetId == budget.BudgetId, false, "BudgetCategoryGroups");

                    if (_userBudget is not null && _userBudget.BudgetCategoryGroups is not null)
                    {
                        foreach (BudgetCategoryGroup categoryGroup in _userBudget.BudgetCategoryGroups)
                        {
                            IEnumerable<BudgetCategory> _userCategories = _dataStore.BudgetCategory.GetAll(c => c.BudgetCategoryGroupID == categoryGroup.BudgetCategoryGroupID);

                            if (_userCategories is not null)
                                _userCategories.ToList().ForEach(c => BudgetCategories.Add(c));
                        }
                    }
                }
            }
        }
        else
            throw new Exception("The logged in user could not be found in the database.");
    }

    private void OnCollectionsLoaded()
    {
        SetSelectedBankAccount();
        SetSelectedCategory();
        SetTransactionType();
    }

    private void SetSelectedBankAccount()
    {
        if (ObservableTransaction is not null)
            if (ObservableTransaction.Transaction is not null)
            {
                Guid bankAccountId = ObservableTransaction.Transaction.BankAccountId;
                SelectedBankAccount = BankAccounts.FirstOrDefault(b => b.BankAccountId == bankAccountId);
            }
    }

    private void SetSelectedCategory()
    {
        if (ObservableTransaction is not null)
            if (ObservableTransaction.Transaction is not null)
            {
                Guid categoryId = ObservableTransaction.Transaction.BudgetCategoryId;
                SelectedCategory = BudgetCategories.FirstOrDefault(c => c.BudgetCategoryID == categoryId);
            }
    }

    private void SetTransactionType()
    {
        string? expenseAmount = ObservableTransaction!.ExpenseAmount;
        string? depositAmount = ObservableTransaction!.DepositAmount;

        if (ObservableTransaction is not null)
            if (string.IsNullOrEmpty(expenseAmount) && !string.IsNullOrEmpty(depositAmount))
            {
                HasDepositChecked = true;
                TransactionType = 1;
            }
    }
}
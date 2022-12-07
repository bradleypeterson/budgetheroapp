using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DesktopApplication.Contracts.Data;
using DesktopApplication.Contracts.Services;
using DesktopApplication.CustomEventArgs;
using DesktopApplication.Models;
using DesktopApplication.ViewModels.Forms;
using DesktopApplication.Views.Forms;
using ModelsLibrary;
using ModelsLibrary.Utilities;

namespace DesktopApplication.ViewModels;

public class AccountsViewModel : ObservableRecipient
{
    private readonly ISessionService _sessionService;
    private readonly IDialogService _dialogService;
    private readonly IDataStore _dataStore;
    private readonly IAPIService _apiService;
    private readonly Guid _userId;

    public AccountsViewModel()
    {
        _sessionService = App.GetService<ISessionService>();
        _dialogService = App.GetService<IDialogService>();
        _dataStore = App.GetService<IDataStore>();
        _apiService = App.GetService<IAPIService>();

        ShowAddDialogCommand = new AsyncRelayCommand(ShowAddDialog);
        ShowEditDialogCommand = new AsyncRelayCommand(ShowEditDialog);
        ShowDeleteDialogCommand = new AsyncRelayCommand(ShowDeleteDialog);
        ShowTransferDialogCommand = new AsyncRelayCommand(ShowTransferDialog);

        _userId = _sessionService.GetSessionUserId();
    }

    public IAsyncRelayCommand ShowAddDialogCommand { get; }
    public IAsyncRelayCommand ShowEditDialogCommand { get; }
    public IAsyncRelayCommand ShowDeleteDialogCommand { get; }
    public IAsyncRelayCommand ShowTransferDialogCommand { get; }

    private IEnumerable<Transaction>? allTransactions;
    public ObservableCollection<ObservableBankAccount> BankAccounts { get; set; } = new();
    public ObservableCollection<Transaction> AccountTransactions { get; set; } = new();
    
    private ObservableBankAccount? _selectedBankAccount;
    public ObservableBankAccount? SelectedBankAccount
    {
        get => _selectedBankAccount;
        set
        {
            SetProperty(ref _selectedBankAccount, value);
            if (value is not null)
                FilterAccountTransactions(value.BankAccount.BankAccountId);
            HasItemSelected = value is not null;
        }
    }

    private bool _hasItemSelected;
    public bool HasItemSelected
    {
        get => _hasItemSelected;
        set => SetProperty(ref _hasItemSelected, value);
    }

    private bool _hasMultipleAccounts;
    public bool HasMultipleAccounts
    {
        get => _hasMultipleAccounts;
        set => SetProperty(ref _hasMultipleAccounts, value);
    }

    public async Task LoadAsync()
    {
        if (BankAccounts.Any()) return;

        IEnumerable<BankAccount> _databaseAccounts;
        IEnumerable<Transaction> _databaseTransactions;
        IEnumerable<BudgetCategory> _databaseCategories;
        IEnumerable<Transaction> _apiTransactions;
        IEnumerable<BankAccount> _apiAccounts;
        IEnumerable<BudgetCategory> _apiCategories;
        Budget personalBudget = _dataStore.Budget.GetPersonalBudget(_userId);
        
        _databaseAccounts = await _dataStore.BankAccount.ListAsync(a => a.UserId == _userId);
        _databaseCategories = await _dataStore.Budget.GetBudgetCategories(personalBudget);
        _apiTransactions = await _apiService.GetTransactions(_userId);
        _apiAccounts = await _apiService.GetUserAccounts(_userId);
        _apiCategories = await _apiService.GetCategories(personalBudget);

        if (_databaseAccounts.Any())
        {
            _databaseAccounts.ToList().ForEach(a => BankAccounts.Add(new ObservableBankAccount(a)));

            await _apiService.UpdateAccounts(_apiAccounts, _databaseAccounts);

            _databaseTransactions = await _dataStore.Transaction
            .ListAsync(t => t.BankAccount!.UserId == _userId, null!, "BankAccount");

            if (_databaseCategories.Any())
                await _apiService.UpdateCategories(_apiCategories, _databaseCategories);

            if (_databaseTransactions.Any())
            {
                await _apiService.UpdateTransactions(_apiTransactions, _databaseTransactions);
                allTransactions = _databaseTransactions.ToList();
            }
        } 
        else if (_apiAccounts.Any())
        {
            await _dataStore.BankAccount.SaveWithForeignKeysAsync(_apiAccounts);

            _apiAccounts.ToList().ForEach(a => BankAccounts.Add(new ObservableBankAccount(a)));

            if (_apiCategories.Any())
                await _dataStore.Budget.SaveWithForeignKeysAsync(_apiCategories, personalBudget);

            if (_apiTransactions.Any())
            {
                await _dataStore.Transaction.SaveWithForeignKeysAsync(_apiTransactions);

                allTransactions = _apiTransactions.ToList();
            }
        }
        VerifyUserAccountCount();
    }

    private async Task ShowAddDialog()
    {
        _dialogService.OnSaved += AddBankAccountAsync;
        
        string dialogTitle = "Add Account";        
        await _dialogService.ShowDialogAsync<BankAccountForm>(dialogTitle);

        _dialogService.OnSaved -= AddBankAccountAsync;
    }

    private async Task ShowEditDialog()
    {
        _dialogService.OnSaved += EditBankAccountAsync;
        
        string dialogTitle = "Edit Account";
        BankAccount mutableBankAccount = EntityUtilities.Duplicate(SelectedBankAccount!.BankAccount);
        await _dialogService.ShowDialogAsync<BankAccountForm>(dialogTitle, mutableBankAccount);
        
        _dialogService.OnSaved -= EditBankAccountAsync;
    }

    private async Task ShowDeleteDialog()
    {
        _dialogService.OnSaved += DeleteBankAccountAsync;

        string dialogTitle = "Delete Account";
        BankAccount _selectedBankAccount = SelectedBankAccount!.BankAccount;
        await _dialogService.ShowDialogAsync<DeleteItemForm>(dialogTitle, _selectedBankAccount);

        _dialogService.OnSaved -= DeleteBankAccountAsync;
    }

    private async Task ShowTransferDialog()
    {
        _dialogService.OnSaved += TransferFundsAsync;

        string dialogTitle = "Transfer Funds";
        await _dialogService.ShowDialogAsync<TransferFundsForm>(dialogTitle);

        _dialogService.OnSaved -= TransferFundsAsync;
    }

    private async void AddBankAccountAsync(object? sender, DialogServiceEventArgs e)
    {
        BankAccount newBankAccount = GetBankAccount(e);
        newBankAccount.UserId = _sessionService.GetSessionUserId();

        int result = await _dataStore.BankAccount.AddAsync(newBankAccount);

        if (result == 1)
        {
            BankAccounts.Add(new ObservableBankAccount(newBankAccount));
        }

        VerifyUserAccountCount();
    }

    private async void EditBankAccountAsync(object? sender, DialogServiceEventArgs e)
    {
        BankAccount existingBankAccount = SelectedBankAccount!.BankAccount;
        existingBankAccount = EntityUtilities.Update(existingBankAccount, GetBankAccount(e));

        ObservableBankAccount? listedBankAccount = BankAccounts.FirstOrDefault(
            a => a.BankAccount.BankAccountId == existingBankAccount.BankAccountId);
        int index;

        if (listedBankAccount is not null)
        {
            await _dataStore.BankAccount.Update(existingBankAccount);

            index = BankAccounts.IndexOf(listedBankAccount);
            BankAccounts[index].BankAccount = existingBankAccount;
        }
    }

    private async void DeleteBankAccountAsync(object? sender, DialogServiceEventArgs e)
    {
        BankAccount selectedBankAccount = _selectedBankAccount!.BankAccount;
        await _dataStore.BankAccount.DeleteAsync(selectedBankAccount);

        BankAccounts.Remove(_selectedBankAccount);

        if (BankAccounts.Count == 0)
            await _apiService.DeleteAsync($"bankaccounts/{selectedBankAccount.BankAccountId}");

        VerifyUserAccountCount();
    }

    private async void TransferFundsAsync(object? sender, DialogServiceEventArgs e)
    {
        BankAccount[] accounts = GetBankAccounts(e);
        ObservableBankAccount transferFromAccount = GetBankAccount(accounts[0].BankAccountId);
        ObservableBankAccount transferToAccount = GetBankAccount(accounts[1].BankAccountId);
        decimal transferAmount = GetTransferAmount(e);

        await UpdateAccountBalance(transferFromAccount, -transferAmount);
        await UpdateAccountBalance(transferToAccount, transferAmount);
    }

    private async Task UpdateAccountBalance(ObservableBankAccount observableBankAccount, decimal transferAmount)
    {
        BankAccount bankAccount = observableBankAccount.BankAccount;

        bankAccount.Balance += transferAmount;
        await _dataStore.BankAccount.Update(bankAccount);

        observableBankAccount.BankAccount = bankAccount;
    }

    private static BankAccount GetBankAccount(DialogServiceEventArgs e)
    {
        BankAccountForm accountForm = (BankAccountForm)e.Content;
        return accountForm.ViewModel.BankAccount;
    }

    private ObservableBankAccount GetBankAccount(Guid bankAccountId)
    {
        ObservableBankAccount? observableAccount = BankAccounts.FirstOrDefault(a => a.BankAccount.BankAccountId == bankAccountId, null);

        if (observableAccount is not null)
            return observableAccount;
        else
            throw new NullReferenceException();
    }

    private static BankAccount[] GetBankAccounts(DialogServiceEventArgs e)
    {
        TransferFundsForm transferForm = (TransferFundsForm)e.Content;
        BankAccount[] accounts = new BankAccount[2];

        accounts[0] = transferForm.ViewModel.SelectedTransferFromAccount!;
        accounts[1] = transferForm.ViewModel.SelectedTransferToAccount!;

        return accounts;
    }

    private static decimal GetTransferAmount(DialogServiceEventArgs e)
    {
        TransferFundsForm transferForm = (TransferFundsForm)e.Content;

        if (decimal.TryParse(transferForm.ViewModel.TransferAmount, out decimal transferAmount))
            return transferAmount;
        else
            throw new Exception("Somehow a non-decimal value made it this far...");
    }

    private void FilterAccountTransactions(Guid accountId)
    {
        if (allTransactions is not null)
        {
            AccountTransactions.Clear();
            List<Transaction> accountTransactions = allTransactions.Where(t => t.BankAccountId == accountId)
                                                                   .OrderByDescending(t => t.TransactionDate).ToList();
            accountTransactions.ForEach(AccountTransactions.Add);
        }
    }

    private void VerifyUserAccountCount()
    {
        if (BankAccounts.Count > 1)
            HasMultipleAccounts = true;
        else
            HasMultipleAccounts = false;
    }
}
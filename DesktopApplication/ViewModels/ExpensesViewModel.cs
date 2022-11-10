using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DesktopApplication.Contracts.Data;
using DesktopApplication.Contracts.Services;
using DesktopApplication.CustomEventArgs;
using DesktopApplication.Models;
using DesktopApplication.Views.Forms;
using ModelsLibrary;

namespace DesktopApplication.ViewModels;

public class ExpensesViewModel : ObservableRecipient
{
    private readonly ISessionService _sessionService;
    private readonly IDialogService _dialogService;
    private readonly IDataStore _dataStore;

    public ExpensesViewModel()
    {
        _sessionService = App.GetService<ISessionService>();
        _dialogService = App.GetService<IDialogService>();
        _dataStore = App.GetService<IDataStore>();

        ShowAddDialogCommand = new AsyncRelayCommand(ShowAddDialog);
        ShowEditDialogCommand = new AsyncRelayCommand(ShowEditDialog);
        ShowDeleteDialogCommand = new AsyncRelayCommand(ShowDeleteDialog);
    }

    public IAsyncRelayCommand ShowAddDialogCommand { get; }
    public IAsyncRelayCommand ShowEditDialogCommand { get; }
    public IAsyncRelayCommand ShowDeleteDialogCommand { get; }
    public ObservableCollection<ObservableTransaction> Transactions { get; set; } = new();
    
    private ObservableTransaction? _selectedTransaction;
    public ObservableTransaction? SelectedTransaction
    {
        get => _selectedTransaction;
        set
        {
            SetProperty(ref _selectedTransaction, value);
            if (value is not null)
            {
                HasItemSelected = true;
            }
            else
            {
                HasItemSelected = false;
            }
        }
    }

    private bool _hasItemSelected;
    public bool HasItemSelected
    {
        get => _hasItemSelected;
        set => SetProperty(ref _hasItemSelected, value);
    }

    public async Task LoadAsync()
    {
        await Task.Delay(1500);
        Transactions.Add(new ObservableTransaction(new Transaction()
        {
            TransactionDate = DateTime.Now,
            TransactionPayee = "Walmart",
            TransactionMemo = "Toaster",
            ExpenseAmount = 20.34m,
            DepositAmount = 30.43m,
            BudgetCategory = new() { CategoryName = "Household Item"},
        }));
    }

    private async Task ShowAddDialog()
    {
        _dialogService.OnSaved += AddTransactionAsync;

        var dialogTitle = "Add Transaction";
        await _dialogService.ShowDialogAsync<TransactionForm>(dialogTitle);

        _dialogService.OnSaved -= AddTransactionAsync;
    }

    private async Task ShowEditDialog()
    {
        _dialogService.OnSaved += EditTransactionAsync;

        var dialogTitle = "Edit Transaction";
        var _selectedBankAccount = SelectedTransaction!.Transaction;
        await _dialogService.ShowDialogAsync<TransactionForm>(dialogTitle, _selectedBankAccount);

        _dialogService.OnSaved -= EditTransactionAsync;
    }

    private async Task ShowDeleteDialog()
    {
        _dialogService.OnSaved += DeleteTransactionAsync;

        var dialogTitle = "Delete Transaction";
        var _selectedBankAccount = SelectedTransaction!.Transaction;
        await _dialogService.ShowDialogAsync<TransactionForm>(dialogTitle, _selectedBankAccount, true);

        _dialogService.OnSaved -= DeleteTransactionAsync;
    }

    private void AddTransactionAsync(object? sender, DialogServiceEventArgs e)
    {
        //var newBankAccount = GetBankAccount(e);
        //newBankAccount.UserId = _sessionService.GetSessionUserId();

        //var result = await _dataStore.BankAccount.AddAsync(newBankAccount);

        //if (result == 1)
        //{
        //    BankAccounts.Add(new ObservableBankAccount(newBankAccount));
        //}
    }

    private void EditTransactionAsync(object? sender, DialogServiceEventArgs e)
    {
        //var editedBankAccount = GetBankAccount(e);
        //var listedBankAccount = BankAccounts.FirstOrDefault(
        //    a => a.BankAccount.BankAccountId == editedBankAccount.BankAccountId);
        //int index;

        //if (listedBankAccount is not null)
        //{
        //    await _dataStore.BankAccount.Update(editedBankAccount);

        //    index = BankAccounts.IndexOf(listedBankAccount);
        //    BankAccounts[index].BankAccount = editedBankAccount;
        //}
    }

    private void DeleteTransactionAsync(object? sender, DialogServiceEventArgs e)
    {
        //var selectedBankAccount = _selectedBankAccount!.BankAccount;
        //await _dataStore.BankAccount.DeleteAsync(selectedBankAccount);

        //BankAccounts.Remove(_selectedBankAccount);
    }
}

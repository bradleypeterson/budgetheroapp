using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DesktopApplication.Contracts.Data;
using DesktopApplication.Contracts.Services;
using DesktopApplication.CustomEventArgs;
using DesktopApplication.Helpers;
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
                BankAccountName = value.Transaction.BankAccount.BankName;
                BankAccountBalance = value.Transaction.BankAccount.Balance.ToString("C2");
            }
        }
    }

    private bool _hasItemSelected;
    public bool HasItemSelected
    {
        get => _hasItemSelected;
        set => SetProperty(ref _hasItemSelected, value);
    }

    private string? _bankAccountName;
    public string BankAccountName
    {
        get => _bankAccountName ?? string.Empty;
        set => SetProperty(ref _bankAccountName, value);
    }

    private string? _bankAccountBalance;
    public string BankAccountBalance
    {
        get => _bankAccountBalance ?? string.Empty;
        set => SetProperty(ref _bankAccountBalance, value);
    } 

    public async Task LoadAsync()
    {
        if (Transactions.Any()) return;

        int userId = _sessionService.GetSessionUserId();
        IEnumerable<Transaction?> transactions = 
            await _dataStore.Transaction.ListAsync(t => t.BankAccount.UserId == userId, null!, "BankAccount,BudgetCategory");

        if (transactions is not null)
        {
            foreach (var transaction in transactions)
            {
                if (transaction is not null) 
                    Transactions.Add(new ObservableTransaction(transaction));
            }
        }
    }

    private async Task ShowAddDialog()
    {
        _dialogService.OnSaved += AddTransactionAsync;

        string dialogTitle = "Add Transaction";
        await _dialogService.ShowDialogAsync<TransactionForm>(dialogTitle);

        _dialogService.OnSaved -= AddTransactionAsync;
    }

    private async Task ShowEditDialog()
    {
        _dialogService.OnSaved += EditTransactionAsync;

        string dialogTitle = "Edit Transaction";
        Transaction mutableTransaction = EntityUtilities.Duplicate(SelectedTransaction!.Transaction);

        await _dialogService.ShowDialogAsync<TransactionForm>(dialogTitle, mutableTransaction);

        _dialogService.OnSaved -= EditTransactionAsync;
    }

    private async Task ShowDeleteDialog()
    {
        _dialogService.OnSaved += DeleteTransactionAsync;

        string dialogTitle = "Delete Transaction";
        Transaction _selectedTransaction = SelectedTransaction!.Transaction;
        await _dialogService.ShowDialogAsync<TransactionForm>(dialogTitle, _selectedTransaction, true);

        _dialogService.OnSaved -= DeleteTransactionAsync;
    }

    private async void AddTransactionAsync(object? sender, DialogServiceEventArgs e)
    {
        Transaction newTransaction = GetTransaction(e);

        int result = await _dataStore.Transaction.AddAsync(newTransaction);

        if (result == 1)
        {
            Transactions.Add(new ObservableTransaction(newTransaction));
        }
    }

    private async void EditTransactionAsync(object? sender, DialogServiceEventArgs e)
    {
        Transaction existingTransaction = SelectedTransaction!.Transaction;
        existingTransaction = EntityUtilities.Update(existingTransaction, GetTransaction(e));

        ObservableTransaction? listedTransaction = Transactions.FirstOrDefault(
            a => a.Transaction.TransactionId == existingTransaction.TransactionId);
        int index;

        if (listedTransaction is not null)
        {
            await _dataStore.Transaction.Update(existingTransaction);

            index = Transactions.IndexOf(listedTransaction);
            Transactions[index].Transaction = existingTransaction;
            BankAccountName = existingTransaction.BankAccount.BankName;
            BankAccountBalance = existingTransaction.BankAccount.Balance.ToString("C2");
        }
    }

    private async void DeleteTransactionAsync(object? sender, DialogServiceEventArgs e)
    {
        Transaction selectedTransaction = _selectedTransaction!.Transaction;
        await _dataStore.Transaction.DeleteAsync(selectedTransaction);

        Transactions.Remove(_selectedTransaction);
    }

    private static Transaction GetTransaction(DialogServiceEventArgs e)
    {
        TransactionForm form = (TransactionForm)e.Content;
        Transaction transaction = form.ViewModel.ObservableTransaction!.Transaction;
        return transaction;
    }
}
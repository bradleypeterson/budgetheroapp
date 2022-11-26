using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DesktopApplication.Contracts.Data;
using DesktopApplication.Contracts.Services;
using DesktopApplication.CustomEventArgs;
using DesktopApplication.Helpers;
using DesktopApplication.Models;
using DesktopApplication.ViewModels.Forms;
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
    public ObservableCollection<ObservableBankAccount> BankAccounts { get; set; } = new();

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
        if (BankAccounts.Any()) return;
        
        int userId = _sessionService.GetSessionUserId();

        IEnumerable<BankAccount?> bankAccounts = await _dataStore.BankAccount.ListAsync(a => a.UserId == _sessionService.GetSessionUserId());
        if (bankAccounts is not null)
        {
            foreach (var bankAccount in bankAccounts)
            {
                BankAccounts.Add(new ObservableBankAccount(bankAccount!));
            }
        }

        if (Transactions.Any()) return;
        IEnumerable<Transaction?> transactions = 
            await _dataStore.Transaction.ListAsync(t => t.BankAccount.UserId == userId, null!, "BankAccount,BudgetCategory");

        if (transactions is not null)
        {
            foreach (Transaction? transaction in transactions)
            {
                if (transaction is not null && transaction.BankAccount.UserId == userId) 
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
        await _dialogService.ShowDialogAsync<DeleteItemForm>(dialogTitle, _selectedTransaction);

        _dialogService.OnSaved -= DeleteTransactionAsync;
    }

    private async void AddTransactionAsync(object? sender, DialogServiceEventArgs e)
    {
        Transaction newTransaction = GetTransaction(e);
        BankAccount? bankAccount = _dataStore.BankAccount.GetById(newTransaction.BankAccountId);

        if (bankAccount is not null)
        {
            newTransaction.BankAccount = bankAccount;
            UpdateAccountBalance(newTransaction);
        }

        int result = await _dataStore.Transaction.AddAsync(newTransaction);

        if (result > 0)
            Transactions.Add(new ObservableTransaction(newTransaction));

    }

    private async void EditTransactionAsync(object? sender, DialogServiceEventArgs e)
    {
        Transaction existingTransaction = SelectedTransaction!.Transaction;
        existingTransaction = EntityUtilities.Update(existingTransaction, GetTransaction(e));
        UpdateAccountBalance(existingTransaction);

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
        UpdateAccountBalance(selectedTransaction, true);
        await _dataStore.Transaction.DeleteAsync(selectedTransaction);

        Transactions.Remove(_selectedTransaction);
    }

    private static Transaction GetTransaction(DialogServiceEventArgs e)
    {
        TransactionForm form = (TransactionForm)e.Content;
        Transaction transaction = form.ViewModel.ObservableTransaction!.Transaction;
        return transaction;
    }

    //Used to filter the transaction list
    public void FilterList(string filter, string category)
    {
        int userId = _sessionService.GetSessionUserId();

        ObservableCollection<ObservableTransaction> filteredList = new ObservableCollection<ObservableTransaction>();

        IEnumerable<Transaction?> transactions =
            _dataStore.Transaction.List(t => t.BankAccount.UserId == userId, null!, "BankAccount,BudgetCategory");

        if (transactions is not null)
        {
            foreach (var transaction in transactions)
            {
                if (transaction is not null && transaction.BankAccount.UserId == userId)
                    if (!filter.Equals(""))
                    {
                        switch (category)
                        {
                            case "Payee":
                                if (transaction.TransactionPayee.Contains(filter)) filteredList.Add(new ObservableTransaction(transaction));
                                break;
                            case "Category":
                                if (transaction.BudgetCategory.CategoryName.Contains(filter)) filteredList.Add(new ObservableTransaction(transaction));
                                break;
                            default:
                                Console.Error.WriteLine("You did done f'd up");
                                break;
                        }
                    }
                    else
                    {
                        filteredList.Add(new ObservableTransaction(transaction));
                    }
            }
        }

        Transactions= filteredList;
    }

    private void UpdateAccountBalance(Transaction transaction, bool isDeleting = false)
    {
        if (transaction.ExpenseAmount is 0 && transaction.DepositAmount is not 0)
        {
            if (isDeleting)
                transaction.BankAccount.Balance -= transaction.DepositAmount;
            else
                transaction.BankAccount.Balance += transaction.DepositAmount;
        }
        else if (transaction.ExpenseAmount is not 0 && transaction.DepositAmount is 0)
        {
            if (isDeleting)
                transaction.BankAccount.Balance += transaction.ExpenseAmount;
            else
                transaction.BankAccount.Balance -= transaction.ExpenseAmount;
        }
        else
        {
            Debug.WriteLine("The bank account balance could not be updated.");
        }
    }
}
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DesktopApplication.Contracts.Data;
using DesktopApplication.Contracts.Services;
using DesktopApplication.CustomEventArgs;
using DesktopApplication.Models;
using DesktopApplication.Services;
using DesktopApplication.Views.Forms;
using ModelsLibrary;
using Windows.System;
using Windows.UI.Popups;
using WinUIEx.Messaging;

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
            HasItemSelected = value is not null;
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
        Transaction _selectedTransaction = SelectedTransaction!.Transaction;
        await _dialogService.ShowDialogAsync<TransactionForm>(dialogTitle, _selectedTransaction);

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
        
        if (validateDate(newTransaction))//Remove this validation once buttons can be enabled and disabled
        {
            int result = await _dataStore.Transaction.AddAsync(newTransaction);//keep this

            Transactions.Add(new ObservableTransaction(newTransaction));//keep this
        }

    }

    private async void EditTransactionAsync(object? sender, DialogServiceEventArgs e)
    {
        Transaction editedTransaction = GetTransaction(e);
        ObservableTransaction? listedTransaction = Transactions.FirstOrDefault(
            a => a.Transaction.TransactionId == editedTransaction.TransactionId);
        int index;

        if (listedTransaction is not null)//Remove this validation  once buttons can be enabled and disabled
        {

            if (validateDate(editedTransaction))//keep this
            {
                await _dataStore.Transaction.Update(editedTransaction);//keep this

                index = Transactions.IndexOf(listedTransaction);//keep this 
                Transactions[index].Transaction = editedTransaction;//keep this
            }
            
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
        Transaction transaction = form.ViewModel.ObservableTransaction.Transaction;
        //transaction.BankAccountId = form.ViewModel.SelectedBankAccount!.BankAccountId;
        return transaction;
    }

    //Used to filter the tranaction list
    public async void filterList(string filter, string category)
    {
        int userId = _sessionService.GetSessionUserId();

        ObservableCollection<ObservableTransaction> filteredList = new ObservableCollection<ObservableTransaction>();

        IEnumerable<Transaction?> transactions =
            _dataStore.Transaction.List(t => t.BankAccount.UserId == userId, null!, "BankAccount,BudgetCategory");

        if (transactions is not null)
        {
            foreach (var transaction in transactions)
            {
                if (transaction is not null)
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

    //Once buttons can be disbled and enabled this can be deleted.
    public bool validateDate(Transaction transaction)
    {
        try
        {
            if (transaction.TransactionPayee == null) { return false; }
            if (transaction.BankAccountId == 0) { return false; }
            if (transaction== null) { return false; }
            if (transaction.BudgetCategoryId == 0) { return false; }
            if (transaction.DepositAmount== 0 && transaction.ExpenseAmount == 0) { return false; }
            return true;
        }
        catch
        {
            return false;
        }
    }

}
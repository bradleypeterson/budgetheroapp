using CommunityToolkit.Mvvm.ComponentModel;
using DesktopApplication.Contracts.Data;
using DesktopApplication.Contracts.Services;
using DesktopApplication.Models;
using System.Collections.ObjectModel;
using ModelsLibrary;

namespace DesktopApplication.ViewModels;

public class ReportsViewModel : ObservableRecipient
{

    private readonly ISessionService _sessionService; //session service to find the current user
    private readonly IDataStore _dataStore; //used to get and save data

    //Observable List of Transactions from the User
    public ObservableCollection<ObservableTransaction> Transactions { get; set; } = new();

    //Constructor
    public ReportsViewModel()
    {
        _sessionService = App.GetService<ISessionService>();
        _dataStore = App.GetService<IDataStore>();
    }


    //Used to get the user and the user's transactions
    public async Task LoadAsync()
    {
        int userId = _sessionService.GetSessionUserId();

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
}

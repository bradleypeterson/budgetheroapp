using CommunityToolkit.Mvvm.ComponentModel;
using DesktopApplication.Contracts.Data;
using DesktopApplication.Contracts.Services;
using DesktopApplication.Models;
using System.Collections.ObjectModel;
using ModelsLibrary;
using Windows.System;
using System.Collections.Generic;
using System.Linq;
using static DesktopApplication.ViewModels.ReportsViewModel;

namespace DesktopApplication.ViewModels;

public class ReportsViewModel : ObservableRecipient
{

    private readonly ISessionService _sessionService; //session service to find the current user
    private readonly IDataStore _dataStore; //used to get and save data

    //Observable List of Transactions from the User
    public ObservableCollection<ObservableTransaction> Transactions { get; set; } = new();
    public List<lineGraphData> CategoryLineData { get; set; } = new();

    //Constructor
    public ReportsViewModel()
    {
        _sessionService = App.GetService<ISessionService>();
        _dataStore = App.GetService<IDataStore>();
    }


    //Used to get the user and the user's transactions
    public async Task LoadAsync()
    {
        Guid userId = _sessionService.GetSessionUserId();

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

    public List<lineGraphData> gatherData(string category)
    {
        var transactionList = Transactions.Where(x => x.CategoryName.Equals(category)).ToList();

        List<lineGraphData> list = new List<lineGraphData>();

        //Add all the months to the list with 0 total cost.
        for (int i = 1; i < 13; i++)
        {
            lineGraphData temp = new lineGraphData();
            temp.TotalCost = 0.0;
            temp.Category = category;
            temp.Month = monthCase(i);

            list.Add(temp);
        }

        //add the transactions to the total cost per month
        foreach (var transaction in transactionList)
        {
            if (transaction.ExpenseAmount.Equals(""))
            {
                list[transaction.TransactionDate.Month - 1].TotalCost = list[transaction.TransactionDate.Month - 1].TotalCost + Double.Parse(transaction.DepositAmount);
            }
            else
            {
                list[transaction.TransactionDate.Month-1].TotalCost = list[transaction.TransactionDate.Month-1].TotalCost + Double.Parse(transaction.ExpenseAmount);
            }
            
        }

        return list;
    }

    public List<lineGraphData> gatherData()
    {
        var transactionList = Transactions.Where(x => (x.DepositAmount.Equals(""))).ToList();

        List<lineGraphData> list = new List<lineGraphData>();

        for (int i =0 ; i < 12; i++)
        {
            lineGraphData data = new lineGraphData();
            data.TotalCost = 0.0;
            data.Month = monthCase(i + 1);
            list.Add(data);
        }

        foreach (var transaction in transactionList)
        {
            list[transaction.TransactionDate.Month-1].TotalCost += Double.Parse(transaction.ExpenseAmount);
        }
        return list;
    }

    public class lineGraphData
    {
        public string? Month { get; set;}
        public string? Category { get; set;}
        public double TotalCost { get; set;}
    }

    private string monthCase(int i)
    {
        switch (i)
        {
            case 1:
                return "Jan";
            case 2:
                return "Feb";
            case 3:
                return "Mar";
            case 4:
                return "Apr";
            case 5:
                return "May";
            case 6:
                return "Jun";
            case 7:
                return "Jul";
            case 8:
                return "Aug";
            case 9:
                return "Sep";
            case 10:
                return "Oct";
            case 11:
                return "Nov";
            case 12:
                return "Dec";
            default:
                return "Not Found";

        }
    }

    public class pieChartData
    {
        public string? Category { get; set; }
        public double TotalCost { get; set; }
    }


    public List<pieChartData> gatherPieChartData(int month)
    {
        var transactionList = Transactions.Where(x => (x.TransactionDate.Month == month && x.DepositAmount.Equals(""))).ToList();

        List<pieChartData> list = new List<pieChartData>();
        List<string> categories = new List<string>();

        foreach (var transaction in transactionList)
        {
            if (!categories.Contains(transaction.CategoryName))
            {
                pieChartData temp = new pieChartData();
                temp.TotalCost = Double.Parse(transaction.ExpenseAmount);
                temp.Category = transaction.CategoryName;
                categories.Add(transaction.CategoryName);

                list.Add(temp);
            }
            else
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].Category.Equals(transaction.CategoryName))
                    {
                        list[i].TotalCost += Double.Parse(transaction.ExpenseAmount);
                    }
                }
            }
        }
        return list;
    }

    public List<lineGraphData> gatherDepositData()
    {
        var transactionList = Transactions.Where(x => (x.ExpenseAmount.Equals(""))).ToList();

        List<lineGraphData> list = new List<lineGraphData>();

        for (int i = 0; i < 12; i++)
        {
            lineGraphData data = new lineGraphData();
            data.TotalCost = 0.0;
            data.Month = monthCase(i + 1);
            list.Add(data);
        }

        foreach (var transaction in transactionList)
        {
            list[transaction.TransactionDate.Month - 1].TotalCost += Double.Parse(transaction.DepositAmount);
        }
        return list;
    }
}

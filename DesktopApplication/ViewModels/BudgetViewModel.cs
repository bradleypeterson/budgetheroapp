using CommunityToolkit.Mvvm.ComponentModel;
using DesktopApplication.Contracts.Data;
using DesktopApplication.Contracts.Services;
using DesktopApplication.Models;
using DesktopApplication.ViewModels.Models;
using ModelsLibrary;
using System.Collections.ObjectModel;

namespace DesktopApplication.ViewModels;

public class BudgetViewModel : ObservableRecipient
{
    private readonly IDataStore _dataStore;
    private readonly ISessionService _sessionService;

    public List<BudgetCategoryGroupViewModel> BudgetCategoryGroups { get; }

    public BudgetViewModel()
    {
        _dataStore = App.GetService<IDataStore>();
        _sessionService = App.GetService<ISessionService>();
        BudgetCategoryGroups = GenerateSampleBudgetCategoryGroups();
    }

    public ObservableCollection<ObservableBankAccount> BankAccounts { get; set; } = new();

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
    }

        private static List<BudgetCategoryGroupViewModel> GenerateSampleBudgetCategoryGroups()
    {
        List<BudgetCategoryGroupViewModel> list = new()
        {
            new BudgetCategoryGroupViewModel(new BudgetCategoryGroup(){ CategoryGroupDesc = "Housing" }),
            new BudgetCategoryGroupViewModel(new BudgetCategoryGroup(){ CategoryGroupDesc = "Utilities" }),
            new BudgetCategoryGroupViewModel(new BudgetCategoryGroup(){ CategoryGroupDesc = "Food" }),
            new BudgetCategoryGroupViewModel(new BudgetCategoryGroup(){ CategoryGroupDesc = "Transportation" }),
        };

        return list;
    }
}

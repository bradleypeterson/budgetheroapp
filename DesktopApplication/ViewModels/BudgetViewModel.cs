using CommunityToolkit.Mvvm.ComponentModel;
using DesktopApplication.Contracts.Data;
using DesktopApplication.Contracts.Services;
using DesktopApplication.ViewModels.Models;
using ModelsLibrary;

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

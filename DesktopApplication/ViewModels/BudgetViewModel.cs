using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using DesktopApplication.ViewModels.Models;
using ModelsLibrary;

namespace DesktopApplication.ViewModels;

public class BudgetViewModel : ObservableRecipient
{
    public List<BudgetCategoryGroupViewModel> BudgetCategoryGroups { get; }

    public BudgetViewModel()
    {
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

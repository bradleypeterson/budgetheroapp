using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesktopApplication.ViewModels.Models;
using ModelsLibrary;

namespace DesktopApplication.ViewModels.Models;
public class BudgetCategoryGroupViewModel
{
    private readonly BudgetCategoryGroup _budgetCategoryGroup;
    public string CategoryGroupDesc { get; }
    public List<BudgetCategoryViewModel> BudgetCategories { get; }
    

    public BudgetCategoryGroupViewModel(BudgetCategoryGroup budgetCategoryGroup)
    {
        _budgetCategoryGroup = budgetCategoryGroup;
        CategoryGroupDesc = _budgetCategoryGroup.CategoryGroupDesc;
        //BudgetCategories = //TODO: pull all budget items associated with category
    }

    //TEST: Delete when not needed
    private List<BudgetCategoryViewModel> GenerateSampleBudgetCategories()
    {
        List<BudgetCategoryViewModel> budgetCategories = new()
        {
            new BudgetCategoryViewModel(
                new BudgetCategory()
                {
                    CategoryName = "Item 1",
                    CategoryAmount = 100.00M,
                    BudgetCategoryGroupID = 1,
                    BudgetCategoryGroup = _budgetCategoryGroup
                }
            ),
            new BudgetCategoryViewModel(
                new BudgetCategory()
                {
                    CategoryName = "Item 2",
                    CategoryAmount = 200.00M,
                    BudgetCategoryGroupID = 2,
                    BudgetCategoryGroup = _budgetCategoryGroup
                }
            ),
            new BudgetCategoryViewModel(
                new BudgetCategory()
                {
                    CategoryName = "Item 3",
                    CategoryAmount = 300.00M,
                    BudgetCategoryGroupID = 3,
                    BudgetCategoryGroup = _budgetCategoryGroup
                }
            ),
        };

        return budgetCategories;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelsLibrary;

namespace DesktopApplication.ViewModels.Models;
public class BudgetCategoryViewModel
{
    private readonly BudgetCategory _budgetCategory;
    public string? CategoryName => _budgetCategory.CategoryName;
    public decimal CategoryAmount => _budgetCategory.CategoryAmount;

    public BudgetCategoryViewModel(BudgetCategory budgetCategory)
    {
        _budgetCategory = budgetCategory;
    }

}

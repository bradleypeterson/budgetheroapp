using CommunityToolkit.Mvvm.ComponentModel;
using ModelsLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopApplication.Models
{
    public class ObservableCategoryGroup : ObservableObject
    {
        public ObservableCategoryGroup(BudgetCategoryGroup budgetCategoryGroup)
        {
            BudgetCategoryGroup = budgetCategoryGroup;
        }

        private BudgetCategoryGroup _budgetCategoryGroup = new();
        public BudgetCategoryGroup BudgetCategoryGroup
        {
            get => _budgetCategoryGroup!;
            set
            {
                _budgetCategoryGroup = value;
                CategoryGroupDesc = _budgetCategoryGroup.CategoryGroupDesc;
            }
        }

        private string? _categoryGroupDesc;
        public string? CategoryGroupDesc
        {
            get=> _categoryGroupDesc;
            set
            {
                SetProperty(ref _categoryGroupDesc, value);
                if (value is not null)
                    _budgetCategoryGroup.CategoryGroupDesc = value;
            }
        }

    }
}

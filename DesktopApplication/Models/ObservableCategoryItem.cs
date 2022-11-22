using CommunityToolkit.Mvvm.ComponentModel;
using ModelsLibrary;
using System.Diagnostics;

namespace DesktopApplication.Models
{
    public class ObservableCategoryItem : ObservableObject
    {
        public ObservableCategoryItem(BudgetCategory budgetCategory) 
        {
            BudgetCategory = budgetCategory;
        }

        private BudgetCategory _budgetCategory = new();
        public BudgetCategory BudgetCategory
        {
            get => _budgetCategory!;
            set
            {
                _budgetCategory = value;
                CategoryName = _budgetCategory.CategoryName;
                CategoryAmount= _budgetCategory.CategoryAmount;
            }
        
        
        }

        private string _categoryName;
        public string CategoryName
        {
            get => _categoryName;
            set
            {
                SetProperty(ref _categoryName, value);
                _budgetCategory.CategoryName = _categoryName!;
            }
        }

        private decimal _categoryAmount;
        public decimal CategoryAmount
        {
            get => _categoryAmount;
            set
            {
                SetProperty(ref _categoryAmount, value);
                _budgetCategory.CategoryAmount = _categoryAmount!;
            }
        }

        private void ConvertToDecimal(string value)
        {
            decimal tempBalance;
            if (decimal.TryParse(value, out tempBalance))
            {
                CategoryAmount = tempBalance;
            }
            else
            {
                Debug.WriteLine("Error: You must provide a decimal value.");
            }
        }
    }
}

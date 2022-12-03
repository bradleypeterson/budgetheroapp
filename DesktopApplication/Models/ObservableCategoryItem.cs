using CommunityToolkit.Mvvm.ComponentModel;
using DesktopApplication.Contracts.Data;
using DesktopApplication.Contracts.Services;
using ModelsLibrary;
using System.Diagnostics;

namespace DesktopApplication.Models
{
    public class ObservableCategoryItem : ObservableObject
    {
        private readonly IDataStore _dataStore;
        IEnumerable<Transaction?> transactions;

        public ObservableCategoryItem() { }

        public ObservableCategoryItem(BudgetCategory budgetCategory) 
        {
            BudgetCategory = budgetCategory;
            _dataStore = App.GetService<IDataStore>();
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

        private decimal _allocated;
        public decimal Allocated
        {
            get => _allocated;
            set
            {
                SetProperty(ref _allocated, value);
            }
        }

        private decimal _remaining;
        public decimal Remaining
        {
            get => _remaining;
            set
            {
                SetProperty(ref _remaining, value);
            }
        }


        public void SetAllocatedAndRemaining()
        {
            /* First set Allocated */
            decimal totalAllocated = 0;
            transactions = _dataStore.Transaction!.GetAll(t => t.BudgetCategoryId == BudgetCategory.BudgetCategoryID);
            foreach (Transaction? transaction in transactions)
            {
                totalAllocated += transaction.ExpenseAmount;
            }
            Allocated = totalAllocated;

            /* Next set Remaining */
            decimal totalRemaining = _categoryAmount - _allocated;
            if (totalRemaining <= 0)
            {
                totalRemaining = 0.00m;
            }
            Remaining = totalRemaining;
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

using CommunityToolkit.Mvvm.ComponentModel;
using DesktopApplication.Contracts.Data;
using DesktopApplication.Contracts.Services;
using DesktopApplication.Data;
using ModelsLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopApplication.Models
{
    public class ObservableExpander : ObservableObject
    {
        private readonly BudgetCategoryGroup _budgetCategoryGroup;
        
        private readonly IDataStore _dataStore;
        
        public ObservableCollection<ObservableCategoryItem>? CategoryItems { get; set; } = new();

        public ObservableExpander(BudgetCategoryGroup budgetCategoryGroup)
        {
            _budgetCategoryGroup = budgetCategoryGroup;
            _dataStore = App.GetService<IDataStore>();
            LoadAsync();
        }

        public string? CategoryGroupDesc => _budgetCategoryGroup.CategoryGroupDesc;
        public int? CategoryGroupID => _budgetCategoryGroup.BudgetCategoryGroupID;

        public void AddItemToExpanderList(BudgetCategory catItem)
        {
            CategoryItems.Add(new ObservableCategoryItem(catItem));
        }

        public void DeleteItemFromExpanderList(BudgetCategory catItem)
        {
            ObservableCategoryItem item = CategoryItems.FirstOrDefault(x => x.BudgetCategory.BudgetCategoryID == catItem.BudgetCategoryID);

            if (item != null)
            {
                CategoryItems.Remove(item);
            }
        }

        public void EditItemInExpanderList(BudgetCategory catItem)
        {
            var itemToEdit = CategoryItems.FirstOrDefault(i => i.BudgetCategory.BudgetCategoryID == catItem.BudgetCategoryID);
            itemToEdit.CategoryName = catItem.CategoryName;
            itemToEdit.CategoryAmount = catItem.CategoryAmount;
        }

        private void LoadAsync()
        {
            if (CategoryItems.Any())
            {
                return;
            }
            
            var groupID = _budgetCategoryGroup.BudgetCategoryGroupID;
            var BudgetItems = _dataStore.BudgetCategory.GetAll(c => c.BudgetCategoryGroupID == groupID);

            foreach (var item in BudgetItems)
            {
                CategoryItems.Add(new ObservableCategoryItem(item));
            }

            foreach (ObservableCategoryItem item in CategoryItems)
            {
                item.SetAllocatedAndRemaining();
            }
        }
    }
}

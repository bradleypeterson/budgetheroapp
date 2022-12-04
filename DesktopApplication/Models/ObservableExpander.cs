using CommunityToolkit.Mvvm.ComponentModel;
using DesktopApplication.Contracts.Data;
using ModelsLibrary;
using System.Collections.ObjectModel;

namespace DesktopApplication.Models
{
    public class ObservableExpander : ObservableObject
    {
        private BudgetCategoryGroup _budgetCategoryGroup;
        
        private readonly IDataStore _dataStore;
        
        public ObservableCollection<ObservableCategoryItem>? CategoryItems { get; set; } = new();

        public ObservableExpander(BudgetCategoryGroup budgetCategoryGroup)
        {
            _budgetCategoryGroup = budgetCategoryGroup;
            _dataStore = App.GetService<IDataStore>();
            LoadAsync();
        }

        private string? _categoryGroupDesc;
        public string? CategoryGroupDesc { 
            get => _budgetCategoryGroup.CategoryGroupDesc;
            set
            {
                SetProperty(ref _categoryGroupDesc, value);
            }
        }
        public Guid CategoryGroupID => _budgetCategoryGroup.BudgetCategoryGroupID;

        public void AddItemToExpanderList(BudgetCategory catItem)
        {
            CategoryItems.Add(new ObservableCategoryItem(catItem));
            
            var newItem = CategoryItems.FirstOrDefault(i => i.BudgetCategory.BudgetCategoryID == catItem.BudgetCategoryID);
            newItem.SetAllocatedAndRemaining();
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

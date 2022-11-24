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
        public ObservableCollection<BudgetCategory>? CategoryItems { get; set; } = new();

        public ObservableExpander(BudgetCategoryGroup budgetCategoryGroup)
        {
            _budgetCategoryGroup = budgetCategoryGroup;
            _dataStore = App.GetService<IDataStore>();
            LoadAsync();
        }

        public string? CategoryGroupDesc => _budgetCategoryGroup.CategoryGroupDesc;
        public int? CategoryGroupID => _budgetCategoryGroup.BudgetCategoryGroupID;

        public void AddToExpanderList(BudgetCategory catItem)
        {
            CategoryItems.Add(catItem);
        }

        public void DeleteFromExpanderList(BudgetCategory catItem)
        {
            CategoryItems.Remove(catItem);
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
                CategoryItems.Add(item);
            }
        }

    }
}

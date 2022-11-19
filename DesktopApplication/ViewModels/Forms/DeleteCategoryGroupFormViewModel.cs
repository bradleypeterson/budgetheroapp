using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using DesktopApplication.Contracts.Data;
using DesktopApplication.Contracts.Services;
using DesktopApplication.Models;
using ModelsLibrary;

namespace DesktopApplication.ViewModels.Forms
{
    public class DeleteCategoryGroupFormViewModel : ObservableRecipient
    {
        private readonly IDataStore _dataStore;
        private readonly ISessionService _sessionService;

        public ObservableCollection<BudgetCategoryGroup> BudgetCategoryGroups { get; } = new();
        public ObservableCategoryGroup ObservableCategoryGroup { get; set; }

        public DeleteCategoryGroupFormViewModel()
        {
            _dataStore = App.GetService<IDataStore>();
            _sessionService = App.GetService<ISessionService>();
        }

        private BudgetCategoryGroup? _selectedCategoryGroup;
        public BudgetCategoryGroup? SelectedCategoryGroup
        {
            get => _selectedCategoryGroup;
            set
            {
                SetProperty(ref _selectedCategoryGroup, value);
                ObservableCategoryGroup.BudgetCategoryGroup.BudgetCategoryGroupID = value!.BudgetCategoryGroupID;
            }
        }


        public async Task LoadAsync()
        {
            if (BudgetCategoryGroups.Any()) return;

            int? userId = _sessionService.GetSessionUserId();
            User? user = _dataStore.User!.Get(u => u.UserId == userId, false, "Budgets");
            var userBudgets = user?.Budgets;
            Budget? budget = userBudgets?.ToList()[0];
            int? budgetId = budget!.BudgetId;
            Budget? personalBudget = _dataStore.Budget!.Get(b => b.BudgetId == budgetId, false, "BudgetCategoryGroups");

            var _usersCategoryGroups = personalBudget.BudgetCategoryGroups;

            if (_usersCategoryGroups is not null)
            {
                foreach (BudgetCategoryGroup? categoryGroup in _usersCategoryGroups
)
                {
                    BudgetCategoryGroups.Add(categoryGroup!);
                }
            }

        }
    }
}

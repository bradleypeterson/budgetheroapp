using CommunityToolkit.Mvvm.ComponentModel;
using DesktopApplication.Contracts.Data;
using DesktopApplication.Contracts.Services;
using ModelsLibrary;
using System.Collections.ObjectModel;

namespace DesktopApplication.ViewModels.Forms
{
    public class DeleteCategoryGroupFormViewModel : ObservableRecipient
    {
        private readonly IDataStore _dataStore;
        private readonly ISessionService _sessionService;

        public ObservableCollection<BudgetCategoryGroup> BudgetCategoryGroups { get; } = new();

        public DeleteCategoryGroupFormViewModel()
        {
            _dataStore = App.GetService<IDataStore>();
            _sessionService = App.GetService<ISessionService>();
        }

        private BudgetCategoryGroup? _selectedCategoryGroup = new();
        public BudgetCategoryGroup? SelectedCategoryGroup
        {
            get => _selectedCategoryGroup;
            set
            {
                SetProperty(ref _selectedCategoryGroup, value);
            }
        }

        private void SetSelectedGroup()
        {
            int categoryGroupId = 0;

            if (SelectedCategoryGroup is not null)
            {
                categoryGroupId = SelectedCategoryGroup.BudgetCategoryGroupID;
            }
            
            SelectedCategoryGroup = BudgetCategoryGroups.FirstOrDefault(g => g.BudgetCategoryGroupID == categoryGroupId);
        }

        public void LoadAsync()
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
                foreach (BudgetCategoryGroup? categoryGroup in _usersCategoryGroups)
                {
                    if (categoryGroup.CategoryGroupDesc != "Household")
                    { 
                        BudgetCategoryGroups.Add(categoryGroup!);
                    } 
                }
            }
            SetSelectedGroup();
        }
    }
}

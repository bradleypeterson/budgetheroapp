using CommunityToolkit.Mvvm.ComponentModel;
using DesktopApplication.Contracts.Data;
using DesktopApplication.Contracts.Services;
using ModelsLibrary;
using System.Collections.ObjectModel;


namespace DesktopApplication.ViewModels.Forms
{
    public class EditCategoryGroupFormViewModel : ObservableRecipient
    {
        private readonly IDataStore _dataStore;
        private readonly ISessionService _sessionService;

        public ObservableCollection<BudgetCategoryGroup> BudgetCategoryGroups { get; } = new();
        public ObservableCollection<BudgetCategory> BudgetCategories { get; } = new();
        public ObservableCollection<BudgetCategory> CategoriesToShow { get; } = new();

        public EditCategoryGroupFormViewModel()
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
        
        private BudgetCategory? _selectedCategoryItem = new();
        public BudgetCategory? SelectedCategoryItem
        {
            get => _selectedCategoryItem;
            set
            {
                SetProperty(ref _selectedCategoryItem, value);
            }
        }

        private string? _categoryGroupDescText;
        public string? CategoryGroupDescText
        {
            get => _categoryGroupDescText;
            set
            {
               SetProperty(ref _categoryGroupDescText, value);
            }
        }

        private string? _categoryGroupRadStatus;
        public string? CategoryGroupRadStatus
        {
            get => _categoryGroupRadStatus;
            set
            {
                SetProperty(ref _categoryGroupRadStatus, value);
            }
        }
        
        private string? _categoryItemName;
        public string? CategoryItemName
        {
            get => _categoryItemName;
            set
            {
                SetProperty(ref _categoryItemName, value);
            }
        }

        private decimal? _categoryItemBudgetAmt;
        public decimal? CategoryItemBudgetAmt
        {
            get => _categoryItemBudgetAmt;
            set
            {
                SetProperty(ref _categoryItemBudgetAmt, value);
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

        private void SetSelectedItem() 
        {
            int categoryItemId = 0;
            
            if (SelectedCategoryItem is not null)
            {
                categoryItemId = SelectedCategoryItem.BudgetCategoryID;
            }
            
            SelectedCategoryItem = BudgetCategories.FirstOrDefault(i => i.BudgetCategoryID == categoryItemId);
        }

        public void SetCategoriesToShow()
        {
            if (CategoriesToShow.Any())
            {
                CategoriesToShow.Clear();
            }
            //Grab ID of selected Group
            int catGroupId = SelectedCategoryGroup.BudgetCategoryGroupID;

            foreach (BudgetCategory? item in BudgetCategories)
            {
                if (item.BudgetCategoryGroupID == catGroupId)
                {
                    CategoriesToShow.Add(item);
                }
            }
        }

        public void LoadAsync()
        {
            if (BudgetCategoryGroups.Any() || BudgetCategories.Any()) { return; }

            int? userId = _sessionService.GetSessionUserId();
            User? user = _dataStore.User!.Get(u => u.UserId == userId, false, "Budgets");
            var userBudgets = user?.Budgets;
            Budget? budget = userBudgets?.FirstOrDefault(b => b.BudgetType == "personal");
            int? budgetId = budget!.BudgetId;
            Budget? personalBudget = _dataStore.Budget!.Get(b => b.BudgetId == budgetId, false, "BudgetCategoryGroups");
            var _usersCategoryGroups = personalBudget.BudgetCategoryGroups;

            if (_usersCategoryGroups is not null)
            {
                foreach (BudgetCategoryGroup? categoryGroup in _usersCategoryGroups)
                {
                    if(categoryGroup.CategoryGroupDesc != "Household")
                    {
                        BudgetCategoryGroups.Add(categoryGroup!);

                        var groupID = categoryGroup.BudgetCategoryGroupID;
                        var BudgetItems = _dataStore.BudgetCategory.GetAll(c => c.BudgetCategoryGroupID == groupID);

                        foreach (var item in BudgetItems)
                        {
                            BudgetCategories.Add(item);
                        }
                    }
                }
            }

            SetSelectedGroup();
            SetSelectedItem();
        }
    }
}

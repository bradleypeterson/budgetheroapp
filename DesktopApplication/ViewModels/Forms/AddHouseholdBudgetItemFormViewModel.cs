using CommunityToolkit.Mvvm.ComponentModel;
using DesktopApplication.Contracts.Data;
using DesktopApplication.Contracts.Services;
using DesktopApplication.Models;
using ModelsLibrary;
using System.Collections.ObjectModel;

namespace DesktopApplication.ViewModels.Forms
{
    public class AddHouseholdBudgetItemFormViewModel : ObservableRecipient
    {
        private readonly IDataStore _dataStore;
        private readonly ISessionService _sessionService;

        public ObservableCollection<User> UsersToShow { get; set; } = new();
        
        public AddHouseholdBudgetItemFormViewModel()
        {
            _dataStore = App.GetService<IDataStore>();
            _sessionService= App.GetService<ISessionService>();
        }

        public ObservableCollection<User> SelectedUsers { get; set; } = new();
        
        private string? _catItemName;
        public string? CatItemName
        {
            get => _catItemName;
            set
            {
                SetProperty(ref _catItemName, value);
            }
        }

        private decimal? _catItemAmount;
        public decimal? CatItemAmount
        {
            get => _catItemAmount;
            set
            {
                SetProperty(ref _catItemAmount, value);
            }
        }

        public void SetUsersToShow()
        {
            if (UsersToShow.Any()) { UsersToShow.Clear(); }

            Budget budget = _dataStore.Budget.Get(b => b.BudgetId == _dataStore.Budget.GetHouseholdBudget(_sessionService.GetSessionUserId()).BudgetId, false, "Users");

            List<User> users = budget.Users.ToList();

            foreach(var u in users)
            {
                UsersToShow.Add(u);
            }
        }

        public void LoadAsync()
        {
            SetUsersToShow();
        }
    }
}

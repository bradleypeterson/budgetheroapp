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

        public ObservableCollection<User> UsersToShow { get; } = new();
        
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

            int? userId = _sessionService.GetSessionUserId();
            User? user = _dataStore.User!.Get(u => u.UserId == userId, false, "Budgets");
            var userBudgets = user?.Budgets;
            Budget? budget = userBudgets?.FirstOrDefault(b => b.BudgetType == "household");
            ICollection<User> usersOfHousehold = budget.Users;

            foreach(var u in usersOfHousehold)
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

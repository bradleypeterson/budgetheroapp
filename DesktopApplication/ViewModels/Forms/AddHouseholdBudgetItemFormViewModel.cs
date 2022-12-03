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

        public void LoadAsync()
        {

        }
    }
}

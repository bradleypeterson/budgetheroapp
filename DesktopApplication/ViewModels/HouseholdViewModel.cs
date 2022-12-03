using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DesktopApplication.Contracts.Data;
using DesktopApplication.Contracts.Services;
using DesktopApplication.CustomEventArgs;
using DesktopApplication.Models;
using DesktopApplication.Views.Forms;
using ModelsLibrary;
using System.Collections.ObjectModel;

namespace DesktopApplication.ViewModels;

public class HouseholdViewModel : ObservableRecipient
{
    private readonly ISessionService _sessionService;
    private readonly IDialogService _dialogService;
    private readonly IDataStore _dataStore;

    public IAsyncRelayCommand CreateHouseholdCommand { get; }
    public IAsyncRelayCommand ShowJoinHouseholdCommand { get; }
    public IAsyncRelayCommand ShowAddBudgetItemCommand { get; }
    public IAsyncRelayCommand ShowEditBudgetItemCommand { get; }
    public IAsyncRelayCommand ShowInviteUserCommand { get; }

    public ObservableCollection<ObservableCategoryItem> CategoryItems { get; set; } = new();

    public HouseholdViewModel()
    {
        _sessionService = App.GetService<ISessionService>();
        _dialogService = App.GetService<IDialogService>();
        _dataStore = App.GetService<IDataStore>();

        CreateHouseholdCommand = new AsyncRelayCommand(CreateHouseholdBudget);
        ShowJoinHouseholdCommand = new AsyncRelayCommand(ShowJoinHousehold);
        ShowAddBudgetItemCommand = new AsyncRelayCommand(ShowAddBudgetItem);
        ShowEditBudgetItemCommand = new AsyncRelayCommand(ShowEditBudgetItem);
        ShowInviteUserCommand = new AsyncRelayCommand(ShowInviteUser);
    }

    private bool _hasHousehold;
    public bool HasHousehold
    {
        get => _hasHousehold;
        set => SetProperty(ref _hasHousehold, value);
    }

    private bool _hasNoHousehold;
    public bool HasNoHousehold
    {
        get => _hasNoHousehold;
        set => SetProperty(ref _hasNoHousehold, value);
    }

    private bool _creatingHH = false;
    public bool CreatingHH
    {
        get => _creatingHH;
        set => SetProperty(ref _creatingHH, value);
    }

    public async Task LoadAsync()
    {
        VerifyUserBudgetCount();

        if(CategoryItems.Any()) { return; }

        //TODO: load category items into collection
        int? userId = _sessionService.GetSessionUserId();
        User? user = _dataStore.User!.Get(u => u.UserId == userId, false, "Budgets");
        var userBudgets = user?.Budgets;
        Budget? budget = userBudgets?.FirstOrDefault(b => b.BudgetType == "household");
        int? budgetId = budget!.BudgetId;
        Budget? householdBudget = _dataStore.Budget!.Get(b => b.BudgetId == budgetId, false, "BudgetCategoryGroups");

        if (householdBudget is not null)
        {
            if (householdBudget.BudgetCategoryGroups is not null)
            {
                foreach (var categoryGroup in householdBudget.BudgetCategoryGroups)
                {
                    var groupID = categoryGroup.BudgetCategoryGroupID;
                    var BudgetItems = _dataStore.BudgetCategory.GetAll(c => c.BudgetCategoryGroupID == groupID);

                    foreach (var item in BudgetItems)
                    {
                        CategoryItems.Add(new ObservableCategoryItem(item));
                    }
                }

                foreach (var item in CategoryItems) { item.SetAllocatedAndRemaining(); }
            }
        }
    }

    private async Task CreateHouseholdBudget()
    {
        CreatingHH = true; 
        await Task.Delay(2000);

        int userId = _sessionService.GetSessionUserId();
        User? user = _dataStore.User.Get(u => u.UserId == userId, false, "Budgets");

        if (user is not null)
        {
            List<Budget> userBudgets;

            if (user.Budgets is not null)
            {
                userBudgets = user.Budgets.ToList();
            }
            else
            {
                userBudgets = new List<Budget>();
            }

            Budget budget = new()
            {
                BudgetName = "Household Budget",
                BudgetType = "household",
            };

            userBudgets.Add(budget);

            user.Budgets = userBudgets;

            _dataStore.User.Update(user);

            // create a Household category Group linked to the Household budget.
            BudgetCategoryGroup newCategoryGroup = new BudgetCategoryGroup { CategoryGroupDesc = "Household" };
            budget.BudgetCategoryGroups = new List<BudgetCategoryGroup>();
            budget.BudgetCategoryGroups!.Add(newCategoryGroup);

            await _dataStore.Budget.Update(budget);
        }
        CreatingHH = false;
    }
   
    private async Task ShowJoinHousehold()
    {
        _dialogService.OnSaved += JoinHouseholdAsync;

        string dialogTitle = "Join Household";
        await _dialogService.ShowDialogAsync<JoinHouseholdForm>(dialogTitle);

        _dialogService.OnSaved -= JoinHouseholdAsync;
    }

    private async Task ShowAddBudgetItem()
    {
        _dialogService.OnSaved += AddHouseholdBudgetItem;

        string dialogTitle = "Add Household Budget Item";
        await _dialogService.ShowDialogAsync<AddHouseholdBudgetItemForm>(dialogTitle);

        _dialogService.OnSaved -= AddHouseholdBudgetItem;
    }

    private async Task ShowEditBudgetItem()
    {
        _dialogService.OnSaved += EditHouseholdBudgetItem;

        string dialogTitle = "Edit Household Budget item";
        await _dialogService.ShowDialogAsync<EditHouseholdBudgetItemForm>(dialogTitle);

        _dialogService.OnSaved -= EditHouseholdBudgetItem;
    }
    
    private async Task ShowInviteUser()
    {
        _dialogService.OnSaved += InviteToHouseholdAsync;

        string dialogTitle = "Invite to Household";
        await _dialogService.ShowDialogAsync<InviteUserForm>(dialogTitle);

        _dialogService.OnSaved -= InviteToHouseholdAsync;
    }

    private async void InviteToHouseholdAsync(object? sender, DialogServiceEventArgs e)
    {
        //TODO: Implement logic for inviting another user to a household by creating an invite code and sending it via email.
        //      Invite code should be created by using the Household Budget ID and Encrypting/Hashing it.
    }

    private async void JoinHouseholdAsync(object? sender, DialogServiceEventArgs e)
    {
        //TODO: Implement logic for joining a household based on decrypting the invite code recieved from email.
        //      Decrypting the invite code should result in a BudgetID to add to the users Budgets
    }

    private async void AddHouseholdBudgetItem(object? sender, DialogServiceEventArgs e)
    {
        //TODO: Implement logic for adding a budget item to a household.
        //      Should be able to add name of the item and a total amount.
        //      User will be able to select any number of household members to split the items amount.
        //      Total amount will be split evenly among all selected members of the household.
        //      (Still need to figure out how we are going to handle the users paying towards it)
    }

    private async void EditHouseholdBudgetItem(object? sender, DialogServiceEventArgs e)
    {
        //TODO: Implement logic for editing a budget item
        //      User should be able to change the name, amount, and what users are splitting it.
        //      all necessary updates should be made in the appropriate places in the database.
    }

    private void VerifyUserBudgetCount()
    {
        int? userId = _sessionService.GetSessionUserId();
        User? user = _dataStore.User!.Get(u => u.UserId == userId, false, "Budgets");
        ICollection<Budget>? usersBudgets = user?.Budgets;

        if (usersBudgets.Count() > 1)
        {
            HasHousehold = true;
            HasNoHousehold = false;
        }
        else
        {
            HasNoHousehold = true;
            HasHousehold = false;
        }
    }
}

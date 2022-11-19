using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DesktopApplication.Contracts.Data;
using DesktopApplication.Contracts.Services;
using DesktopApplication.CustomEventArgs;
using DesktopApplication.Models;
using DesktopApplication.Services;
using DesktopApplication.ViewModels.Models;
using DesktopApplication.Views.Forms;
using ModelsLibrary;
using System.Collections.ObjectModel;


namespace DesktopApplication.ViewModels;

public class BudgetViewModel : ObservableRecipient
{
    private readonly ISessionService _sessionService;
    private readonly IDialogService _dialogService;
    private readonly IDataStore _dataStore;

    public ObservableCollection<ObservableCategoryGroup>? BudgetCategoryGroups { get; set; } = new();
    
    public IAsyncRelayCommand ShowAddDialogCommand { get; }
    public IAsyncRelayCommand ShowEditDialogCommand { get; }
    public IAsyncRelayCommand ShowDeleteDialogCommand { get; }

    public BudgetViewModel()
    {
        _sessionService = App.GetService<ISessionService>();
        _dialogService = App.GetService<IDialogService>();
        _dataStore = App.GetService<IDataStore>();
        
        ShowAddDialogCommand = new AsyncRelayCommand(ShowAddDialog);
        ShowEditDialogCommand = new AsyncRelayCommand(ShowEditDialog);
        ShowDeleteDialogCommand = new AsyncRelayCommand(ShowDeleteDialog);

    }

    //Load Category Groups into an observable collection
    public async Task LoadAsync()
    {
        if (BudgetCategoryGroups.Any())
        {
            return;
        }

        //var categoryGroups = await _dataStore.Budget.ListAsync(b => b. == _sessionService.GetSessionUserId());

        int? userId = _sessionService.GetSessionUserId();
        User? user = _dataStore.User!.Get(u => u.UserId == userId, false, "Budgets");
        var userBudgets = user?.Budgets;
        Budget? budget = userBudgets?.ToList()[0];
        int? budgetId = budget!.BudgetId;
        Budget? personalBudget = _dataStore.Budget!.Get(b => b.BudgetId == budgetId, false, "BudgetCategoryGroups");

        if (personalBudget.BudgetCategoryGroups is not null)
        {
            foreach (var categoryGroup in personalBudget.BudgetCategoryGroups)
            {
                BudgetCategoryGroups.Add(new ObservableCategoryGroup(categoryGroup!));
            }
        }
    }

    private static BudgetCategoryGroup GetCategoryGroup(DialogServiceEventArgs e, bool isDeleting = false)
    {
        if (isDeleting)
        {
            var categoryGroupForm = (DeleteCategoryGroupForm)e.Content;
            return categoryGroupForm.ViewModel.SelectedCategoryGroup;
        }
        else
        {
            var categoryGroupForm = (AddCategoryGroupForm)e.Content;
            return categoryGroupForm.ViewModel.BudgetCategoryGroup;
        }
    }

    private async Task ShowAddDialog()
    {
        _dialogService.OnSaved += AddCategoryGroupAsync;

        var dialogTitle = "Add Category Group";
        await _dialogService.ShowDialogAsync<AddCategoryGroupForm>(dialogTitle);

        _dialogService.OnSaved -= AddCategoryGroupAsync;
    }

    private async Task ShowEditDialog()
    {
        _dialogService.OnSaved += EditCategoryGroupAsync;

        var dialogTitle = "Edit Category Group";
        await _dialogService.ShowDialogAsync<EditCategoryGroupForm>(dialogTitle);

        _dialogService.OnSaved -= EditCategoryGroupAsync;
    }

    private async Task ShowDeleteDialog()
    {
        _dialogService.OnSaved += DeleteCategoryGroupAsync;

        var dialogTitle = "Delete Category Group";
        await _dialogService.ShowDialogAsync<DeleteCategoryGroupForm>(dialogTitle);

        _dialogService.OnSaved -= DeleteCategoryGroupAsync;
    }

    private async void AddCategoryGroupAsync(object? sender, DialogServiceEventArgs e)
    {
        BudgetCategoryGroup newCategoryGroup = GetCategoryGroup(e);

        int? userId = _sessionService.GetSessionUserId();
        User? user = _dataStore.User!.Get(u => u.UserId == userId, false, "Budgets");
        var userBudgets = user?.Budgets;
        Budget? budget = userBudgets?.ToList()[0];
        int? budgetId = budget!.BudgetId;
        Budget? personalBudget = _dataStore.Budget!.Get(b => b.BudgetId == budgetId, false, "BudgetCategoryGroups");

        personalBudget!.BudgetCategoryGroups!.Add(newCategoryGroup);

        var result = await _dataStore.BudgetCategoryGroup.AddAsync(newCategoryGroup);

        if (result == 2)
        {
            BudgetCategoryGroups?.Add(new ObservableCategoryGroup(newCategoryGroup));
        }
    }
    
    private async void DeleteCategoryGroupAsync(object? sender, DialogServiceEventArgs e)
    {
        BudgetCategoryGroup selectedCategoryGroup = GetCategoryGroup(e, true);

        ObservableCategoryGroup? listedCategoryGroup = BudgetCategoryGroups.FirstOrDefault(
            c => c.BudgetCategoryGroup.BudgetCategoryGroupID == selectedCategoryGroup.BudgetCategoryGroupID);
        int index;

        if(listedCategoryGroup != null)
        {
            await _dataStore.BudgetCategoryGroup.DeleteAsync(selectedCategoryGroup);

            index = BudgetCategoryGroups.IndexOf(listedCategoryGroup);
            BudgetCategoryGroups.RemoveAt(index);
        }
    }
    
    private async void EditCategoryGroupAsync(object? sender, DialogServiceEventArgs e)
    {
        //var editedBankAccount = GetBankAccount(e);
        //var listedBankAccount = BankAccounts.FirstOrDefault(a => a.BankAccount.BankAccountId == editedBankAccount.BankAccountId);
        //int index;

        //if (listedBankAccount is not null)
        //{
        //    await _dataStore.BankAccount.Update(editedBankAccount);

        //    index = BankAccounts.IndexOf(listedBankAccount);
        //    BankAccounts[index].BankAccount = editedBankAccount;
        //}
    }

   
}

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DesktopApplication.Contracts.Data;
using DesktopApplication.Contracts.Services;
using DesktopApplication.Models;
using DesktopApplication.CustomEventArgs;
using DesktopApplication.Views.Forms;
using ModelsLibrary;
using System.Collections.ObjectModel;

namespace DesktopApplication.ViewModels;

public class BudgetViewModel : ObservableRecipient
{
    private readonly ISessionService _sessionService;
    private readonly IDialogService _dialogService;
    private readonly IDataStore _dataStore;

    public IAsyncRelayCommand ShowAddDialogCommand { get; }
    public IAsyncRelayCommand ShowEditDialogCommand { get; }
    public IAsyncRelayCommand ShowDeleteDialogCommand { get; }

    public ObservableCollection<ObservableCategoryGroup>? BudgetCategoryGroups { get; set; } = new();
    public ObservableCollection<ObservableBankAccount> BankAccounts { get; set; } = new();
    public ObservableCollection<ObservableCategoryItem>? CategoryItems { get; set; } = new();
    public ObservableCollection<ObservableExpander>? Expanders { get; set; } = new();

    public BudgetViewModel()
    {
        _sessionService = App.GetService<ISessionService>();
        _dialogService = App.GetService<IDialogService>();
        _dataStore = App.GetService<IDataStore>();
        
        ShowAddDialogCommand = new AsyncRelayCommand(ShowAddDialog);
        ShowEditDialogCommand = new AsyncRelayCommand(ShowEditDialog);
        ShowDeleteDialogCommand = new AsyncRelayCommand(ShowDeleteDialog);
    }

    //Load Category Groups into observable collection
    public async Task LoadAsync()
    {
        if (BudgetCategoryGroups.Any() || CategoryItems.Any())
        {
            return;
        }

        int? userId = _sessionService.GetSessionUserId();
        User? user = _dataStore.User!.Get(u => u.UserId == userId, false, "Budgets");
        var userBudgets = user?.Budgets;
        Budget? budget = userBudgets?.FirstOrDefault(b => b.BudgetType == "personal");
        int? budgetId = budget!.BudgetId;
        Budget? personalBudget = _dataStore.Budget!.Get(b => b.BudgetId == budgetId, false, "BudgetCategoryGroups");

        if (personalBudget is not null)
        {
            if (personalBudget.BudgetCategoryGroups is not null)
            {
                foreach (var categoryGroup in personalBudget.BudgetCategoryGroups)
                {
                    BudgetCategoryGroups.Add(new ObservableCategoryGroup(categoryGroup!));
                    Expanders.Add(new ObservableExpander(categoryGroup!));

                    var groupID = categoryGroup.BudgetCategoryGroupID;
                    var BudgetItems = _dataStore.BudgetCategory.GetAll(c => c.BudgetCategoryGroupID == groupID);

                    foreach (var item in BudgetItems)
                    {
                        CategoryItems.Add(new ObservableCategoryItem(item));
                    }
                }
            }
        }

        if (BankAccounts.Any()) return;

        IEnumerable<BankAccount?> bankAccounts = await _dataStore.BankAccount.ListAsync(a => a.UserId == _sessionService.GetSessionUserId());
        if (bankAccounts is not null)
        {
            foreach (var bankAccount in bankAccounts)
            {
                BankAccounts.Add(new ObservableBankAccount(bankAccount!));
            }
        }
    }

    private static BudgetCategoryGroup GetCategoryGroup(DialogServiceEventArgs e, bool isDeleting = false, bool isEditing = false)
    {
        if (isDeleting)
        {
            var categoryGroupForm = (DeleteCategoryGroupForm)e.Content;
            return categoryGroupForm.ViewModel.SelectedCategoryGroup;
        }
        else if (isEditing)
        {
            var categoryGroupForm = (EditCategoryGroupForm)e.Content;
            return categoryGroupForm.ViewModel.SelectedCategoryGroup;
        }
        else
        {
            var categoryGroupForm = (AddCategoryGroupForm)e.Content;
            return categoryGroupForm.ViewModel.BudgetCategoryGroup;
        }
    }

    private static BudgetCategory GetCategoryItem(DialogServiceEventArgs e)
    {
        var categoryGroupForm = (EditCategoryGroupForm)e.Content;
        return categoryGroupForm.ViewModel.SelectedCategoryItem;
    }

    private static string GetGroupDescEditTxt(DialogServiceEventArgs e)
    {
        var categoryGroupForm = (EditCategoryGroupForm)e.Content;
        return categoryGroupForm.ViewModel.CategoryGroupDescText;
    }

    private static string GetRadioButtonStatus(DialogServiceEventArgs e)
    {
        var categoryGroupForm = (EditCategoryGroupForm)e.Content;
        return categoryGroupForm.ViewModel.CategoryGroupRadStatus;
    }

    private static string GetCategoryItemNameTxt(DialogServiceEventArgs e)
    {
        var categoryGroupForm = (EditCategoryGroupForm)e.Content;
        return categoryGroupForm.ViewModel.CategoryItemName;
    }

    private static decimal GetCategoryItemBudgetAmt(DialogServiceEventArgs e)
    {
        var categoryGroupForm = (EditCategoryGroupForm)e.Content;
        decimal convertedCategoryAmount = Convert.ToDecimal(categoryGroupForm.ViewModel.CategoryItemBudgetAmt);
        return convertedCategoryAmount;
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

        ICollection<Budget> userBudgets = user?.Budgets;
        Budget? budget = userBudgets?.FirstOrDefault(b => b.BudgetType == "personal");
        int? budgetId = budget!.BudgetId;
        Budget? personalBudget = _dataStore.Budget!.Get(b => b.BudgetId == budgetId, false, "BudgetCategoryGroups");

        personalBudget!.BudgetCategoryGroups!.Add(newCategoryGroup);

        var result = await _dataStore.BudgetCategoryGroup.AddAsync(newCategoryGroup);

        if (result == 2)
        {
            BudgetCategoryGroups?.Add(new ObservableCategoryGroup(newCategoryGroup));
            Expanders.Add(new ObservableExpander(newCategoryGroup!));
        }
    }
    
    private async void DeleteCategoryGroupAsync(object? sender, DialogServiceEventArgs e)
    {
        BudgetCategoryGroup selectedCategoryGroup = GetCategoryGroup(e, true, false);
        ObservableCategoryGroup? listedCategoryGroup = BudgetCategoryGroups.FirstOrDefault(c => c.BudgetCategoryGroup.BudgetCategoryGroupID == selectedCategoryGroup.BudgetCategoryGroupID);
        ObservableExpander? listedExpander = Expanders.FirstOrDefault(e => e.CategoryGroupID == selectedCategoryGroup.BudgetCategoryGroupID);
        int index;

        if(listedCategoryGroup != null)
        {
            await _dataStore.BudgetCategoryGroup.DeleteAsync(selectedCategoryGroup);

            index = BudgetCategoryGroups.IndexOf(listedCategoryGroup);
            BudgetCategoryGroups.RemoveAt(index);
            Expanders.Remove(listedExpander);
        }
    }

    private async void EditCategoryGroupAsync(object? sender, DialogServiceEventArgs e)
    {
        BudgetCategoryGroup selectedCategoryGroup = GetCategoryGroup(e, false, true);
        ObservableCategoryGroup? listedCategoryGroup = BudgetCategoryGroups.FirstOrDefault(c => c.BudgetCategoryGroup.BudgetCategoryGroupID == selectedCategoryGroup.BudgetCategoryGroupID);
        ObservableExpander? listedExpander = Expanders.FirstOrDefault(e => e.CategoryGroupID == selectedCategoryGroup.BudgetCategoryGroupID);

        int index;

        if (listedCategoryGroup != null)
        {
            index = BudgetCategoryGroups.IndexOf(listedCategoryGroup);

            BudgetCategoryGroups[index].CategoryGroupDesc = GetGroupDescEditTxt(e);
            selectedCategoryGroup.CategoryGroupDesc = GetGroupDescEditTxt(e);
            await _dataStore.BudgetCategoryGroup.Update(selectedCategoryGroup); /* updates the database value */

            /* Get status of radio button for Category Item Editing */
            var radStatus = GetRadioButtonStatus(e);

            if (radStatus != null)
            {
                if (radStatus == "Add Category Item")
                {
                    BudgetCategory newCategoryItem = new BudgetCategory();

                    //Set the data members and update database
                    newCategoryItem.CategoryName = GetCategoryItemNameTxt(e);
                    newCategoryItem.BudgetCategoryGroupID = selectedCategoryGroup.BudgetCategoryGroupID;
                    newCategoryItem.CategoryAmount = GetCategoryItemBudgetAmt(e);
                    var result = await _dataStore.BudgetCategory.AddAsync(newCategoryItem);
                    
                    //update list 
                    CategoryItems?.Add(new ObservableCategoryItem(newCategoryItem));
                    listedExpander.AddItemToExpanderList(newCategoryItem);
                }
                else if (radStatus == "Remove Category Item")
                {
                    //Get selected category Item 
                    BudgetCategory selectedCatItem = GetCategoryItem(e);
                    ObservableCategoryItem? listedCatItem = CategoryItems?.FirstOrDefault(i => i.BudgetCategory.BudgetCategoryID == selectedCatItem.BudgetCategoryID);

                    if (listedCatItem != null)
                    {
                        await _dataStore.BudgetCategory.DeleteAsync(selectedCatItem);
                        
                        index = CategoryItems.IndexOf(listedCatItem);
                        CategoryItems.RemoveAt(index);
                        listedExpander.DeleteItemFromExpanderList(selectedCatItem);
                    }
                }
                else if (radStatus == "Edit Category Item")
                {
                    //update database
                    BudgetCategory selectedCatItem = GetCategoryItem(e);
                    if (selectedCatItem != null)
                    {
                        selectedCatItem.CategoryName = GetCategoryItemNameTxt(e);
                        selectedCatItem.CategoryAmount = GetCategoryItemBudgetAmt(e);
                        await _dataStore.BudgetCategory.Update(selectedCatItem);
                    }

                    //update list
                    ObservableCategoryItem? listedCatItem = CategoryItems?.FirstOrDefault(i => i.BudgetCategory.BudgetCategoryID == selectedCatItem.BudgetCategoryID);
                    if (listedCatItem != null)
                    {
                        selectedCatItem.CategoryName = GetCategoryItemNameTxt(e);
                        selectedCatItem.CategoryAmount = GetCategoryItemBudgetAmt(e);
                        await _dataStore.BudgetCategory.Update(selectedCatItem);

                        index = CategoryItems.IndexOf(listedCatItem);
                        CategoryItems[index].CategoryName = GetCategoryItemNameTxt(e);
                        CategoryItems[index].CategoryAmount = GetCategoryItemBudgetAmt(e);

                        listedExpander.EditItemInExpanderList(selectedCatItem);
                    }
                }
            }
        }
    }
   
}

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
    private readonly IAPIService _apiService;
    private readonly Guid _userId;

    private Budget personalBudget;
    private IEnumerable<BudgetCategoryGroup> categoryGroups;
    private IEnumerable<BudgetCategory> categories;

    public IAsyncRelayCommand ShowAddDialogCommand { get; }
    public IAsyncRelayCommand ShowEditDialogCommand { get; }
    public IAsyncRelayCommand ShowDeleteDialogCommand { get; }

    public ObservableCollection<ObservableCategoryGroup> CategoryGroups { get; set; } = new();
    public ObservableCollection<ObservableBankAccount> BankAccounts { get; set; } = new();
    public ObservableCollection<ObservableCategoryItem> Categories { get; set; } = new();
    public ObservableCollection<ObservableExpander> Expanders { get; set; } = new();

    public BudgetViewModel()
    {
        _sessionService = App.GetService<ISessionService>();
        _dialogService = App.GetService<IDialogService>();
        _dataStore = App.GetService<IDataStore>();
        _apiService = App.GetService<IAPIService>();
        _userId = _sessionService.GetSessionUserId();
        
        ShowAddDialogCommand = new AsyncRelayCommand(ShowAddDialog);
        ShowEditDialogCommand = new AsyncRelayCommand(ShowEditDialog);
        ShowDeleteDialogCommand = new AsyncRelayCommand(ShowDeleteDialog);
    }

    //Load Category Groups into observable collection
    public async Task LoadAsync()
    {
        IEnumerable<BudgetCategoryGroup> _apiCategoryGroups;
        IEnumerable<BudgetCategory> _apiCategories;

        if (CategoryGroups.Any()) 
            return;

        // Data: Database
        personalBudget = _dataStore.Budget.GetPersonalBudget(_userId);
        categoryGroups = personalBudget.BudgetCategoryGroups ??= new List<BudgetCategoryGroup>();
        categories = await _dataStore.Budget.GetBudgetCategories(personalBudget);

        // Data: Api
        _apiCategoryGroups = await _apiService.GetCategoryGroups(personalBudget);
        _apiCategories = await _apiService.GetCategories(personalBudget);

        // UI: Account Details
        if (BankAccounts.Any())
        {
            IEnumerable<BankAccount?> bankAccounts = await _dataStore.BankAccount.ListAsync(a => a.UserId == _userId);

            if (bankAccounts is not null && bankAccounts.Any())
                foreach (BankAccount? account in bankAccounts)
                    BankAccounts.Add(new ObservableBankAccount(account!));
        }

        // UI: Load Category Groups
        if (categoryGroups.Any())
        {
            categoryGroups.ToList().ForEach(group => CategoryGroups.Add(new ObservableCategoryGroup(group)));
            categoryGroups.ToList().ForEach(group => Expanders.Add(new ObservableExpander(group)));

            await _apiService.UpdateCategoryGroups(_apiCategoryGroups, categoryGroups);
        }
        else if (_apiCategoryGroups.Any())
        {
            int result = await _dataStore.BudgetCategoryGroup.AddAsync(_apiCategoryGroups);
            categoryGroups = _apiCategoryGroups;

            if (result > 0)
            {
                categoryGroups.ToList().ForEach(group => CategoryGroups.Add(new ObservableCategoryGroup(group)));
                categoryGroups.ToList().ForEach(group => Expanders.Add(new ObservableExpander(group)));
            }   
        }
        else
            return;

        // UI: Load Categories
        if (categories.Any())
        {
            categories.ToList().ForEach(category => Categories.Add(new ObservableCategoryItem(category)));

            await _apiService.UpdateCategories(_apiCategories, categories);
        }
        else if (_apiCategories.Any())
        {
            int result = await _dataStore.BudgetCategory.AddAsync(_apiCategories);
            categories = _apiCategories;

            if (result > 0)
                categories.ToList().ForEach(category => Categories.Add(new ObservableCategoryItem(category)));
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

        personalBudget!.BudgetCategoryGroups!.Add(newCategoryGroup);

        var result = await _dataStore.BudgetCategoryGroup.AddAsync(newCategoryGroup);

        if (result == 2)
        {
            CategoryGroups?.Add(new ObservableCategoryGroup(newCategoryGroup));
            Expanders.Add(new ObservableExpander(newCategoryGroup!));
        }
    }
    
    private async void DeleteCategoryGroupAsync(object? sender, DialogServiceEventArgs e)
    {
        BudgetCategoryGroup selectedCategoryGroup = GetCategoryGroup(e, true, false);
        ObservableCategoryGroup? listedCategoryGroup = CategoryGroups.FirstOrDefault(c => c.BudgetCategoryGroup.BudgetCategoryGroupID == selectedCategoryGroup.BudgetCategoryGroupID);
        ObservableExpander? listedExpander = Expanders.FirstOrDefault(e => e.CategoryGroupID == selectedCategoryGroup.BudgetCategoryGroupID);
        int index;

        if(listedCategoryGroup != null)
        {
            await _dataStore.BudgetCategoryGroup.DeleteAsync(selectedCategoryGroup);

            Guid categoryGroupId = selectedCategoryGroup.BudgetCategoryGroupID;
            index = CategoryGroups.IndexOf(listedCategoryGroup);
            CategoryGroups.RemoveAt(index);
            Expanders.Remove(listedExpander!);

            if (CategoryGroups.Count == 0)
                await _apiService.DeleteAsync($"budgetcategorygroups/{categoryGroupId}");
        }
    }

    private async void EditCategoryGroupAsync(object? sender, DialogServiceEventArgs e)
    {
        BudgetCategoryGroup selectedCategoryGroup = GetCategoryGroup(e, false, true);
        ObservableCategoryGroup? listedCategoryGroup = CategoryGroups.FirstOrDefault(c => c.BudgetCategoryGroup.BudgetCategoryGroupID == selectedCategoryGroup.BudgetCategoryGroupID);
        ObservableExpander? listedExpander = Expanders.FirstOrDefault(e => e.CategoryGroupID == selectedCategoryGroup.BudgetCategoryGroupID);

        int index;

        if (listedCategoryGroup != null)
        {
            index = CategoryGroups.IndexOf(listedCategoryGroup);

            CategoryGroups[index].CategoryGroupDesc = GetGroupDescEditTxt(e);
            selectedCategoryGroup.CategoryGroupDesc = GetGroupDescEditTxt(e);
            var exp = Expanders.FirstOrDefault(e => e.CategoryGroupID == selectedCategoryGroup.BudgetCategoryGroupID);
            exp.CategoryGroupDesc = GetGroupDescEditTxt(e);
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
                    Categories?.Add(new ObservableCategoryItem(newCategoryItem));
                    listedExpander.AddItemToExpanderList(newCategoryItem);

                }
                else if (radStatus == "Remove Category Item")
                {
                    //Get selected category Item 
                    BudgetCategory selectedCatItem = GetCategoryItem(e);
                    ObservableCategoryItem? listedCatItem = Categories?.FirstOrDefault(i => i.BudgetCategory.BudgetCategoryID == selectedCatItem.BudgetCategoryID);

                    if (listedCatItem != null)
                    {
                        await _dataStore.BudgetCategory.DeleteAsync(selectedCatItem);

                        Guid categoryId = selectedCatItem.BudgetCategoryID;
                        index = Categories.IndexOf(listedCatItem);
                        Categories.RemoveAt(index);
                        listedExpander.DeleteItemFromExpanderList(selectedCatItem);

                        if (Categories.Count == 0)
                            await _apiService.DeleteAsync($"budgetcategories/{categoryId}");
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
                    ObservableCategoryItem? listedCatItem = Categories?.FirstOrDefault(i => i.BudgetCategory.BudgetCategoryID == selectedCatItem.BudgetCategoryID);
                    if (listedCatItem != null)
                    {
                        if (selectedCatItem != null)
                        {
                            selectedCatItem.CategoryName = GetCategoryItemNameTxt(e);
                            selectedCatItem.CategoryAmount = GetCategoryItemBudgetAmt(e);
                            await _dataStore.BudgetCategory.Update(selectedCatItem);

                            index = Categories.IndexOf(listedCatItem);
                            Categories[index].CategoryName = GetCategoryItemNameTxt(e);
                            Categories[index].CategoryAmount = GetCategoryItemBudgetAmt(e);

                            listedExpander.EditItemInExpanderList(selectedCatItem);
                        }
                    }
                }
            }
        }
    }
}

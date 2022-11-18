using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DesktopApplication.Contracts.Data;
using DesktopApplication.Contracts.Services;
using DesktopApplication.CustomEventArgs;
using DesktopApplication.Models;
using DesktopApplication.ViewModels.Forms;
using DesktopApplication.Views.Forms;
using ModelsLibrary;

namespace DesktopApplication.ViewModels;

public class AccountsViewModel : ObservableRecipient
{
    private readonly ISessionService _sessionService;
    private readonly IDialogService _dialogService;
    private readonly IDataStore _dataStore;

    public AccountsViewModel()
    {
        _sessionService = App.GetService<ISessionService>();
        _dialogService = App.GetService<IDialogService>();
        _dataStore = App.GetService<IDataStore>();

        ShowAddDialogCommand = new AsyncRelayCommand(ShowAddDialog);
        ShowEditDialogCommand = new AsyncRelayCommand(ShowEditDialog);
        ShowDeleteDialogCommand = new AsyncRelayCommand(ShowDeleteDialog);
    }

    public IAsyncRelayCommand ShowAddDialogCommand { get; }
    public IAsyncRelayCommand ShowEditDialogCommand { get; }
    public IAsyncRelayCommand ShowDeleteDialogCommand { get; }
    public ObservableCollection<ObservableBankAccount> BankAccounts { get; set; } = new();

    private ObservableBankAccount? _selectedBankAccount;
    public ObservableBankAccount? SelectedBankAccount
    {
        get => _selectedBankAccount;
        set
        {
            SetProperty(ref _selectedBankAccount, value);
            HasItemSelected = value is not null;
        }
    }

    private bool _hasItemSelected;
    public bool HasItemSelected
    {
        get => _hasItemSelected;
        set => SetProperty(ref _hasItemSelected, value);
    }

    public async Task LoadAsync()
    {
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

    private async Task ShowAddDialog()
    {
        _dialogService.OnSaved += AddBankAccountAsync;
        
        var dialogTitle = "Add Account";        
        await _dialogService.ShowDialogAsync<BankAccountForm>(dialogTitle);

        _dialogService.OnSaved -= AddBankAccountAsync;
    }

    private async Task ShowEditDialog()
    {
        _dialogService.OnSaved += EditBankAccountAsync;
        
        var dialogTitle = "Edit Account";
        var _selectedBankAccount = SelectedBankAccount!.BankAccount;
        await _dialogService.ShowDialogAsync<BankAccountForm>(dialogTitle, _selectedBankAccount);
        
        _dialogService.OnSaved -= EditBankAccountAsync;
    }

    private async Task ShowDeleteDialog()
    {
        _dialogService.OnSaved += DeleteBankAccountAsync;

        var dialogTitle = "Delete Account";
        var _selectedBankAccount = SelectedBankAccount!.BankAccount;
        await _dialogService.ShowDialogAsync<BankAccountForm>(dialogTitle, _selectedBankAccount, true);

        _dialogService.OnSaved -= DeleteBankAccountAsync;
    }

    private async void AddBankAccountAsync(object? sender, DialogServiceEventArgs e)
    {
        var newBankAccount = GetBankAccount(e);
        newBankAccount.UserId = _sessionService.GetSessionUserId();

        var result = await _dataStore.BankAccount.AddAsync(newBankAccount);

        if (result == 1)
        {
            BankAccounts.Add(new ObservableBankAccount(newBankAccount));
        }
    }

    private async void EditBankAccountAsync(object? sender, DialogServiceEventArgs e)
    {
        var editedBankAccount = GetBankAccount(e);
        var listedBankAccount = BankAccounts.FirstOrDefault(
            a => a.BankAccount.BankAccountId == editedBankAccount.BankAccountId);
        int index;

        if (listedBankAccount is not null)
        {
            await _dataStore.BankAccount.Update(editedBankAccount);

            index = BankAccounts.IndexOf(listedBankAccount);
            BankAccounts[index].BankAccount = editedBankAccount;
        }
    }

    private async void DeleteBankAccountAsync(object? sender, DialogServiceEventArgs e)
    {
        var selectedBankAccount = _selectedBankAccount!.BankAccount;
        await _dataStore.BankAccount.DeleteAsync(selectedBankAccount);

        BankAccounts.Remove(_selectedBankAccount);
    }

    private static BankAccount GetBankAccount(DialogServiceEventArgs e)
    {
        var accountForm = (BankAccountForm)e.Content;
        return accountForm.ViewModel.BankAccount;
    }
}
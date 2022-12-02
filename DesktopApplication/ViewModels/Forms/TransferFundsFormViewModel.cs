using CommunityToolkit.Mvvm.ComponentModel;
using DesktopApplication.Contracts.Data;
using DesktopApplication.Contracts.Services;
using ModelsLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopApplication.ViewModels.Forms
{
    public class TransferFundsFormViewModel : ObservableObject
    {
        private readonly ISessionService _sessionService;
        private readonly IDataStore _dataStore;

        public TransferFundsFormViewModel()
        {
            _sessionService = App.GetService<ISessionService>();
            _dataStore = App.GetService<IDataStore>();
        }

        public ObservableCollection<BankAccount?> BankAccounts { get; set; } = new();
        public ObservableCollection<BankAccount?> TransferToAccounts { get; set; } = new();

        private BankAccount? _selectedTransferFromAccount;
        public BankAccount? SelectedTransferFromAccount
        {
            get => _selectedTransferFromAccount;
            set
            {
                SetProperty(ref _selectedTransferFromAccount, value);
                VerifySelectedAccountsDoNotMatch();
                FilterTransferToAccountsCollection();
            }
        }

        private BankAccount? _selectedTransferToAccount;
        public BankAccount? SelectedTransferToAccount
        {
            get => _selectedTransferToAccount;
            set => SetProperty(ref _selectedTransferToAccount, value);
        }

        private string? _transferAmount;
        public string? TransferAmount
        {
            get => _transferAmount;
            set => SetProperty(ref _transferAmount, value);
        }

        public async Task LoadAsync()
        {
            if (BankAccounts.Any()) { return; }

            Guid userId = _sessionService.GetSessionUserId();
            IEnumerable<BankAccount?> _userAccounts = await _dataStore.BankAccount.ListAsync(a => a.UserId == userId);

            if (_userAccounts is not null)
            {
                _userAccounts.ToList().ForEach(a => BankAccounts.Add(a));
                _userAccounts.ToList().ForEach(a => TransferToAccounts.Add(a));
            }
        }

        public bool IsSufficientTransferAmount()
        {
            if (_selectedTransferFromAccount is not null && _transferAmount is not null)
            {
                decimal balance = _selectedTransferFromAccount.Balance;
                decimal transferAmount;

                if (decimal.TryParse(_transferAmount, out transferAmount) && transferAmount <= balance)
                    return true;
            }

            return false;
        }

        private void VerifySelectedAccountsDoNotMatch()
        {
            if (_selectedTransferFromAccount == _selectedTransferToAccount)
                SelectedTransferToAccount = null;
        }

        private void FilterTransferToAccountsCollection()
        {
            ResetTransferToAccountsCollection();

            if (_selectedTransferFromAccount is not null)
            {
                BankAccount? matchingSelection = BankAccounts.FirstOrDefault(b => b?.BankAccountId == _selectedTransferFromAccount.BankAccountId);
                TransferToAccounts.Remove(matchingSelection);
            }
        }

        private void ResetTransferToAccountsCollection()
        {
            TransferToAccounts.Clear();

            foreach (BankAccount? account in BankAccounts)
                TransferToAccounts.Add(account);
        }
    }
}

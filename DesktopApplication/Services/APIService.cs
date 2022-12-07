using DesktopApplication.Contracts.Services;
using ModelsLibrary;
using ModelsLibrary.Utilities.Comparers;

namespace DesktopApplication.Services
{
    public class APIService : GenericApiService, IAPIService
    {
        public async Task<IEnumerable<BankAccount>> GetUserAccounts(Guid userId)
        {
            IEnumerable<BankAccount>? _accounts = await GetAsync<IEnumerable<BankAccount>>("bankaccounts");
            IEnumerable<BankAccount>? userAccounts = null;

            if (_accounts is not null)
                userAccounts = _accounts.Where(b => b.UserId == userId).ToList();
            
            return userAccounts ??= new List<BankAccount>();
        }

        public async Task<IEnumerable<BudgetCategoryGroup>> GetCategoryGroups(Budget _budget)
        {
            Budget? budget = await GetAsync<Budget>($"budgets/{_budget.BudgetId}");

            if (budget is null) 
                return new List<BudgetCategoryGroup>();

            if (budget.BudgetCategoryGroups is not null && budget.BudgetCategoryGroups.Any())
                return budget.BudgetCategoryGroups;
            else
                return new List<BudgetCategoryGroup>();
        }

        public async Task<IEnumerable<BudgetCategory>> GetCategories(Budget _budget)
        {
            IEnumerable<BudgetCategory>? _allCategories = await GetAsync<IEnumerable<BudgetCategory>>("budgetcategories");
            List<BudgetCategoryGroup> budgetGroups = new();
            List<BudgetCategory>? categories = new();

            if (_budget.BudgetCategoryGroups is null || !_budget.BudgetCategoryGroups.Any()
                || _allCategories is null || !_allCategories.Any())
                return new List<BudgetCategory>();

            budgetGroups = _budget.BudgetCategoryGroups.ToList();

            foreach (BudgetCategory category in _allCategories)
            {
                foreach (BudgetCategoryGroup group in budgetGroups)
                {
                    if (category.BudgetCategoryGroupID.ToString() == group.BudgetCategoryGroupID.ToString())
                        categories.Add(category);
                }
            }

            return categories;
        }

        public async Task<IEnumerable<Transaction>> GetTransactions(Guid _userid)
        {
            IEnumerable<Transaction>? _allTransactions = await GetAsync<IEnumerable<Transaction>>("transactions");
            IEnumerable<Transaction> transactions;

            if (_allTransactions is null || !_allTransactions.Any())
                return new List<Transaction>();

            transactions = _allTransactions.Where(t => t.BankAccount!.UserId == _userid);

            return transactions ??= new List<Transaction>();
        }

        public async Task UpdateAccounts(IEnumerable<BankAccount> _apiAccounts, IEnumerable<BankAccount> _databaseAccounts)
        {
            IEnumerable<BankAccount> _savedAccounts = _databaseAccounts.Join(_apiAccounts, a => a.BankAccountId, b => b.BankAccountId, (a, b) => a);
            IEnumerable<BankAccount> _deletedAccounts = _apiAccounts.Where(a => !_savedAccounts.Select(b => b.BankAccountId).Contains(a.BankAccountId));
            IEnumerable<BankAccount> _addedAccounts = _databaseAccounts.Except(_savedAccounts, new BankAccountComparer());
            IEnumerable<BankAccount> _changedAccounts = _savedAccounts.Except(_apiAccounts, new BankAccountComparer());
            string endpoint = "bankaccounts";

            if (_addedAccounts.Any())
                _addedAccounts.ToList().ForEach(async a => await PostAsync(endpoint, a));

            if (_changedAccounts.Any())
                _changedAccounts.ToList().ForEach(async a => await PutAsync($"{endpoint}/{a.BankAccountId}", a));

            if (_deletedAccounts.Any())
                _deletedAccounts.ToList().ForEach(async a => await DeleteAsync($"{endpoint}/{a.BankAccountId}"));
        }

        public async Task UpdateCategoryGroups(IEnumerable<BudgetCategoryGroup> _apiGroups, IEnumerable<BudgetCategoryGroup> _databaseGroups)
        {
            IEnumerable<BudgetCategoryGroup> _savedgroups = _databaseGroups.Join(_apiGroups, a => a.BudgetCategoryGroupID, b => b.BudgetCategoryGroupID, (a, b) => a);
            IEnumerable<BudgetCategoryGroup> _deletedGroups = _apiGroups.Where(a => !_savedgroups.Select(b => b.BudgetCategoryGroupID).Contains(a.BudgetCategoryGroupID));
            IEnumerable<BudgetCategoryGroup> _addedGroups = _databaseGroups.Except(_savedgroups, new CategoryGroupComparer());
            IEnumerable<BudgetCategoryGroup> _changedGroups = _savedgroups.Except(_apiGroups, new CategoryGroupComparer());
            string endpoint = "budgetcategorygroups";

            if (_addedGroups.Any())
                _addedGroups.ToList().ForEach(async g => await PostAsync(endpoint, g));

            if (_changedGroups.Any())
                _changedGroups.ToList().ForEach(async g => await PutAsync($"{endpoint}/{g.BudgetCategoryGroupID}", g));

            if (_deletedGroups.Any())
                _deletedGroups.ToList().ForEach(async g => await DeleteAsync($"{endpoint}/{g.BudgetCategoryGroupID}"));
        }

        public async Task UpdateCategories(IEnumerable<BudgetCategory> _apiCategories, IEnumerable<BudgetCategory> _databaseCategories)
        {
            IEnumerable<BudgetCategory> _savedCategories = _databaseCategories.Join(_apiCategories, a => a.BudgetCategoryID, b => b.BudgetCategoryID, (a, b) => a);
            IEnumerable<BudgetCategory> _deletedCategories = _apiCategories.Where(a => !_savedCategories.Select(b => b.BudgetCategoryID).Contains(a.BudgetCategoryID));
            IEnumerable<BudgetCategory> _addedCategories = _databaseCategories.Except(_savedCategories, new BudgetCategoryComparer());
            IEnumerable<BudgetCategory> _changedCategories = _savedCategories.Except(_apiCategories, new BudgetCategoryComparer());
            string endpoint = "budgetcategories";

            if (_addedCategories.Any())
                _addedCategories.ToList().ForEach(async c => await PostAsync(endpoint, c));

            if (_changedCategories.Any())
                _changedCategories.ToList().ForEach(async c => await PutAsync($"{endpoint}/{c.BudgetCategoryID}", c));

            if (_deletedCategories.Any())
                _deletedCategories.ToList().ForEach(async c => await DeleteAsync($"{endpoint}/{c.BudgetCategoryID}"));
        }

        public async Task UpdateTransactions(IEnumerable<Transaction> _apiTransactions, IEnumerable<Transaction> _databaseTransactions)
        {
            IEnumerable<Transaction> _savedTransactions = _databaseTransactions.Join(_apiTransactions, a => a.TransactionId, b => b.TransactionId, (a, b) => a);
            IEnumerable<Transaction> _deletedTransactions = _apiTransactions.Where(a => !_savedTransactions.Select(b => b.TransactionId).Contains(a.TransactionId));
            IEnumerable<Transaction> _addedTransactions = _databaseTransactions.Except(_savedTransactions, new TransactionComparer());
            IEnumerable<Transaction> _changedTransactions = _savedTransactions.Except(_apiTransactions, new TransactionComparer());
            string endpoint = "transactions";

            if (_addedTransactions.Any())
                _addedTransactions.ToList().ForEach(async t => await PostAsync(endpoint, t));

            if (_changedTransactions.Any())
                _changedTransactions.ToList().ForEach(async t => await PutAsync($"{endpoint}/{t.TransactionId}", t));

            if (_deletedTransactions.Any())
                _deletedTransactions.ToList().ForEach(async t => await DeleteAsync($"{endpoint}/{t.TransactionId}"));
        }
    }
}
using ModelsLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopApplication.Contracts.Services
{
    public interface IAPIService : IGenericApiService
    {
        Task<IEnumerable<BankAccount>> GetUserAccounts(Guid userId);
        Task<IEnumerable<BudgetCategoryGroup>> GetCategoryGroups(Budget _budget);
        Task<IEnumerable<BudgetCategory>> GetCategories(Budget _budget);
        Task<IEnumerable<Transaction>> GetTransactions(Guid _userid);
        Task UpdateAccounts(IEnumerable<BankAccount> _apiAccounts, IEnumerable<BankAccount> _databaseAccounts);
        Task UpdateCategoryGroups(IEnumerable<BudgetCategoryGroup> _apiGroups, IEnumerable<BudgetCategoryGroup> _databaseGroups);
        Task UpdateCategories(IEnumerable<BudgetCategory> _apiCategories, IEnumerable<BudgetCategory> _databaseCategories);
        Task UpdateTransactions(IEnumerable<Transaction> _apiTransactions, IEnumerable<Transaction> _databaseTransactions);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesktopApplication.Data;
using Microsoft.EntityFrameworkCore;
using ModelsLibrary;

namespace DesktopApplication.Contracts.Data;
public interface IDataStore
{
    public IAccountRepository BankAccount { get; }
    public IBudgetRepository Budget { get; }
    public IRepository<BudgetCategory> BudgetCategory { get; }
    public IRepository<BudgetCategoryGroup> BudgetCategoryGroup { get; }
    public ITransactionRepository Transaction { get; }
    public IRepository<User> User { get; }
    void Commit();

    Task<int> CommitAsync();
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesktopApplication.Contracts.Data;
using ModelsLibrary;

namespace DesktopApplication.Data;
public class DataStore : IDataStore
{
    private readonly BudgetAppContext dbContext;

    public IRepository<BankAccount>? _BankAccount;

    public IRepository<Budget>? _Budget;

    public IRepository<BudgetCategory>? _BudgetCategory;

    public IRepository<BudgetCategoryGroup>? _BudgetCategoryGroup;

    public IRepository<Transaction>? _Transaction;

    public IRepository<User>? _User;

    public DataStore(BudgetAppContext dbContext)
    {
        this.dbContext = dbContext;
        dbContext.Database.EnsureCreated();
    }

    public IRepository<BankAccount> BankAccount
    {
        get
        {
            _BankAccount ??= new Repository<BankAccount>(dbContext);
            return _BankAccount;
        }
    }

    public IRepository<Budget> Budget
    {
        get
        {
            _Budget ??= new Repository<Budget>(dbContext);
            return _Budget;
        }
    }

    public IRepository<BudgetCategory> BudgetCategory
    {
        get
        {
            _BudgetCategory ??= new Repository<BudgetCategory>(dbContext);
            return _BudgetCategory;
        }
    }

    public IRepository<BudgetCategoryGroup> BudgetCategoryGroup
    {
        get
        {
            _BudgetCategoryGroup ??= new Repository<BudgetCategoryGroup>(dbContext);
            return _BudgetCategoryGroup;
        }
    }

    public IRepository<Transaction> Transaction
    {
        get
        {
            _Transaction ??= new Repository<Transaction>(dbContext);
            return _Transaction;
        }
    }

    public IRepository<User> User
    {
        get
        {
            _User ??= new Repository<User>(dbContext);
            return _User;
        }
    }

    public async Task<int> CommitAsync()
    {
        return await dbContext.SaveChangesAsync();
    }
    public void Dispose()
    {
        dbContext.Dispose();
    }
    public void Commit()
    {
        dbContext.SaveChanges();
    }
}

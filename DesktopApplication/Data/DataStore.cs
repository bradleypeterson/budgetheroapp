using DesktopApplication.Contracts.Data;
using ModelsLibrary;

namespace DesktopApplication.Data;
public class DataStore : IDataStore
{
    private readonly BudgetAppContext _dbContext;

    public IAccountRepository? _BankAccount;

    public IBudgetRepository? _Budget;

    public IRepository<BudgetCategory>? _BudgetCategory;

    public IRepository<BudgetCategoryGroup>? _BudgetCategoryGroup;

    public ITransactionRepository? _Transaction;

    public IRepository<User>? _User;

    public DataStore(BudgetAppContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IAccountRepository BankAccount
    {
        get
        {
            _BankAccount ??= new AccountRepository(_dbContext);
            return _BankAccount;
        }
    }

    public IBudgetRepository Budget
    {
        get
        {
            _Budget ??= new BudgetRepository(_dbContext);
            return _Budget;
        }
    }

    public IRepository<BudgetCategory> BudgetCategory
    {
        get
        {
            _BudgetCategory ??= new Repository<BudgetCategory>(_dbContext);
            return _BudgetCategory;
        }
    }

    public IRepository<BudgetCategoryGroup> BudgetCategoryGroup
    {
        get
        {
            _BudgetCategoryGroup ??= new Repository<BudgetCategoryGroup>(_dbContext);
            return _BudgetCategoryGroup;
        }
    }

    public ITransactionRepository Transaction
    {
        get
        {
            _Transaction ??= new TransactionRepository(_dbContext);
            return _Transaction;
        }
    }

    public IRepository<User> User
    {
        get
        {
            _User ??= new Repository<User>(_dbContext);
            return _User;
        }
    }

    public BudgetAppContext GetDbContext() => _dbContext;

    public async Task<int> CommitAsync()
    {
        return await _dbContext.SaveChangesAsync();
    }
    public void Dispose()
    {
        _dbContext.Dispose();
    }
    public void Commit()
    {
        _dbContext.SaveChanges();
    }
}

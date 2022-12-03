using ModelsLibrary;

namespace Web_API.Contracts.Data;
public interface IDataStore
{
    public IRepository<BankAccount> BankAccount { get; }
    public IRepository<Budget> Budget { get; }
    public IRepository<BudgetCategory> BudgetCategory { get; }
    public IRepository<BudgetCategoryGroup> BudgetCategoryGroup { get; }
    public IRepository<Transaction> Transaction { get; }
    public IRepository<User> User { get; }

    void Commit();

    Task<int> CommitAsync();
}

using DesktopApplication.Contracts.Data;
using Microsoft.EntityFrameworkCore;
using ModelsLibrary;
using Syncfusion.UI.Xaml.Data;

namespace DesktopApplication.Data
{
    public class TransactionRepository : Repository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(BudgetAppContext context)
            : base(context) {}

        public async Task SaveWithForeignKeysAsync(IEnumerable<Transaction> transactions)
        {
            Guid? _userId;
            List<BankAccount> accounts = new();
            List<BankAccount> savedAccounts = new();
            List<BudgetCategory> allCategories = new();
            List<BudgetCategory> savedCategories = new();
            List<BudgetCategory> distinctCategories = new();
            List<BudgetCategoryGroup> savedGroups = new();
            List<BudgetCategoryGroup> allGroups = new();
            List<BudgetCategoryGroup> distinctGroups = new();

            _userId = transactions.Select(b => b.BankAccount!.UserId).FirstOrDefault();

            if (_context.BankAccounts is null)
                throw new ArgumentNullException("No accounts have been saved to the local database.");

            if (_context.BudgetCategories is null)
                throw new ArgumentNullException("No categories have been found in the local database.");

            //transactions.ForEach(t => allCategories.Add(t.BudgetCategory!));

            //if (allCategories is null || !allCategories.Any())
            //    throw new ArgumentNullException("No categories have been found attached to the incoming model.");

            //distinctCategories = allCategories.Distinct().ToList();

            //allCategories.ForEach(t => allGroups.Add(t.BudgetCategoryGroup!));

            //if (allGroups is null || !allGroups.Any())
            //    throw new ArgumentNullException("No category groups have been found attached to the incoming model.");

            //distinctGroups = allGroups.Distinct().ToList();

            //savedGroups = _context.BudgetCategoryGroups!
            //    .Where(g => distinctGroups.Contains(g)).ToList();

            //foreach (BudgetCategory category in distinctCategories)
            //    category.BudgetCategoryGroup = savedGroups
            //        .FirstOrDefault(g => g.BudgetCategoryGroupID == category.BudgetCategoryGroupID);

            savedCategories = _context.BudgetCategories.Include(b => b.BudgetCategoryGroup).ToList();

            savedAccounts = _context.BankAccounts.Include(u => u.User)
                .Where(u => u.UserId == _userId).ToList();

            if (savedAccounts is null || !savedAccounts.Any())
                throw new ArgumentNullException("No accounts have been found for this user");

            foreach (Transaction transaction in transactions)
            {
                transaction.BankAccount = savedAccounts
                    .FirstOrDefault(a => a.BankAccountId == transaction.BankAccountId);

                transaction.BudgetCategory = savedCategories
                    .FirstOrDefault(c => c.BudgetCategoryGroupID == transaction.BudgetCategory!.BudgetCategoryGroupID);
            }

            _context.BudgetCategories.AddRange(distinctCategories);
            _context.Transactions!.AddRange(transactions);

            int result = await _context.SaveChangesAsync();

            if (result == 0)
                throw new Exception("Not all incoming models could be saved to the local database.");
        }
    }
}

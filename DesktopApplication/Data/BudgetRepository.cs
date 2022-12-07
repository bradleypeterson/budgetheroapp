using DesktopApplication.Contracts.Data;
using Microsoft.EntityFrameworkCore;
using ModelsLibrary;
using Syncfusion.UI.Xaml.Data;

namespace DesktopApplication.Data
{
    public class BudgetRepository : Repository<Budget>, IBudgetRepository
    {
        public BudgetRepository(BudgetAppContext context) 
            : base(context) {}

        public Budget GetPersonalBudget(Guid userId)
        {            
            User? user = _context.Users!
                .Include(b => b.Budgets!)
                .ThenInclude(g => g.BudgetCategoryGroups)
                .FirstOrDefault(u => u.UserId == userId);

            if (user is null)
                throw new ArgumentNullException("Session user is not set.");

            if (user.Budgets is null)
                throw new ArgumentNullException("Session user does not have any budgets saved.");

            Budget? budget = user.Budgets.Where(b => b.BudgetType == "personal").SingleOrDefault();

            if (budget is null)
                throw new ArgumentNullException("Session user does not have a personal budget saved.");

            return (Budget)budget;
        }

        public Budget GetHouseholdBudget(Guid userId)
        {
            User? user = _context.Users!
                .Include(b => b.Budgets!)
                .ThenInclude(g => g.BudgetCategoryGroups)
                .FirstOrDefault(u => u.UserId == userId);

            if (user is null)
                throw new ArgumentNullException("Session user is not set.");

            if (user.Budgets is null)
                throw new ArgumentNullException("Session user does not have any budgets saved.");

            Budget? budget = user.Budgets.Where(b => b.BudgetType == "household").SingleOrDefault();

            if (budget is null)
                throw new ArgumentNullException("Session user does not have a household budget saved.");

            return (Budget)budget;
        }

        public async Task<IEnumerable<BudgetCategory>> GetBudgetCategories(Budget budget)
        {
            if (_context.BudgetCategories is not null && _context.BudgetCategories.Any())
            {
                IEnumerable<BudgetCategory> categories = await _context.BudgetCategories
                    .Include(g => g.BudgetCategoryGroup!)
                    .ThenInclude(b => b.Budgets)
                    .Where(b => b.BudgetCategoryGroup!.Budgets!.Contains(budget))
                    .ToListAsync();

                return categories ??= new List<BudgetCategory>();
            }
            else
                return new List<BudgetCategory>();
        }

        public async Task SaveWithForeignKeysAsync(IEnumerable<BudgetCategory> categories, Budget budget)
        {
            IEnumerable<BudgetCategoryGroup> categoryGroups;

            categoryGroups = _context.BudgetCategoryGroups!.Include(b => b.Budgets).ToList();

            //foreach (BudgetCategory category in categories)
            //    foreach (BudgetCategoryGroup group in categoryGroups)
            //        if (categoryGroups.Contains(category.BudgetCategoryGroup))
            //            category.BudgetCategoryGroup = group;

            foreach (BudgetCategory category in categories)
                category.BudgetCategoryGroup = budget.BudgetCategoryGroups!
                    .Where(g => g.BudgetCategoryGroupID == category.BudgetCategoryGroupID)
                    .FirstOrDefault();

            _context.BudgetCategories!.AddRange(categories);

            int result = await _context.SaveChangesAsync();

            if (result == 0)
                throw new Exception("Not all incoming models could be saved to the local database.");
        }
    }
}

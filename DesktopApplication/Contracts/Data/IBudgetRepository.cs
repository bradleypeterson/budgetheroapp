using ModelsLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopApplication.Contracts.Data
{
    public interface IBudgetRepository : IRepository<Budget>
    {
        Budget GetPersonalBudget(Guid userId);
        Budget GetHouseholdBudget(Guid userId);

        Task<IEnumerable<BudgetCategory>> GetBudgetCategories(Budget budget);
        Task SaveWithForeignKeysAsync(IEnumerable<BudgetCategory> categories, Budget budget);
    }
}

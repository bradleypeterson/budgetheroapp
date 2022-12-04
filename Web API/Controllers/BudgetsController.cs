using DebugTools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelsLibrary;
using ModelsLibrary.DTO;
using ModelsLibrary.Utilities;
using Web_API.Contracts.Data;
using Web_API.Data;

namespace Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetsController : ControllerBase
    {
        private readonly IDataStore _dataStore;

        public BudgetsController(IDataStore dataStore)
        {
            _dataStore = dataStore;
        }

        // GET: api/Budgets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Budget>>> GetBudgets()
        {
            IEnumerable<Budget>? _budgets = await _dataStore.Budget.GetAllAsync(null, "Users");
            await LoadBudgetCategoryGroups(_budgets);

            return _budgets.ToList();
        }

        // GET: api/Budgets/5
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Budget>> GetBudget(Guid id)
        {
            Budget? budget = await _dataStore.Budget.GetAsync(b => b.BudgetId == id, false, "Users");

            if (budget == null)
                return NotFound();

            await LoadBudgetCategoryGroups(budget);

            return budget;
        }

        // PUT: api/Budgets/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> PutBudget(Guid id, Budget budget)
        {
            if (id != budget.BudgetId)
                return BadRequest();

            try
            {
                await _dataStore.Budget.Update(budget);
            }
            catch (DbUpdateConcurrencyException)
            {
                bool budgetExists = await BudgetExists(id);

                if (!budgetExists)
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // POST: api/Budgets
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Budget>> PostBudget(Budget budget)
        {
            bool budgetExists = await BudgetExists(budget.BudgetId);

            if (!budgetExists)
            {
                if (budget.Users is not null)
                {
                    List<User> users = budget.Users.ToList();
                    budget.Users = null;
                    await _dataStore.Budget.AddAsync(budget);
                    budget.Users = users;
                    await _dataStore.Budget.Update(budget);
                }

                return CreatedAtAction("GetBudget", new { id = budget.BudgetId }, budget);
            }
            else
                return StatusCode(422);
        }

        // DELETE: api/Budgets/5
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteBudget(Guid id)
        {
            Budget? budget = await _dataStore.Budget.GetAsync(b => b.BudgetId == id);

            if (budget == null)
            {
                return NotFound();
            }

            await _dataStore.Budget.DeleteAsync(budget);

            return NoContent();
        }

        private async Task<bool> BudgetExists(Guid id)
        {
            Budget? budget = await _dataStore.Budget.GetByIdAsync(id);

            if (budget is null)
                return false;
            else
                return true;
        }

        private async Task LoadBudgetCategoryGroups(Budget budget)
        {
            Budget? _budget = await _dataStore.Budget.GetAsync(c => c.BudgetId == budget.BudgetId, false, "BudgetCategoryGroups");

            if (_budget is not null)
                budget = _budget;
        }

        private async Task LoadBudgetCategoryGroups(IEnumerable<Budget> budgets)
        {
            foreach (Budget budget in budgets)
                await LoadBudgetCategoryGroups(budget);
        }
    }
}

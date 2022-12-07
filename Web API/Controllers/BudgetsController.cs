using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelsLibrary;
using Web_API.Data;

namespace Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BudgetsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Budgets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Budget>>> GetBudgets()
        {
            IEnumerable<Budget> budgets = await _context.Budgets
                .Include(u => u.Users)
                .Include(g => g.BudgetCategoryGroups)
                .ToListAsync();

            if (budgets is null || !budgets.Any())
                return NoContent();
            else
                return Ok(budgets);
        }

        // GET: api/Budgets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Budget>> GetBudget(Guid id)
        {
            IEnumerable<Budget> budgets = await _context.Budgets
                .Include(u => u.Users)
                .Include(g => g.BudgetCategoryGroups)
                .ToListAsync();

            Budget? budget = budgets.FirstOrDefault(b => b.BudgetId == id);

            if (budget is null)
                return NotFound();

            return budget;
        }

        // PUT: api/Budgets/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> PutBudget(Guid id, Budget budget)
        {
            if (id != budget.BudgetId || !BudgetExists(id))
                return BadRequest();

            Budget? _budget = _context.Budgets.Include(b => b.Users).FirstOrDefault(b => b.BudgetId == id);

            if (_budget is not null && _budget.Users is not null)
            {
                _budget.Users.Clear();
                await _context.SaveChangesAsync();
                _context.ChangeTracker.Clear();
            }

            _context.Update(budget);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BudgetExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Budgets
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Budget>> PostBudget(Budget budget)
        {
            if (BudgetExists(budget.BudgetId))
                return StatusCode(422);

            if (budget.Users is not null)
            {
                Guid id = budget.Users.Select(u => u.UserId).FirstOrDefault();
                User? user = _context.Users.Where(u => u.UserId == id).SingleOrDefault();

                budget.Users.Clear();
                budget.Users.Add(user!);
            }

            _context.Budgets.Add(budget);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBudget", new { id = budget.BudgetId }, budget);
        }

        // DELETE: api/Budgets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBudget(Guid id)
        {
            var budget = await _context.Budgets.FindAsync(id);
            if (budget == null)
            {
                return NotFound();
            }

            _context.Budgets.Remove(budget);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BudgetExists(Guid id)
        {
            return _context.Budgets.Any(e => e.BudgetId == id);
        }
    }
}

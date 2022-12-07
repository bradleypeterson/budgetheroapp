using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelsLibrary;
using Web_API.Data;

namespace Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetCategoriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BudgetCategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/BudgetCategories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BudgetCategory>>> GetBudgetCategories()
        {
            IEnumerable<BudgetCategory> categories = await _context.BudgetCategories
                .Include(g => g.BudgetCategoryGroup!)
                .ThenInclude(b => b.Budgets!)
                .ThenInclude(u => u.Users)
                .ToListAsync();

            if (categories is null || !categories.Any())
                return NoContent();
            else 
                return Ok(categories);
        }

        // GET: api/BudgetCategories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BudgetCategory>> GetBudgetCategory(Guid id)
        {
            IEnumerable<BudgetCategory> categories = await _context.BudgetCategories
                .Include(g => g.BudgetCategoryGroup!)
                .ThenInclude(b => b.Budgets!)
                .ThenInclude(u => u.Users)
                .ToListAsync();

            BudgetCategory? category = categories.FirstOrDefault(c => c.BudgetCategoryID == id);

            if (category == null)
            {
                return NotFound();
            }

            return category;
        }

        // PUT: api/BudgetCategories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBudgetCategory(Guid id, BudgetCategory budgetCategory)
        {
            if (id != budgetCategory.BudgetCategoryID || !BudgetCategoryExists(id))
                return BadRequest();

            _context.Entry(budgetCategory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BudgetCategoryExists(id))
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

        // POST: api/BudgetCategories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BudgetCategory>> PostBudgetCategory(BudgetCategory category)
        {
            if (BudgetCategoryExists(category.BudgetCategoryID))
                return StatusCode(422);

            BudgetCategoryGroup? categoryGroup = await _context.BudgetCategoryGroups.FindAsync(category.BudgetCategoryGroupID);

            if (categoryGroup == null)
                return BadRequest();

            category.BudgetCategoryGroup = categoryGroup;
            _context.BudgetCategories.Add(category);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBudgetCategory", new { id = category.BudgetCategoryID }, category);
        }

        // DELETE: api/BudgetCategories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBudgetCategory(Guid id)
        {
            var budgetCategory = await _context.BudgetCategories.FindAsync(id);
            if (budgetCategory == null)
            {
                return NotFound();
            }

            _context.BudgetCategories.Remove(budgetCategory);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BudgetCategoryExists(Guid id)
        {
            return _context.BudgetCategories.Any(e => e.BudgetCategoryID == id);
        }

        private bool BudgetCategoryGroupExists(Guid id)
        {
            return _context.BudgetCategories.Any(c => c.BudgetCategoryID == id);
        }
    }
}

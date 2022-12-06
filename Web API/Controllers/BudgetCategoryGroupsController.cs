using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelsLibrary;
using Web_API.Data;

namespace Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetCategoryGroupsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BudgetCategoryGroupsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/BudgetCategoryGroups
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BudgetCategoryGroup>>> GetBudgetCategoryGroups()
        {
            IEnumerable<BudgetCategoryGroup> categoryGroups = await _context.BudgetCategoryGroups
                .Include(b => b.Budgets!)
                .ThenInclude(u => u.Users)
                .ToListAsync();

            if (categoryGroups is null || !categoryGroups.Any())
                return NoContent();
            else
                return Ok(categoryGroups);
        }

        // GET: api/BudgetCategoryGroups/5
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<BudgetCategoryGroup>> GetBudgetCategoryGroup(Guid id)
        {
            IEnumerable<BudgetCategoryGroup> categoryGroups = await _context.BudgetCategoryGroups
                .Include(b => b.Budgets!)
                .ThenInclude(u => u.Users)
                .ToListAsync();

            BudgetCategoryGroup? categoryGroup = categoryGroups.FirstOrDefault(c => c.BudgetCategoryGroupID == id);

            if (categoryGroup is null)
                return NotFound();

            return categoryGroup;
        }

        // PUT: api/BudgetCategoryGroups/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> PutBudgetCategoryGroup(Guid id, BudgetCategoryGroup budgetCategoryGroup)
        {
            if (id != budgetCategoryGroup.BudgetCategoryGroupID || !BudgetCategoryGroupExists(id))
            {
                return BadRequest();
            }

            _context.Entry(budgetCategoryGroup).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BudgetCategoryGroupExists(id))
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

        // POST: api/BudgetCategoryGroups
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BudgetCategoryGroup>> PostBudgetCategoryGroup(BudgetCategoryGroup categoryGroup)
        {
            if (BudgetCategoryGroupExists(categoryGroup.BudgetCategoryGroupID))
                return StatusCode(422);

            if (categoryGroup.Budgets is not null)
            {
                Guid id = categoryGroup.Budgets.Select(b => b.BudgetId).FirstOrDefault();
                Budget? budget = _context.Budgets.Where(b => b.BudgetId == id).SingleOrDefault();

                categoryGroup.Budgets.Clear();
                categoryGroup.Budgets.Add(budget!);
            }

            _context.BudgetCategoryGroups.Add(categoryGroup);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBudgetCategoryGroup", new { id = categoryGroup.BudgetCategoryGroupID }, categoryGroup);
        }

        // DELETE: api/BudgetCategoryGroups/5
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteBudgetCategoryGroup(Guid id)
        {
            var budgetCategoryGroup = await _context.BudgetCategoryGroups.FindAsync(id);
            if (budgetCategoryGroup == null)
            {
                return NotFound();
            }

            _context.BudgetCategoryGroups.Remove(budgetCategoryGroup);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BudgetCategoryGroupExists(Guid id)
        {
            return _context.BudgetCategoryGroups.Any(e => e.BudgetCategoryGroupID == id);
        }
    }
}

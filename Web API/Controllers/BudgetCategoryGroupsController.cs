using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelsLibrary;
using Web_API.Models;

namespace Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetCategoryGroupsController : ControllerBase
    {
        private readonly BudgetHeroAPIDbContext _context;

        public BudgetCategoryGroupsController(BudgetHeroAPIDbContext context)
        {
            _context = context;
        }

        // GET: api/BudgetCategoryGroups
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BudgetCategoryGroup>>> GetBudgetCategoryGroups()
        {
            return await _context.BudgetCategoryGroups.ToListAsync();
        }

        // GET: api/BudgetCategoryGroups/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BudgetCategoryGroup>> GetBudgetCategoryGroup(Guid id)
        {
            var budgetCategoryGroup = await _context.BudgetCategoryGroups.FindAsync(id);

            if (budgetCategoryGroup == null)
            {
                return NotFound();
            }

            return budgetCategoryGroup;
        }

        // PUT: api/BudgetCategoryGroups/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBudgetCategoryGroup(Guid id, BudgetCategoryGroup budgetCategoryGroup)
        {
            if (id != budgetCategoryGroup.BudgetCategoryGroupID)
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
        public async Task<ActionResult<BudgetCategoryGroup>> PostBudgetCategoryGroup(BudgetCategoryGroup budgetCategoryGroup)
        {
            _context.BudgetCategoryGroups.Add(budgetCategoryGroup);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBudgetCategoryGroup", new { id = budgetCategoryGroup.BudgetCategoryGroupID }, budgetCategoryGroup);
        }

        // DELETE: api/BudgetCategoryGroups/5
        [HttpDelete("{id}")]
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

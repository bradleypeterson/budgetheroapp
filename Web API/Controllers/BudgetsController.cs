using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelsLibrary;
using ModelsLibrary.DTO;
using ModelsLibrary.Utilities;
using Web_API.Models;

namespace Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetsController : ControllerBase
    {
        private readonly BudgetHeroAPIDbContext _context;

        public BudgetsController(BudgetHeroAPIDbContext context)
        {
            _context = context;
        }

        // GET: api/Budgets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BudgetDTO>>> GetBudgets()
        {
            List<Budget> budgets = await _context.Budgets.Include(b => b.Users).ToListAsync();

            return AutoMapper.Map(budgets, true).ToList();
        }

        // GET: api/Budgets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BudgetDTO>> GetBudget(Guid id)
        {
            IEnumerable<Budget>? budgets = await _context.Budgets.Include(b => b.Users).ToListAsync();
            Budget? budget = budgets.FirstOrDefault(b => b.BudgetId == id);

            if (budget == null)
            {
                return NotFound();
            }

            return AutoMapper.Map(budget, true);
        }

        // PUT: api/Budgets/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBudget(Guid id, BudgetDTO budgetDTO)
        {
            Budget budget = AutoMapper.ReverseMap(budgetDTO, true);

            if (id != budget.BudgetId)
            {
                return BadRequest();
            }

            _context.Entry(budget).State = EntityState.Modified;

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
        public async Task<ActionResult<BudgetDTO>> PostBudget(BudgetDTO budgetDTO)
        {
            Budget budget = AutoMapper.ReverseMap(budgetDTO, true);

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

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelsLibrary;
using Web_API.Data;

namespace Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            IEnumerable<User> users = await _context.Users
                .Include(b => b.Budgets!)
                .ThenInclude(c => c.BudgetCategoryGroups)
                .ToListAsync();

            if (users is null || !users.Any())
                return NoContent();
            else 
                return Ok(users);
        }

        // GET: api/Users/5
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<User>> GetUser(Guid id)
        {
            IEnumerable<User> users = await _context.Users
                .Include(b => b.Budgets!)
                .ThenInclude(c => c.BudgetCategoryGroups)
                .ToListAsync();

            User? user = users.FirstOrDefault(u => u.UserId == id);

            if (user == null)
                return NotFound();

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> PutUser(Guid id, User user)
        {
            if (id != user.UserId || !UserExists(user.UserId))
                return BadRequest();

            User? _user = _context.Users.Include(b => b.Budgets).FirstOrDefault(u => u.UserId == id);

            if (_user is not null && _user.Budgets is not null)
            {
                if (user.Budgets is not null)
                {
                    List<Budget> newBudgets = new();

                    foreach (Budget budget in user.Budgets)
                    {
                        if (!BudgetExists(budget.BudgetId))
                            newBudgets.Add(budget);
                    }

                    if (newBudgets.Any())
                    {
                        foreach (Budget newBudget in newBudgets)
                        {
                            _context.Budgets.Add(newBudget);
                            await _context.SaveChangesAsync();
                            _context.ChangeTracker.Clear();
                        }
                    }
                }

                _user.Budgets.Clear();
                await _context.SaveChangesAsync();
                _context.ChangeTracker.Clear();
            }

            _context.Update(user);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            if (UserExists(user.UserId))
                return StatusCode(422);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.UserId }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(Guid id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }

        private bool BudgetExists(Guid id)
        {
            return _context.Budgets.Any(e => e.BudgetId == id);
        }
    }
}

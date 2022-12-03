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
    public class UsersController : ControllerBase
    {
        private readonly BudgetHeroAPIDbContext _context;

        public UsersController(BudgetHeroAPIDbContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            List<User>? users = await _context.Users.Include(u => u.Budgets).ToListAsync();
            return AutoMapper.Map(users, true).ToList();
        }

        // GET: api/Users/5
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<UserDTO>> GetUser(Guid id)
        {
            IEnumerable<User> users = await _context.Users.Include(u => u.Budgets).ToListAsync();
            User? user = users.FirstOrDefault(u => u.UserId == id);

            if (user == null)
            {
                return NotFound();
            }

            return AutoMapper.Map(user, true);
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> PutUser(Guid id, UserDTO userDTO)
        {
            User _user = AutoMapper.ReverseMap(userDTO, true);

            if (id != _user.UserId)
            {
                return BadRequest();
            }

            IEnumerable<User> users = await _context.Users.Include(u => u.Budgets).ToListAsync();
            User? user = users.FirstOrDefault(u => u.UserId == id);

            if (user is not null)
            {
                user.FirstName = _user.FirstName;
                user.LastName = _user.LastName;
                user.EmailAddress = _user.EmailAddress;
                user.PercentageMod = _user.PercentageMod;
                user.Username = _user.Username;
                user.Password = _user.Password;
                user.UserImageLink = _user.UserImageLink;


                if (_user.Budgets is not null)
                {
                    if (user.Budgets is null)
                        user.Budgets = new List<Budget>();

                    foreach (Budget _budget in _user.Budgets)
                    {
                        Budget? budget = user.Budgets.FirstOrDefault(b => b.BudgetId == _budget.BudgetId);

                        if (budget is not null)
                        {
                            budget.BudgetName = _budget.BudgetName;
                            budget.BudgetType = _budget.BudgetType;
                        }
                        else
                        {
                            budget = new()
                            {
                                BudgetId = _budget.BudgetId,
                                BudgetName = _budget.BudgetName,
                                BudgetType = _budget.BudgetType,
                            };

                            budget.Users = new List<User>() { user };
                            _context.Budgets.Add(budget);
                            await _context.SaveChangesAsync();
                            user.Budgets.Add(budget);
                        }
                    }
                }

                _context.Entry(user).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(id))
                        return NotFound();
                    else
                        throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserDTO>> PostUser(UserDTO userDTO)
        {
            User user = AutoMapper.ReverseMap(userDTO, true);

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
            {
                return NotFound();
            }

            IEnumerable<Budget> budgets = await _context.Budgets.Include(b => b.Users).ToListAsync();
            List<Budget> ghostBudgets = budgets.Where(b => b.Users.Count() == 0).ToList();

            foreach (Budget budget in ghostBudgets)
                _context.Budgets.Remove(budget);

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
            return _context.Budgets.Any(b => b.BudgetId == id);
        }
    }
}

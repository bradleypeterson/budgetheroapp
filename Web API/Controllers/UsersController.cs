using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelsLibrary;
using ModelsLibrary.Utilities;
using Web_API.Contracts.Data;

namespace Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IDataStore _dataStore;

        public UsersController(IDataStore dataStore)
        {
            _dataStore = dataStore;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            IEnumerable<User> users = await _dataStore.User.GetAllAsync(null, "Budgets");
            await LoadBudgetCategoryGroups(users);

            return users.ToList();
        }

        // GET: api/Users/5
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<User>> GetUser(Guid id)
        {
            User? user = await _dataStore.User.GetAsync(u => u.UserId == id, false, "Budgets");

            if (user == null)
                return NotFound();

            await LoadBudgetCategoryGroups(user);

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> PutUser(Guid id, User _user)
        {
            if (id != _user.UserId)
                return BadRequest();

            User? user = await _dataStore.User.GetAsync(u => u.UserId == _user.UserId, false, "Budgets");

            if (user == null)
                return NotFound();

            user = EntityUtilities.Update(user, _user);

            await UpdateBudgets(user, _user);

            await _dataStore.User.Update(user);

            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            bool userExists = await UserExists(user.UserId);

            if (!userExists)
            {
                await _dataStore.User.AddAsync(user);

                return CreatedAtAction("GetUser", new { id = user.UserId }, user);
            }
            else
                return StatusCode(422);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            User? user = await _dataStore.User.GetAsync(u => u.UserId == id, false, "Budgets");

            if (user is null)
                return NotFound();
            else 
                await _dataStore.User.DeleteAsync(user);

            return NoContent();
        }

        private async Task<bool> UserExists(Guid id)
        {
            User? user = await _dataStore.User.GetByIdAsync(id);
            if (user is null)
                return false;
            else
                return true;
        }

        private async Task LoadBudgetCategoryGroups(User user)
        {
            if (user.Budgets is not null)
            {
                List<Budget> budgets = new List<Budget>();

                foreach (Budget budget in user.Budgets)
                {
                    Budget? _budget = await _dataStore.Budget.GetAsync(c => c.BudgetId == budget.BudgetId, false, "BudgetCategoryGroups");

                    if (_budget is not null)
                        budgets.Add(_budget);
                }

                user.Budgets = budgets;
            }
        }

        private async Task LoadBudgetCategoryGroups(IEnumerable<User> users)
        {
            foreach (User user in users)
            {
                await LoadBudgetCategoryGroups(user);
            }
        }

        private async Task UpdateBudgets(User existingUser, User incomingUser)
        {
            List<Budget> incomingBudgets = (incomingUser.Budgets is null) ? new List<Budget>() : incomingUser.Budgets.ToList();

            if (incomingBudgets.Any())
            {
                IEnumerable<Budget> existingBudgets = await _dataStore.Budget.GetAllAsync(null, "Users");

                if (incomingBudgets.Any())
                {
                    foreach (Budget incomingBudget in incomingBudgets)
                    {
                        Budget? existingBudget = existingBudgets.FirstOrDefault(b => b.BudgetId == incomingBudget.BudgetId);

                        if (existingBudget is not null)
                        {
                            existingBudget = EntityUtilities.Update(existingBudget, incomingBudget);

                            if (incomingBudget.Users is not null && incomingBudget.Users.Any())
                            {
                                existingBudget.Users = (existingBudget.Users is null) ? new List<User>() : existingBudget.Users.ToList();
                                incomingBudget.Users.ToList().ForEach(u => existingBudget.Users.Add(u));
                            }

                            await _dataStore.Budget.Update(existingBudget);
                        }
                        else
                            await _dataStore.Budget.AddAsync(incomingBudget);
                    }
                    existingUser.Budgets = incomingBudgets;
                }
            }
        }
    }
}

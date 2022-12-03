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
using Web_API.Contracts.Data;
using Web_API.Data;

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
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            IEnumerable<User>? users = await _dataStore.User.GetAllAsync(null, "Budgets");

            if (users is not null)
                return AutoMapper.Map(users, true).ToList();
            else
                return new List<UserDTO>();    
        }

        // GET: api/Users/5
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<UserDTO>> GetUser(Guid id)
        {
            User? user = await _dataStore.User.GetAsync(u => u.UserId == id, false, "Budgets");

            if (user == null)
            {
                return NotFound();
            }

            return AutoMapper.Map(user, true);
        }

        // GET: api/Users/doeman
        [HttpGet("{username:string}")]
        public async Task<ActionResult<UserDTO>> GetUser(string username)
        {
            User? user = await _dataStore.User.GetAsync(u => u.Username == username, false, "Budgets");

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

            User? user = await _dataStore.User.GetAsync(u => u.UserId == id, false, "Budgets");

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
                            await _dataStore.Budget.AddAsync(budget);
                            user.Budgets.Add(budget);
                        }
                    }
                }

                try
                {
                    await _dataStore.User.Update(user);
                }
                catch (DbUpdateConcurrencyException)
                {
                    bool userExists = await UserExists(id);

                    if (userExists)
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

            await _dataStore.User.AddAsync(user);

            return CreatedAtAction("GetUser", new { id = user.UserId }, user);
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
    }
}

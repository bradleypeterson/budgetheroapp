using DebugTools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelsLibrary;
using Web_API.Contracts.Data;
using Web_API.Data;

namespace Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetCategoryGroupsController : ControllerBase
    {
        private readonly IDataStore _dataStore;

        public BudgetCategoryGroupsController(IDataStore dataStore)
        {
            _dataStore = dataStore;
        }

        // GET: api/BudgetCategoryGroups
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BudgetCategoryGroup>>> GetBudgetCategoryGroups()
        {
            IEnumerable<BudgetCategoryGroup>? _categoryGroups = await _dataStore.BudgetCategoryGroup.GetAllAsync(null, "Budgets");
            
            return _categoryGroups.ToList();
        }

        // GET: api/BudgetCategoryGroups/5
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<BudgetCategoryGroup>> GetBudgetCategoryGroup(Guid id)
        {
            BudgetCategoryGroup? _categoryGroup = await _dataStore.BudgetCategoryGroup
                .GetAsync(b => b.BudgetCategoryGroupID == id, false, "Budgets");
            Jsonizer.GimmeDatJson(_categoryGroup);
            if (_categoryGroup == null)
                return NotFound();

            return _categoryGroup;
        }

        // PUT: api/BudgetCategoryGroups/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> PutBudgetCategoryGroup(Guid id, BudgetCategoryGroup _categoryGroup)
        {
            if (id != _categoryGroup.BudgetCategoryGroupID)
                return BadRequest();

            try
            {
                await _dataStore.BudgetCategoryGroup.Update(_categoryGroup);
            }
            catch (DbUpdateConcurrencyException)
            {
                bool categoryGroupExists = await BudgetCategoryGroupExists(id);

                if (!categoryGroupExists)
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // POST: api/BudgetCategoryGroups
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BudgetCategoryGroup>> PostBudgetCategoryGroup(BudgetCategoryGroup categoryGroup)
        {
            bool categoryGroupExists = await BudgetCategoryGroupExists(categoryGroup.BudgetCategoryGroupID);

            if (!categoryGroupExists)
            {
                if (categoryGroup.Budgets is not null)
                {
                    List<Budget> budgets = categoryGroup.Budgets.ToList();
                    categoryGroup.Budgets = null;
                    await _dataStore.BudgetCategoryGroup.AddAsync(categoryGroup);
                    categoryGroup.Budgets = budgets;
                    await _dataStore.BudgetCategoryGroup.Update(categoryGroup);
                }
                else
                    await _dataStore.BudgetCategoryGroup.AddAsync(categoryGroup);

                return CreatedAtAction("GetBudgetCategoryGroup", new { id = categoryGroup.BudgetCategoryGroupID }, categoryGroup);
            }
            else
                return StatusCode(422);
        }

        // DELETE: api/BudgetCategoryGroups/5
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteBudgetCategoryGroup(Guid id)
        {
            BudgetCategoryGroup? _categoryGroup = await _dataStore.BudgetCategoryGroup.GetAsync(b => b.BudgetCategoryGroupID == id);

            if (_categoryGroup == null)
            {
                return NotFound();
            }

            await _dataStore.BudgetCategoryGroup.DeleteAsync(_categoryGroup);

            return NoContent();
        }

        private async Task<bool> BudgetCategoryGroupExists(Guid id)
        {
            BudgetCategoryGroup? _categoryGroup = await _dataStore.BudgetCategoryGroup.GetByIdAsync(id);

            if (_categoryGroup is null)
                return false;
            else
                return true;
        }
    }
}

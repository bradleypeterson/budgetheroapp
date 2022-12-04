using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelsLibrary;
using ModelsLibrary.DTO;
using ModelsLibrary.Utilities;
using Web_API.Contracts.Data;

namespace Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankAccountsController : ControllerBase
    {
        private readonly IDataStore _dataStore;

        public BankAccountsController(IDataStore dataStore)
        {
            _dataStore = dataStore;
        }

        // GET: api/BankAccounts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BankAccountDTO>>> GetBankAccounts()
        {
            IEnumerable<BankAccount>? bankAccounts = await _dataStore.BankAccount.GetAllAsync();
            
            return AutoMapper.Map(bankAccounts).ToList();
        }

        // GET: api/BankAccounts/5
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<BankAccountDTO>> GetBankAccount(Guid id)
        {
            BankAccount? bankAccount = await _dataStore.BankAccount.GetAsync(b => b.BankAccountId == id);

            if (bankAccount == null)
            {
                return NotFound();
            }

            return AutoMapper.Map(bankAccount);
        }

        // PUT: api/BankAccounts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> PutBankAccount(Guid id, BankAccountDTO bankAccountDTO)
        {
            BankAccount bankAccount = AutoMapper.ReverseMap(bankAccountDTO);

            if (id != bankAccount.BankAccountId)
            {
                return BadRequest();
            }

            try
            {
                await _dataStore.BankAccount.Update(bankAccount);
            }
            catch (DbUpdateConcurrencyException)
            {
                bool bankAccountExists = await BankAccountExists(id);

                if (!bankAccountExists)
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

        // POST: api/BankAccounts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BankAccountDTO>> PostBankAccount(BankAccountDTO bankAccountDTO)
        {
            BankAccount bankAccount = AutoMapper.ReverseMap(bankAccountDTO);
            bool bankAccountExists = await BankAccountExists(bankAccount.BankAccountId);

            if (!bankAccountExists)
            {
                await _dataStore.BankAccount.AddAsync(bankAccount);
                return CreatedAtAction("GetBankAccount", new { id = bankAccount.BankAccountId }, bankAccount);
            }
            else
                return StatusCode(422);
        }

        // DELETE: api/BankAccounts/5
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteBankAccount(Guid id)
        {
            BankAccount? bankAccount = await _dataStore.BankAccount.GetAsync(b => b.BankAccountId == id);

            if (bankAccount == null)
            {
                return NotFound();
            }

            await _dataStore.BankAccount.DeleteAsync(bankAccount);

            return NoContent();
        }

        private async Task<bool> BankAccountExists(Guid id)
        {
            BankAccount? bankAccount = await _dataStore.BankAccount.GetByIdAsync(id);
            if (bankAccount is null)
                return false;
            else
                return true;
        }
    }
}

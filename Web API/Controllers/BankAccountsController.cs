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
    public class BankAccountsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BankAccountsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/BankAccounts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BankAccount>>> GetBankAccounts()
        {
            IEnumerable<BankAccount> accounts = await _context.BankAccounts
                .Include(u => u.User)
                .ToListAsync();

            if (accounts is null || !accounts.Any())
                return NoContent();
            else
                return Ok(accounts);
        }

        // GET: api/BankAccounts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BankAccount>> GetBankAccount(Guid id)
        {
            IEnumerable<BankAccount> bankAccounts = await _context.BankAccounts
                .Include(u => u.User)
                .ToListAsync();

            BankAccount? bankAccount = bankAccounts.FirstOrDefault(a => a.BankAccountId == id);

            if (bankAccount == null)
                return NotFound();

            return bankAccount;
        }

        // PUT: api/BankAccounts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBankAccount(Guid id, BankAccount bankAccount)
        {
            if (id != bankAccount.BankAccountId || !BankAccountExists(id))
                return BadRequest();

            User? user = await _context.Users.FindAsync(bankAccount.UserId);

            if (user == null)
                return BadRequest();

            bankAccount.User = user;
            _context.Entry(bankAccount).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BankAccountExists(id))
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
        public async Task<ActionResult<BankAccount>> PostBankAccount(BankAccount bankAccount)
        {
            if (BankAccountExists(bankAccount.BankAccountId))
                return StatusCode(422);

            User? user = await _context.Users.FindAsync(bankAccount.UserId);

            if (user == null)
                return BadRequest();

            bankAccount.User = user;
            _context.BankAccounts.Add(bankAccount);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBankAccount", new { id = bankAccount.BankAccountId }, bankAccount);
        }

        // DELETE: api/BankAccounts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBankAccount(Guid id)
        {
            var bankAccount = await _context.BankAccounts.FindAsync(id);
            if (bankAccount == null)
            {
                return NotFound();
            }

            _context.BankAccounts.Remove(bankAccount);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BankAccountExists(Guid id)
        {
            return _context.BankAccounts.Any(e => e.BankAccountId == id);
        }
    }
}

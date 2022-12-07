using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelsLibrary;
using Web_API.Data;

namespace Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TransactionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Transactions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactions()
        {
            IEnumerable<Transaction> transactions = await _context.Transactions
                .Include(a => a.BankAccount!).ThenInclude(u => u.User)
                .Include(c => c.BudgetCategory).ThenInclude(g => g.BudgetCategoryGroup!)
                .ThenInclude(b => b.Budgets)
                .ToListAsync();

            if (transactions is null || !transactions.Any())
                return NoContent();
            else
                return Ok(transactions);
        }

        // GET: api/Transactions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Transaction>> GetTransaction(Guid id)
        {
            IEnumerable<Transaction> transactions = await _context.Transactions
                .Include(a => a.BankAccount!).ThenInclude(u => u.User)
                .Include(c => c.BudgetCategory).ThenInclude(g => g.BudgetCategoryGroup!)
                .ThenInclude(b => b.Budgets)
                .ToListAsync();

            var transaction = transactions.FirstOrDefault(t => t.TransactionId == id);

            if (transaction == null)
                return NotFound();

            return transaction;
        }

        // PUT: api/Transactions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransaction(Guid id, Transaction transaction)
        {
            if (id != transaction.TransactionId || !TransactionExists(id))
                return BadRequest();

            _context.Entry(transaction).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactionExists(id))
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

        // POST: api/Transactions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Transaction>> PostTransaction(Transaction transaction)
        {
            if (TransactionExists(transaction.TransactionId))
                return StatusCode(422);

            BankAccount? bankAccount = await _context.BankAccounts.FindAsync(transaction.BankAccountId);
            BudgetCategory? category = await _context.BudgetCategories.FindAsync(transaction.BudgetCategoryId);

            if (bankAccount is null || category is null)
                return BadRequest();

            transaction.BankAccount = bankAccount;
            transaction.BudgetCategory = category;

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTransaction", new { id = transaction.TransactionId }, transaction);
        }

        // DELETE: api/Transactions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(Guid id)
        {
            var transaction = await _context.Transactions.FindAsync(id);

            if (transaction is null)
                return NotFound();

            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TransactionExists(Guid id)
        {
            return _context.Transactions.Any(e => e.TransactionId == id);
        }
    }
}

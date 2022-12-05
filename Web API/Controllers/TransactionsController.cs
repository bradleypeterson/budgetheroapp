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
    public class TransactionsController : ControllerBase
    {
        private readonly IDataStore _dataStore;

        public TransactionsController(IDataStore dataStore)
        {
            _dataStore = dataStore;
        }

        // GET: api/Transactions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactions()
        {
            IEnumerable<Transaction> transactions = await _dataStore.Transaction.GetAllAsync();

            return transactions.ToList();
        }

        // GET: api/Transactions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Transaction>> GetTransaction(Guid id)
        {
            Transaction? transaction = await _dataStore.Transaction.GetAsync(t => t.TransactionId == id);

            if (transaction == null) 
                return NotFound();

            return transaction;
        }

        // PUT: api/Transactions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransaction(Guid id, Transaction transaction)
        {
            if (id != transaction.TransactionId)
                return BadRequest();

            try
            {
                await _dataStore.Transaction.Update(transaction);
            }
            catch (DbUpdateConcurrencyException)
            {
                bool transactionExists = await TransactionExists(id);

                if (!transactionExists)
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // POST: api/Transactions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Transaction>> PostTransaction(Transaction transaction)
        {
            bool transactionExists = await TransactionExists(transaction.BankAccountId);

            if (!transactionExists)
            {
                if (transaction.BudgetCategory is null)
                    return BadRequest();

                BudgetCategory? category = transaction.BudgetCategory;

                category = _dataStore.BudgetCategory.Get(c => c.BudgetCategoryID == transaction.BudgetCategoryId, false, "BudgetCategoryGroup");

                if (category is null)
                    return BadRequest();

                transaction.BudgetCategory = category;

                await _dataStore.Transaction.AddAsync(transaction);
                return CreatedAtAction("GetTransaction", new { id = transaction.TransactionId }, transaction);
            }
            else
                return StatusCode(422);
        }

        // DELETE: api/Transactions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(Guid id)
        {
            Transaction? transaction = await _dataStore.Transaction.GetAsync(t => t.TransactionId == id);

            if (transaction == null)
            {
                return NotFound();
            }

            await _dataStore.Transaction.DeleteAsync(transaction);

            return NoContent();
        }

        private async Task<bool> TransactionExists(Guid id)
        {
            Transaction? transaction = await _dataStore.Transaction.GetByIdAsync(id);
            if (transaction is null)
                return false;
            else
                return true;
        }
    }
}

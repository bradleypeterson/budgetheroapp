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
        public async Task<ActionResult<IEnumerable<TransactionDTO>>> GetTransactions()
        {
            IEnumerable<Transaction>? transactions = await _dataStore.Transaction.GetAllAsync();

            return AutoMapper.Map(transactions).ToList();
        }

        // GET: api/Transactions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionDTO>> GetTransaction(Guid id)
        {
            Transaction? transaction = await _dataStore.Transaction.GetAsync(t => t.TransactionId == id);

            if (transaction == null)
            {
                return NotFound();
            }

            return AutoMapper.Map(transaction);
        }

        // PUT: api/Transactions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransaction(Guid id, TransactionDTO transactionDTO)
        {
            Transaction transaction = AutoMapper.ReverseMap(transactionDTO);

            if (id != transaction.TransactionId)
            {
                return BadRequest();
            }

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
        public async Task<ActionResult<Transaction>> PostTransaction(TransactionDTO transactionDTO)
        {
            Transaction transaction = AutoMapper.ReverseMap(transactionDTO);
            bool transactionExists = await TransactionExists(transaction.BankAccountId);

            if (!transactionExists)
            {
                await _dataStore.Transaction.AddAsync(transaction);
                return CreatedAtAction("GetTransaction", new { id = transactionDTO.TransactionId }, transactionDTO);
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

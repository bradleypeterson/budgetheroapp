using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ModelsLibrary
{
    public class Transaction
    {
        public Guid TransactionId { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; }

        [Required]
        public string? TransactionPayee { get; set; }

        public string? TransactionMemo { get; set; }

        [Precision(18,2)]
        public decimal ExpenseAmount { get; set; }

        [Precision(18, 2)]
        public decimal DepositAmount { get; set; }

        [Required]
        public bool IsTransactionPaid { get; set; }

        [Required]
        public bool IsHousehold { get; set; }

        [Required]
        public Guid BankAccountId { get; set; }

        [Required]
        public virtual BankAccount? BankAccount { get; set; }

        [Required]
        public Guid BudgetCategoryId { get; set; }

        [Required]
        public virtual BudgetCategory? BudgetCategory { get; set; }
    }
}
 
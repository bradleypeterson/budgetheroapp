using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLibrary
{
    public class Transaction
    {
        public int TransactionId { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; }

        [Required]
        public string TransactionPayee { get; set; } = null!;

        public string? TransactionMemo { get; set; }

        [Precision(18,2)]
        public decimal ExpenseAmount { get; set; }

        [Precision(18, 2)]
        public decimal DepositAmount { get; set; }

        [Required]
        public bool? IsTransactionPaid { get; set; }

        [Required]
        public bool IsHousehold { get; set; }

        [Required]
        public int BankAccountId { get; set; }

        [Required]
        public BankAccount BankAccount { get; set; } = null!;

        [Required]
        public int BudgetCategoryId { get; set; }

        [Required]
        public BudgetCategory BudgetCategory { get; set; } = null!;
    }
}
 
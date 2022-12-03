using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLibrary.DTO
{
    public class TransactionDTO
    {
        public Guid TransactionId { get; set; }
        public DateTime TransactionDate { get; set; }
        public string? TransactionPayee { get; set; }
        public string? TransactionMemo { get; set; }
        public decimal ExpenseAmount { get; set; }
        public decimal DepositAmount { get; set; }
        public bool IsTransactionPaid { get; set; }
        public bool IsHousehold { get; set; }
        public Guid BankAccountId { get; set; }
        public Guid BudgetCategoryId { get; set; }
    }
}

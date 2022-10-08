using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLibrary
{
    public class Transaction
    {
        public int TransactionId { get; set; }

        public DateTime TransactionDate { get; set; }

        public string TransactionPayee { get; set; }

        public string TransactionMemo { get; set; }

        [Precision(18,2)]
        public decimal ExpenseAmount { get; set; }

        [Precision(18, 2)]
        public decimal DepositAmount { get; set; }

        public bool IsTransactionPaid { get; set; }

        public bool IsHousehold { get; set; }

        public int BankAccountId { get; set; }

        public BankAccount BankAccount { get; set; }

        public int BudgetCategoryId { get; set; }

        public BudgetCategory BudgetCategory { get; set; }
    }
}
 
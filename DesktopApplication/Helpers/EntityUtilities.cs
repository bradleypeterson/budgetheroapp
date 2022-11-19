using ModelsLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopApplication.Helpers
{
    public class EntityUtilities
    {
        public static BankAccount Update(BankAccount existing, BankAccount modified)
        {
            existing.BankAccountId = modified.BankAccountId;
            existing.BankName = modified.BankName;
            existing.AccountType = modified.AccountType;
            existing.Balance = modified.Balance;
            existing.UserId= modified.UserId;
            existing.User = modified.User;

            return existing;
        }

        public static Transaction Update(Transaction existing, Transaction modified)
        {
            existing.TransactionDate = modified.TransactionDate;
            existing.TransactionPayee = modified.TransactionPayee;
            existing.TransactionMemo = modified.TransactionMemo;
            existing.ExpenseAmount = modified.ExpenseAmount;
            existing.DepositAmount = modified.DepositAmount;
            existing.IsTransactionPaid = modified.IsTransactionPaid;
            existing.IsHousehold = modified.IsHousehold;
            existing.BankAccountId = modified.BankAccountId;
            existing.BankAccount = modified.BankAccount;
            existing.BudgetCategoryId = modified.BudgetCategoryId;
            existing.BudgetCategory = modified.BudgetCategory;

            return existing;
        }

        public static BankAccount Duplicate(BankAccount selectedBankAccount)
            => Update(new BankAccount(), selectedBankAccount);

        public static Transaction Duplicate(Transaction selectedTransaction)
            => Update(new Transaction(), selectedTransaction);
    }
}

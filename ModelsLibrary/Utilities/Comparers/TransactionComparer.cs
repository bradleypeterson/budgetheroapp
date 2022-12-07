using System.Diagnostics.CodeAnalysis;

namespace ModelsLibrary.Utilities.Comparers
{
    public class TransactionComparer : IEqualityComparer<Transaction>
    {
        public bool Equals(Transaction? x, Transaction? y)
        {
            if (ReferenceEquals(x, y))
                return true;

            if (x is null || y is null)
                return false;

            return x.TransactionId == y.TransactionId &&
                   x.TransactionDate == y.TransactionDate &&
                   x.TransactionPayee == y.TransactionPayee &&
                   x.TransactionMemo == y.TransactionMemo &&
                   x.ExpenseAmount == y.ExpenseAmount &&
                   x.IsTransactionPaid == y.IsTransactionPaid &&
                   x.IsHousehold == y.IsHousehold &&
                   x.BankAccountId == y.BankAccountId &&
                   x.BudgetCategoryId == y.BudgetCategoryId;
        }

        public int GetHashCode([DisallowNull] Transaction t)
        {
            if (t is null) return 0;

            int hashId = t.TransactionId.GetHashCode();

            int hashDate = t.TransactionDate.GetHashCode();

            int hashPayee = t.TransactionPayee!.GetHashCode();

            int hashMemo = t.TransactionMemo!.GetHashCode();

            int hashExpenseAmount = t.ExpenseAmount.GetHashCode();

            int hashDepositAmount = t.DepositAmount.GetHashCode();

            int hashIsPaid = t.IsTransactionPaid.GetHashCode();

            int hashIsHousehold = t.IsHousehold.GetHashCode();

            int hashAccountId = t.BankAccountId.GetHashCode();

            int hashCategoryId = t.BudgetCategoryId.GetHashCode();

            return hashId ^ hashDate ^ hashPayee ^ hashMemo ^
                   hashExpenseAmount ^ hashDepositAmount ^
                   hashIsPaid ^ hashIsHousehold ^ hashAccountId ^
                   hashCategoryId;
        }
    }
}

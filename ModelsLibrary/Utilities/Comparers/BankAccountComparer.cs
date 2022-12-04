using System.Diagnostics.CodeAnalysis;

namespace ModelsLibrary.Utilities.Comparers
{
    public class BankAccountComparer : IEqualityComparer<BankAccount>
    {
        public bool Equals(BankAccount? x, BankAccount? y)
        {
            if (ReferenceEquals(x, y)) 
                return true;

            if (x is null || y is null)
                return false;

            return x.BankAccountId == y.BankAccountId && 
                   x.BankName == y.BankName &&
                   x.AccountType == y.AccountType &&
                   x.Balance == y.Balance &&
                   x.UserId == x.UserId;
        }

        public int GetHashCode([DisallowNull] BankAccount a)
        {
            if (a is null) return 0;

            int hashBankAccountId = a.BankAccountId.GetHashCode();

            int hashBankName = a.BankName is null ? 0 : a.BankName.GetHashCode();

            int hashAccountType = a.AccountType is null ? 0 : a.AccountType.GetHashCode();

            int hashBalance = a.Balance.GetHashCode();

            int hashUserId = a.UserId.GetHashCode();

            return hashBankAccountId ^ hashBankName ^ 
                   hashAccountType ^ hashBalance ^ hashUserId;
        }
    }
}

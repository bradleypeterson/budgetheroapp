using DesktopApplication.Contracts.Data;
using ModelsLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopApplication.Data
{
    public class AccountRepository : Repository<BankAccount>, IAccountRepository
    {
        public AccountRepository(BudgetAppContext context) 
            : base(context){}

        public async Task SaveWithForeignKeysAsync(IEnumerable<BankAccount> accounts)
        {
            Guid? _userid;
            User? user;

            _userid = accounts.Select(a => a.UserId).FirstOrDefault();

            if (_userid is null)
                throw new ArgumentNullException("No user ID found attached to incoming model.");

            if (_context.Users is null)
                throw new ArgumentNullException("No users have been saved to the local database.");

            user = _context.Users.FirstOrDefault(a => a.UserId == _userid);

            if (user is null) 
                throw new ArgumentNullException(nameof(user));

            accounts.ToList().ForEach(a => a.User = user);

            _context.BankAccounts!.AddRange(accounts);

            int result = await _context.SaveChangesAsync();

            if (result != accounts.Count())
                throw new Exception("Not all incoming models could be saved to the local database.");
        }
    }
}

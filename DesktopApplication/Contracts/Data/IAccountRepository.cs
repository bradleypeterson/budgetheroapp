using ModelsLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopApplication.Contracts.Data
{
    public interface IAccountRepository : IRepository<BankAccount>
    {
        Task SaveWithForeignKeysAsync(IEnumerable<BankAccount> accounts);
    }
}

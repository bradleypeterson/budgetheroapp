using ModelsLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopApplication.Contracts.Data
{
    public interface ITransactionRepository : IRepository<Transaction>
    {
        Task SaveWithForeignKeysAsync(IEnumerable<Transaction> transactions);
    }
}

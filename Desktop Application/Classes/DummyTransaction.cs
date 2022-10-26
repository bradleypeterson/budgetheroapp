using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Desktop_Application.Views.ExpensesView;

namespace Desktop_Application.Classes
{
    public class DummyTransaction
    {
        public DateTime TransactionDate { get; set;}

        public double TransactionAmount { get; set; }


        public static ObservableCollection<DummyTransaction> GetDummyTransactions()
        {
            var transactions = new ObservableCollection<DummyTransaction>();

            transactions.Add(new DummyTransaction() { TransactionDate = DateTime.Now, TransactionAmount = -344444.42});
            transactions.Add(new DummyTransaction() { TransactionDate = DateTime.Now, TransactionAmount = -333.23});
            transactions.Add(new DummyTransaction() { TransactionDate = DateTime.Now, TransactionAmount = -231231.43});

            return transactions;
        }
    }
}

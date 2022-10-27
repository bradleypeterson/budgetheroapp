using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktop_Application.Classes
{
    public class DummyAccount
    {
        public string AccountName { get; set; }
        public double AccountBalance { get; set; }

        public static ObservableCollection<DummyAccount> GetDummyAccounts()
        {
            var accounts = new ObservableCollection<DummyAccount>();

            accounts.Add(new DummyAccount() { AccountName = "Chase checking", AccountBalance = -344444.42 });
            accounts.Add(new DummyAccount() { AccountName = "Mountain America checking", AccountBalance = 2000.32 });
            accounts.Add(new DummyAccount() { AccountName = "American Express savings", AccountBalance = -231231.43 });
            accounts.Add(new DummyAccount() { AccountName = "Bank 1 checking", AccountBalance = -231231.43 });
            accounts.Add(new DummyAccount() { AccountName = "Bank 1 savings", AccountBalance = 0.43 });
            accounts.Add(new DummyAccount() { AccountName = "Bank 2 checkings", AccountBalance = 656.43 });
            accounts.Add(new DummyAccount() { AccountName = "Bank 2 savings", AccountBalance = 1.43 });
            accounts.Add(new DummyAccount() { AccountName = "Bank 3 savings", AccountBalance = 236.43 });


            return accounts;
        }
    }
}

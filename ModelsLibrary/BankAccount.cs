using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLibrary
{
    public class BankAccount
    {
        public int BankAccountId { get; set; }
        
        public string BankName { get; set; }

        public string AccountType { get; set; }

        [Precision(18,2)]
        public decimal Balance { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }
    }
}

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

        [Required]
        public string BankName { get; set; } = null!;

        [Required]
        public string AccountType { get; set; } = null!;

        [Precision(18,2)]
        public decimal Balance { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public User User { get; set; } = null!;

        public override string ToString()
        {
            return $"{BankName} | {Balance:C2}";
        }
    }
}

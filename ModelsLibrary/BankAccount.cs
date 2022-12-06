using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ModelsLibrary
{
    public class BankAccount
    {
        public Guid BankAccountId { get; set; }

        [Required]
        public string? BankName { get; set; }

        [Required]
        public string? AccountType { get; set; }

        [Precision(18,2)]
        public decimal Balance { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public virtual User? User { get; set; }

        public override string ToString()
        {
            return $"{BankName} | {Balance:C2}";
        }
    }
}

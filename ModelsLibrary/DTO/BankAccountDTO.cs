using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLibrary.DTO
{
    public class BankAccountDTO
    {
        public Guid BankAccountId { get; set; }
        public string? BankName { get; set; }
        public string? AccountType { get; set; }
        public decimal Balance { get; set; }
        public Guid UserId { get; set; }
    }
}

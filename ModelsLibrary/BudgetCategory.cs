using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLibrary
{
    public class BudgetCategory
    {
        public int BudgetCategoryID { get; set; }

        [Required]
        public string? CategoryName { get; set; }

        [Precision(18,2)]
        public decimal CategoryAmount { get; set; }

        [Required]
        public int BudgetCategoryGroupID { get; set; }

        [Required]
        public BudgetCategoryGroup? BudgetCategoryGroup { get; set; }
    }
}

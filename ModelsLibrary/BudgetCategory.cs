using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ModelsLibrary
{
    public class BudgetCategory
    {
        public Guid BudgetCategoryID { get; set; }

        [Required]
        public string? CategoryName { get; set; }

        [Precision(18,2)]
        public decimal CategoryAmount { get; set; }

        [Required]
        public Guid BudgetCategoryGroupID { get; set; }

        [Required]
        public virtual BudgetCategoryGroup? BudgetCategoryGroup { get; set; }
    }
}

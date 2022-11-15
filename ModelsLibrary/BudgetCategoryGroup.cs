using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLibrary
{
    public class BudgetCategoryGroup
    {
        public int BudgetCategoryGroupID { get; set; }

        [Required]
        public string CategoryGroupDesc { get; set; } = null!;

        [Required]
        public ICollection<Budget>? Budgets { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLibrary
{
    public class Budget
    {
        public int BudgetId { get; set; } 

        [Required]
        public string BudgetName { get; set; } = null!;
        
        [Required]
        public string BudgetType { get; set; } = null!;

        public ICollection<User>? Users { get; set; }

        public ICollection<BudgetCategoryGroup>? BudgetCategoryGroups { get; set; }
    }
}

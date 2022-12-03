using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLibrary.DTO
{
    public class BudgetCategoryGroupDTO
    {
        public Guid BudgetCategoryGroupID { get; set; }
        public string? CategoryGroupDesc { get; set; }
        public IEnumerable<BudgetDTO>? Budgets { get; set; }
    }
}

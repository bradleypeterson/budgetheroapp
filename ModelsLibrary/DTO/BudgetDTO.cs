using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLibrary.DTO
{
    public class BudgetDTO
    {
        public Guid BudgetId { get; set; }
        public string? BudgetName { get; set; }
        public string? BudgetType { get; set; }
        public IEnumerable<UserDTO>? Users { get; set; }
        public IEnumerable<BudgetCategoryGroupDTO>? BudgetCategoryGroups { get; set; }
    }
}

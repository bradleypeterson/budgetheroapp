using System.ComponentModel.DataAnnotations;

namespace ModelsLibrary
{
    public class Budget
    {
        public Guid BudgetId { get; set; }

        [Required]
        public string? BudgetName { get; set; }
        
        [Required]
        public string? BudgetType { get; set; }

        public ICollection<User>? Users { get; set; }

        public ICollection<BudgetCategoryGroup>? BudgetCategoryGroups { get; set; }
    }
}

using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ModelsLibrary
{
    public class Budget
    {
        public Guid BudgetId { get; set; }

        [Required]
        public string? BudgetName { get; set; }
        
        [Required]
        public string? BudgetType { get; set; }

        public virtual ICollection<User>? Users { get; set; }

        public virtual ICollection<BudgetCategoryGroup>? BudgetCategoryGroups { get; set; }
    }
}

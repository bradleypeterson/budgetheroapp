using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLibrary.DTO
{
    public class BudgetCategoryDTO
    {
        public Guid BudgetCategoryID { get; set; }
        public string? CategoryName { get; set; }
        public decimal CategoryAmount { get; set; }
        public Guid BudgetCategoryGroupID { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLibrary
{
    public class BudgetCategory
    {
        public int BudgetCategoryID { get; set; }
              
        public string CategoryName { get; set; }
        [Precision(18,2)]
        public decimal CategoryAmount { get; set; }
        public int BudgetCategoryGroupID { get; set; }
        public BudgetCategoryGroup BudgetCategoryGroup { get; set; }

    }
}

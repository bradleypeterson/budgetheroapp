using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLibrary
{
    public class Budget
    {
        public int BudgetId { get; set; }

        public string BudgetName { get; set; }

        public string BudgetType { get; set; }

        public List<User> Users { get; set; }
    }
}

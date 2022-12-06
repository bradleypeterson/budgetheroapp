using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLibrary.Utilities.Comparers
{
    public class BudgetCategoryComparer : IEqualityComparer<BudgetCategory>
    {
        public bool Equals(BudgetCategory? x, BudgetCategory? y)
        {
            if (ReferenceEquals(x, y))
                return true;

            if (x is null || y is null)
                return false;

            return x.BudgetCategoryID == y.BudgetCategoryID &&
                   x.CategoryName == y.CategoryName &&
                   x.CategoryAmount == y.CategoryAmount &&
                   x.BudgetCategoryGroupID == y.BudgetCategoryGroupID;
        }

        public int GetHashCode([DisallowNull] BudgetCategory c)
        {
            if (c is null) return 0;

            int hashCategoryId = c.BudgetCategoryID.GetHashCode();

            int hashCategoryName = c.CategoryName!.GetHashCode();

            int hashCategoryAmount = c.CategoryAmount.GetHashCode();

            int hasCategoryGroupId = c.BudgetCategoryGroupID.GetHashCode();

            return hashCategoryId ^ hashCategoryName ^ 
                hashCategoryAmount ^ hasCategoryGroupId;
        }
    }
}

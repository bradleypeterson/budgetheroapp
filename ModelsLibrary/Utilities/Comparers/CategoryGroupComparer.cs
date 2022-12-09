using System.Diagnostics.CodeAnalysis;

namespace ModelsLibrary.Utilities.Comparers
{
    public class CategoryGroupComparer : IEqualityComparer<BudgetCategoryGroup>
    {
        public bool Equals(BudgetCategoryGroup? x, BudgetCategoryGroup? y)
        {
            if (ReferenceEquals(x, y))
                return true;

            if (x is null || y is null)
                return false;

            return x.BudgetCategoryGroupID == y.BudgetCategoryGroupID &&
                   x.CategoryGroupDesc == y.CategoryGroupDesc &&
                   x.Budgets!.SequenceEqual(y.Budgets!);
        }

        public int GetHashCode([DisallowNull] BudgetCategoryGroup g)
        {
            if (g is null) return 0;

            int hashCategoryGroupId = g.BudgetCategoryGroupID.GetHashCode();

            int hashCategoryGroupDesc = g.CategoryGroupDesc!.GetHashCode();

            int hashBudgets = g.Budgets!.GetHashCode();

            return hashCategoryGroupId ^ hashCategoryGroupDesc ^ hashBudgets;
        }
    }
}

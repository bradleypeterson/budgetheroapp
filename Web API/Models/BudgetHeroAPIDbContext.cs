using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ModelsLibrary;

namespace Web_API.Models
{
    public class BudgetHeroAPIDbContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<BankAccount> BankAccounts => Set<BankAccount>();
        public DbSet<Budget> Budgets => Set<Budget>();
        public DbSet<Transaction> Transactions => Set<Transaction>();
        public DbSet<BudgetCategory> BudgetCategories => Set<BudgetCategory>();
        public DbSet<BudgetCategoryGroup> BudgetCategoryGroups => Set<BudgetCategoryGroup>();

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source=budgetheroapi.db");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transaction>()
                .Property(t => t.IsTransactionPaid)
                .HasDefaultValue(true);

            modelBuilder.Entity<Transaction>()
                .Property(t => t.IsHousehold)
                .HasDefaultValue(false);
        }
    }
}

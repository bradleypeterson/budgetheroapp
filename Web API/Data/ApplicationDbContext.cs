using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ModelsLibrary;

namespace Web_API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<Budget> Budgets { get; set; } = null!;
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<BudgetCategory> BudgetCategories { get; set; }
        public DbSet<BudgetCategoryGroup> BudgetCategoryGroups { get; set; }

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

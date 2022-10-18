using Microsoft.EntityFrameworkCore;
using ModelsLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.DBContext
{
    public class TestDbContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<BankAccount> BankAccounts => Set<BankAccount>();
        public DbSet<Budget> Budgets => Set<Budget>();
        public DbSet<Transaction> Transactions => Set<Transaction>();
        public DbSet<BudgetCategory> BudgetCategories => Set<BudgetCategory>();
        public DbSet<BudgetCategoryGroup> BudgetCategoryGroups => Set<BudgetCategoryGroup>();
        public string DbPath { get; }

        public TestDbContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = Path.Join(path, "budgetherotesting.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");

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

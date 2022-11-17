using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ModelsLibrary;

namespace DesktopApplication.Data;
public class BudgetAppContext : DbContext
{
    public DbSet<User>? Users { get; set; }
    public DbSet<BankAccount>? BankAccounts { get; set; }
    public DbSet<Budget>? Budgets { get; set; }
    public DbSet<Transaction>? Transactions { get; set; }
    public DbSet<BudgetCategory>? BudgetCategories { get; set;}
    public DbSet<BudgetCategoryGroup>? BudgetCategoryGroups { get; set; }

    public string DbPath { get; }

    public BudgetAppContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = System.IO.Path.Join(path, "budgetHeroDB3.db");
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

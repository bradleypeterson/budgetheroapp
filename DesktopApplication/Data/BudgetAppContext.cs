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
        DbPath = System.IO.Path.Join(path, "BudgetHeroProduction.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //modelBuilder
        //    .Entity<Budget>()
        //    .HasMany(b => b.BudgetCategoryGroups)
        //    .WithMany(b => b.Budgets)
        //    .UsingEntity(j => j.ToTable("BudgetBudgetCategoryGroups"));

        #region Defaults
        modelBuilder.Entity<BudgetCategory>()
            .Property(t => t.CategoryAmount)
            .HasDefaultValue(0);

        modelBuilder.Entity<Transaction>()
            .Property(t => t.IsTransactionPaid)
            .HasDefaultValue(true);

        modelBuilder.Entity<Transaction>()
            .Property(t => t.IsHousehold)
            .HasDefaultValue(false);
        #endregion

        #region BudgetCategoryGroupSeed
        //modelBuilder.Entity<BudgetCategoryGroup>().HasData(
        //    new BudgetCategoryGroup { BudgetCategoryGroupID = 1, CategoryGroupDesc = "Housing" },
        //    new BudgetCategoryGroup { BudgetCategoryGroupID = 2, CategoryGroupDesc = "Living Expenses" },
        //    new BudgetCategoryGroup { BudgetCategoryGroupID = 3, CategoryGroupDesc = "Entertainment" },
        //    new BudgetCategoryGroup { BudgetCategoryGroupID = 4, CategoryGroupDesc = "Savings Goals" });
        #endregion

        #region BudgetCategorySeed
        //modelBuilder.Entity<BudgetCategory>().HasData(
        //    new BudgetCategory { BudgetCategoryID = 1, CategoryName = "Mortgage", BudgetCategoryGroupID = 1 },
        //    new BudgetCategory { BudgetCategoryID = 2, CategoryName = "HOA Dues", BudgetCategoryGroupID = 1 },
        //    new BudgetCategory { BudgetCategoryID = 3, CategoryName = "Lawn Care", BudgetCategoryGroupID = 1 },
        //    new BudgetCategory { BudgetCategoryID = 4, CategoryName = "Homeowners Insurance", BudgetCategoryGroupID = 1 },
        //    new BudgetCategory { BudgetCategoryID = 5, CategoryName = "Restaurants", BudgetCategoryGroupID = 2 },
        //    new BudgetCategory { BudgetCategoryID = 6, CategoryName = "Groceries", BudgetCategoryGroupID = 2 },
        //    new BudgetCategory { BudgetCategoryID = 7, CategoryName = "Coffee Shops", BudgetCategoryGroupID = 2 },
        //    new BudgetCategory { BudgetCategoryID = 8, CategoryName = "Bars", BudgetCategoryGroupID = 2 },
        //    new BudgetCategory { BudgetCategoryID = 9, CategoryName = "Air Travel", BudgetCategoryGroupID = 3 },
        //    new BudgetCategory { BudgetCategoryID = 10, CategoryName = "Hotel", BudgetCategoryGroupID = 3 },
        //    new BudgetCategory { BudgetCategoryID = 11, CategoryName = "Rental Car", BudgetCategoryGroupID = 3 },
        //    new BudgetCategory { BudgetCategoryID = 12, CategoryName = "Movies", BudgetCategoryGroupID = 3 },
        //    new BudgetCategory { BudgetCategoryID = 13, CategoryName = "Car", BudgetCategoryGroupID = 4 },
        //    new BudgetCategory { BudgetCategoryID = 14, CategoryName = "Vacation", BudgetCategoryGroupID = 4 },
        //    new BudgetCategory { BudgetCategoryID = 15, CategoryName = "Retirement", BudgetCategoryGroupID = 4 },
        //    new BudgetCategory { BudgetCategoryID = 16, CategoryName = "Wedding", BudgetCategoryGroupID = 4 });
        #endregion
    }
}

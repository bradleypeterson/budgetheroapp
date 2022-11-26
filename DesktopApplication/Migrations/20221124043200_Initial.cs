using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DesktopApplication.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BudgetCategoryGroups",
                columns: table => new
                {
                    BudgetCategoryGroupID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CategoryGroupDesc = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetCategoryGroups", x => x.BudgetCategoryGroupID);
                });

            migrationBuilder.CreateTable(
                name: "Budgets",
                columns: table => new
                {
                    BudgetId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BudgetName = table.Column<string>(type: "TEXT", nullable: false),
                    BudgetType = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Budgets", x => x.BudgetId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false),
                    EmailAddress = table.Column<string>(type: "TEXT", nullable: false),
                    PercentageMod = table.Column<double>(type: "REAL", nullable: false),
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false),
                    UserImageLink = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "BudgetCategories",
                columns: table => new
                {
                    BudgetCategoryID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CategoryName = table.Column<string>(type: "TEXT", nullable: false),
                    CategoryAmount = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false, defaultValue: 0m),
                    BudgetCategoryGroupID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetCategories", x => x.BudgetCategoryID);
                    table.ForeignKey(
                        name: "FK_BudgetCategories_BudgetCategoryGroups_BudgetCategoryGroupID",
                        column: x => x.BudgetCategoryGroupID,
                        principalTable: "BudgetCategoryGroups",
                        principalColumn: "BudgetCategoryGroupID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BudgetBudgetCategoryGroup",
                columns: table => new
                {
                    BudgetCategoryGroupsBudgetCategoryGroupID = table.Column<int>(type: "INTEGER", nullable: false),
                    BudgetsBudgetId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetBudgetCategoryGroup", x => new { x.BudgetCategoryGroupsBudgetCategoryGroupID, x.BudgetsBudgetId });
                    table.ForeignKey(
                        name: "FK_BudgetBudgetCategoryGroup_BudgetCategoryGroups_BudgetCategoryGroupsBudgetCategoryGroupID",
                        column: x => x.BudgetCategoryGroupsBudgetCategoryGroupID,
                        principalTable: "BudgetCategoryGroups",
                        principalColumn: "BudgetCategoryGroupID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BudgetBudgetCategoryGroup_Budgets_BudgetsBudgetId",
                        column: x => x.BudgetsBudgetId,
                        principalTable: "Budgets",
                        principalColumn: "BudgetId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BankAccounts",
                columns: table => new
                {
                    BankAccountId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BankName = table.Column<string>(type: "TEXT", nullable: false),
                    AccountType = table.Column<string>(type: "TEXT", nullable: false),
                    Balance = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankAccounts", x => x.BankAccountId);
                    table.ForeignKey(
                        name: "FK_BankAccounts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BudgetUser",
                columns: table => new
                {
                    BudgetsBudgetId = table.Column<int>(type: "INTEGER", nullable: false),
                    UsersUserId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetUser", x => new { x.BudgetsBudgetId, x.UsersUserId });
                    table.ForeignKey(
                        name: "FK_BudgetUser_Budgets_BudgetsBudgetId",
                        column: x => x.BudgetsBudgetId,
                        principalTable: "Budgets",
                        principalColumn: "BudgetId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BudgetUser_Users_UsersUserId",
                        column: x => x.UsersUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    TransactionId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TransactionDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TransactionPayee = table.Column<string>(type: "TEXT", nullable: false),
                    TransactionMemo = table.Column<string>(type: "TEXT", nullable: true),
                    ExpenseAmount = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    DepositAmount = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    IsTransactionPaid = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true),
                    IsHousehold = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    BankAccountId = table.Column<int>(type: "INTEGER", nullable: false),
                    BudgetCategoryId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK_Transactions_BankAccounts_BankAccountId",
                        column: x => x.BankAccountId,
                        principalTable: "BankAccounts",
                        principalColumn: "BankAccountId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transactions_BudgetCategories_BudgetCategoryId",
                        column: x => x.BudgetCategoryId,
                        principalTable: "BudgetCategories",
                        principalColumn: "BudgetCategoryID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_UserId",
                table: "BankAccounts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetBudgetCategoryGroup_BudgetsBudgetId",
                table: "BudgetBudgetCategoryGroup",
                column: "BudgetsBudgetId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetCategories_BudgetCategoryGroupID",
                table: "BudgetCategories",
                column: "BudgetCategoryGroupID");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetUser_UsersUserId",
                table: "BudgetUser",
                column: "UsersUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_BankAccountId",
                table: "Transactions",
                column: "BankAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_BudgetCategoryId",
                table: "Transactions",
                column: "BudgetCategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BudgetBudgetCategoryGroup");

            migrationBuilder.DropTable(
                name: "BudgetUser");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Budgets");

            migrationBuilder.DropTable(
                name: "BankAccounts");

            migrationBuilder.DropTable(
                name: "BudgetCategories");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "BudgetCategoryGroups");
        }
    }
}

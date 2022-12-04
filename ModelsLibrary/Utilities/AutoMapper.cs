using Microsoft.EntityFrameworkCore.Query;
using ModelsLibrary;
using ModelsLibrary.DTO;
using System.Collections.ObjectModel;

namespace ModelsLibrary.Utilities
{
    public class AutoMapper
    {
        public static UserDTO Map(User user, bool includeBudgets)
        {
            UserDTO userDTO = new()
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                EmailAddress = user.EmailAddress,
                PercentageMod = user.PercentageMod,
                Username = user.Username,
                Password = user.Password,
                UserImageLink = user.UserImageLink,
            };

            if (includeBudgets)
                userDTO.Budgets = Map(user.Budgets, false);

            return userDTO;
        }

        public static User ReverseMap(UserDTO userDTO, bool includeBudgets)
        {
            User user = new()
            {
                UserId = userDTO.UserId,
                FirstName = userDTO.FirstName,
                LastName = userDTO.LastName,
                EmailAddress = userDTO.EmailAddress,
                PercentageMod = userDTO.PercentageMod,
                Username = userDTO.Username,
                Password = userDTO.Password,
                UserImageLink = userDTO.UserImageLink,
            };

            if (includeBudgets)
                user.Budgets = ReverseMap(userDTO.Budgets, false);

            return user;
        }

        public static BudgetDTO Map(Budget budget, bool includeUsers)
        {
            BudgetDTO budgetDTO = new BudgetDTO
            {
                BudgetId = budget.BudgetId,
                BudgetName = budget.BudgetName,
                BudgetType = budget.BudgetType,
            };

            if (includeUsers)
                budgetDTO.Users = Map(budget.Users, false);

            return budgetDTO;
        }

        public static Budget ReverseMap(BudgetDTO budgetDTO, bool includeUsers)
        {
            Budget budget = new()
            {
                BudgetId = budgetDTO.BudgetId,
                BudgetName = budgetDTO.BudgetName,
                BudgetType = budgetDTO.BudgetType,
            };

            if (includeUsers)
                budget.Users = ReverseMap(budgetDTO.Users, false);

            return budget;
        }

        public static BudgetCategoryGroupDTO Map(BudgetCategoryGroup categoryGroup, bool includeBudgets)
        {
            BudgetCategoryGroupDTO categoryGroupDTO = new()
            {
                BudgetCategoryGroupID = categoryGroup.BudgetCategoryGroupID,
                CategoryGroupDesc = categoryGroup.CategoryGroupDesc,
            };

            if (includeBudgets)
                categoryGroupDTO.Budgets = Map(categoryGroup.Budgets, true);

            return categoryGroupDTO;
        }

        public static BudgetCategoryGroup ReverseMap(BudgetCategoryGroupDTO categoryGroupDTO, bool includeBudgets)
        {
            BudgetCategoryGroup categoryGroup = new()
            {
                BudgetCategoryGroupID = categoryGroupDTO.BudgetCategoryGroupID,
                CategoryGroupDesc = categoryGroupDTO.CategoryGroupDesc,
            };

            if (includeBudgets)
                categoryGroup.Budgets = ReverseMap(categoryGroupDTO.Budgets, true);

            return categoryGroup;
        }

        public static BudgetCategoryDTO Map(BudgetCategory category)
        {
            return new BudgetCategoryDTO
            {
                BudgetCategoryID = category.BudgetCategoryID,
                CategoryName = category.CategoryName,
                CategoryAmount = category.CategoryAmount,
                BudgetCategoryGroupID = category.BudgetCategoryGroupID
            };
        }

        public static BudgetCategory ReverseMap(BudgetCategoryDTO budgetCategoryDTO)
        {
            return new BudgetCategory
            {
                BudgetCategoryID = budgetCategoryDTO.BudgetCategoryID,
                CategoryName = budgetCategoryDTO.CategoryName,
                CategoryAmount = budgetCategoryDTO.CategoryAmount,
                BudgetCategoryGroupID = budgetCategoryDTO.BudgetCategoryGroupID
            };
        }

        public static BankAccountDTO Map(BankAccount bankAccount)
        {
            return new BankAccountDTO
            {
                BankAccountId = bankAccount.BankAccountId,
                BankName = bankAccount.BankName,
                AccountType = bankAccount.AccountType,
                Balance = bankAccount.Balance,
                UserId = bankAccount.UserId
            };
        }

        public static BankAccount ReverseMap(BankAccountDTO bankAccountDTO)
        {
            return new BankAccount
            {
                BankAccountId = bankAccountDTO.BankAccountId,
                BankName = bankAccountDTO.BankName,
                AccountType = bankAccountDTO.AccountType,
                Balance = bankAccountDTO.Balance,
                UserId = bankAccountDTO.UserId
            };
        }

        public static TransactionDTO Map(Transaction transaction)
        {
            return new TransactionDTO
            {
                TransactionId = transaction.TransactionId,
                TransactionDate = transaction.TransactionDate,
                TransactionPayee = transaction.TransactionPayee,
                TransactionMemo = transaction.TransactionMemo,
                ExpenseAmount = transaction.ExpenseAmount,
                DepositAmount = transaction.DepositAmount,
                IsTransactionPaid = transaction.IsTransactionPaid,
                IsHousehold = transaction.IsHousehold,
                BankAccountId = transaction.BankAccountId,
                BudgetCategoryId = transaction.BudgetCategoryId
            };
        }

        public static Transaction ReverseMap(TransactionDTO transactionDTO)
        {
            return new Transaction
            {
                TransactionId = transactionDTO.TransactionId,
                TransactionDate = transactionDTO.TransactionDate,
                TransactionPayee = transactionDTO.TransactionPayee,
                TransactionMemo = transactionDTO.TransactionMemo,
                ExpenseAmount = transactionDTO.ExpenseAmount,
                DepositAmount = transactionDTO.DepositAmount,
                IsTransactionPaid = transactionDTO.IsTransactionPaid,
                IsHousehold = transactionDTO.IsHousehold,
                BankAccountId = transactionDTO.BankAccountId,
                BudgetCategoryId = transactionDTO.BudgetCategoryId
            };
        }

        public static ICollection<UserDTO> Map(IEnumerable<User>? _users, bool includeBudgets)
        {
            if (_users is not null)
            {
                List<UserDTO> users = new();

                foreach (User? user in _users)
                {
                    if (includeBudgets)
                        users.Add(Map(user, true));
                    else
                        users.Add(Map(user, false));
                }
                    
                return users;
            }
            else
                return new List<UserDTO>();
        }

        public static ICollection<User> ReverseMap(IEnumerable<UserDTO>? _users, bool includeBudgets)
        {
            if (_users is not null)
            {
                List<User> users = new();

                foreach (UserDTO? user in _users)
                {
                    if (includeBudgets)
                        users.Add(ReverseMap(user, true));
                    else
                        users.Add(ReverseMap(user, false));
                }
                    
                return users;
            }
            else
                return new List<User>();
        }

        public static ICollection<BudgetDTO> Map(IEnumerable<Budget>? _budgets, bool includeUsers)
        {
            if (_budgets is not null)
            {
                List<BudgetDTO> budgets = new();

                foreach (Budget? budget in _budgets)
                {
                    if (includeUsers)
                        budgets.Add(Map(budget, true));
                    else
                        budgets.Add(Map(budget, false));
                }
                    
                return budgets;
            }
            else
                return new List<BudgetDTO>();
        }

        private static ICollection<Budget> ReverseMap(IEnumerable<BudgetDTO>? _budgets, bool includeUsers)
        {
            if (_budgets is not null)
            {
                List<Budget> budgets = new();

                foreach (BudgetDTO budget in _budgets)
                {
                    if (includeUsers)
                        budgets.Add(ReverseMap(budget, true));
                    else
                        budgets.Add(ReverseMap(budget, false));
                }
                    
                return budgets;
            }
            else
                return new List<Budget>();
        }

        public static IEnumerable<BankAccountDTO> Map(IEnumerable<BankAccount>? _bankAccounts)
        {
            if (_bankAccounts is not null)
            {
                List<BankAccountDTO> bankAccounts = new();

                foreach (BankAccount? bankAccount in _bankAccounts)
                    bankAccounts.Add(Map(bankAccount));

                return bankAccounts;
            }
            else
                return new List<BankAccountDTO>();
        }

        public static IEnumerable<TransactionDTO> Map(IEnumerable<Transaction>? _transactions)
        {
            if (_transactions is not null)
            {
                List<TransactionDTO> transactionDTOs = new();

                foreach (Transaction? transaction in _transactions)
                    transactionDTOs.Add(Map(transaction));

                return transactionDTOs;
            }
            else
                return new List<TransactionDTO>();
        }
    }
}

using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using ModelsLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Desktop_Application.Views
{

    public sealed partial class ExpensesView : Page
    {

        public ExpensesView()
        {
            this.InitializeComponent();
            ExpenseData.ItemsSource = Expense.getExpenses();
            Account_Grid.ItemsSource = BankAccount.GetAccounts();
        }

        public class Expense
        {
            private DateTime date;
            private string item;
            private double amount;
            private string category;
            private string payee;

            public DateTime Date
            {
                get { return date; }
                set { date = value; }
            }

            public string Item
            {
                get { return item; }
                set { item = value; }
            }

            public double Amount
            {
                get { return amount; }
                set { amount = value; }
            }

            public string Category
            {
                get { return category; }
                set { category = value; }
            }

            public string Payee
            {
                get { return payee; }
                set { payee = value; }
            }

            public static ObservableCollection<Expense> getExpenses()
            {
                var expense = new ObservableCollection<Expense>();

                expense.Add(new Expense() { Date = DateTime.Now, Item = "Toaster", Amount = 5.0, Category = "Household Items", Payee = "Walmart" });
                expense.Add(new Expense() { Date = DateTime.Now, Item = "Extension Cord", Amount = 14.95, Category = "Household Items", Payee = "Walmart" });
                expense.Add(new Expense() { Date = DateTime.Now, Item = "Bread", Amount = 2.68, Category = "Household Items", Payee = "Walmart" });

                return expense;
            }


        }

        private void AddExpense_Btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Submit_Btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EditExpense_Btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteExpense_Btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ExpenseData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid grid = sender as DataGrid;
            Expense expense = (Expense)grid.SelectedItem;

            String content = "Name: " + expense.Item + "\nCost: " + expense.Amount.ToString("C");

            Details_Txt.Text = content;


        }

        public class BankAccount : INotifyPropertyChanged
        {
            private string account;
            private string balance;

            public event PropertyChangedEventHandler PropertyChanged;

            private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

            public string Account
            {
                get { return account; }
                set
                {
                    account = value;
                    NotifyPropertyChanged();
                }
            }

            public string Balance
            {
                get { return balance; }
                set
                {
                    balance = value;
                    NotifyPropertyChanged();
                }
            }

            public static ObservableCollection<BankAccount> GetAccounts()
            {
                var accounts = new ObservableCollection<BankAccount>();

                /* Dummy Data until we hook up the DB */
                accounts.Add(new BankAccount() { Account = "Goldenwest Checking", Balance = "$450.25" });
                accounts.Add(new BankAccount() { Account = "Goldenwest Savings", Balance = "$1289.75" });


                return accounts;
            }
        }
    }
}

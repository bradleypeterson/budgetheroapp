using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ApplicationSettings;
using Expander = CommunityToolkit.WinUI.UI.Controls.Expander;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Desktop_Application.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BudgetView : Page
    {
        private MainWindow mainWindow;
       
        public class CatGroupItem
        {
            public string GroupName { get; set; }
        }

        private List<CatGroupItem> categoryGroupNames = new List<CatGroupItem> ();




        public BudgetView()
        {
            this.InitializeComponent();
            GetMainWindow();
            ResizeWindow();
            Account_Grid.ItemsSource = BankAccount.GetAccounts();

            //call function that will fill the categoryGroups list with category names
            categoryGroupNames.Add(new CatGroupItem { GroupName = "Bills"});
            categoryGroupNames.Add(new CatGroupItem { GroupName = "Food"});
            categoryGroupNames.Add(new CatGroupItem { GroupName = "Entertainment"});
            categoryGroupNames.Add(new CatGroupItem { GroupName = "Loans"});
            categoryGroupNames.Add(new CatGroupItem { GroupName = "Goals"});
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
                //TODO: create expanders in GroupsAccordion Item Control from all the categoryGroupNames in the list

                icGroupsAccordion.ItemsSource = categoryGroupNames;
                
                /*TODO: Data grid for content */
                //DataGrid dataGrid = new DataGrid();

                
                /*This data grid will be inside each expander, we will need to name it based on the category it is in
                We will also need to call another function to fill the datagrid with the correct category items.
                Will probably need to create a datagrid class with a constructor that has all the correct columns and such */
                //expander.Content = dataGrid;

        }

        //TODO: create function that will get all of the category group names from the DB and save it to the list


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

        private void GetMainWindow()
        {
            mainWindow = (Application.Current as App)?.Window as MainWindow;
        }

        private void ResizeWindow()
        {
            mainWindow.ResizeWindowForDashboard();
        }


    }
}

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
using System.Xml;
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
       
        public class CatGroup
        {
            public string GroupName { get; set; }
        }

        public class CatItem
        {
            public double progress;
            public double remaining;
            
            public string ItemName { get; set; }
            public double Budgeted { get; set; }
            public double Allocated { get; set; }

            public CatItem()
            {
                progress = Allocated / Budgeted;
                remaining = Budgeted - Allocated;
            }
        }

        private List<CatGroup> categoryGroupNames = new List<CatGroup> ();

        public BudgetView()
        {
            this.InitializeComponent();
            Account_Grid.ItemsSource = BankAccount.GetAccounts();

            //TODO: call function that will fill the categoryGroups list with category names
            
            //TEMP: data for testing UI --------------------------------
            categoryGroupNames.Add(new CatGroup { GroupName = "Bills"});
            categoryGroupNames.Add(new CatGroup { GroupName = "Food"});
            categoryGroupNames.Add(new CatGroup { GroupName = "Entertainment"});
            categoryGroupNames.Add(new CatGroup { GroupName = "Loans"});
            categoryGroupNames.Add(new CatGroup { GroupName = "Goals"});
            //------------------------------------------------------TEMP
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            /* Fills the GroupAccordion with Category Group Names found in the associated list */
            icGroupsAccordion.ItemsSource = categoryGroupNames;
                
            /* Go through the categoryGroupNames list and for each group name add all appropriate category items to the datagrid within  */    
                 
            /*This data grid will be inside each expander, we will need to name it based on the category it is in
            We will also need to call another function to fill the datagrid with the correct category items.
            Will probably need to create a datagrid class with a constructor that has all the correct columns and such */


        }

        //TODO: create function that will get all of the category group names from the DB and save it to the list
        private void GetCatItemNames()
        {

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

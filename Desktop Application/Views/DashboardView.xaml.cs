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
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ApplicationSettings;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Desktop_Application.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DashboardView : Page
    {
        private MainWindow mainWindow;
        

        public DashboardView()
        {
            this.InitializeComponent();
            GetMainWindow();
            ResizeWindow();
            Account_Grid.ItemsSource = BankAccount.GetAccounts();
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
                set { 
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

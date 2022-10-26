using Desktop_Application.Interfaces;
using Desktop_Application.Models;
using Desktop_Application.Navigation;
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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Desktop_Application.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginView : Page
    {
        private MainWindow mainWindow;


        public LoginView()
        {
            this.InitializeComponent();
            GetMainWindow();
            ResizeWindow();
        }

        private void GetMainWindow()
        {
            mainWindow = (Application.Current as App)?.Window as MainWindow;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            using var database = new BudgetAppContext();
            database.Database.EnsureCreated();
            bool loginValid = true;
            bool userExists = true;
            bool validPassword = true;

            //error textblocks
            tbEmptyError.Visibility = Visibility.Collapsed;
            tbUsernameError.Visibility = Visibility.Collapsed;
            tbPasswordError.Visibility = Visibility.Collapsed;

            //check if user exists with this username
            var user = database.Users.FirstOrDefault(u => u.Username == txtUsername.Text.ToString());
            if (user is null)
            {
                loginValid = false;
                userExists = false;
            }
            else
            {
                if(!PasswordInterface.VerifyPassword(pwbPassword.Password,user.Password))
                {
                    //password is invalid for this user
                    loginValid = false;
                    validPassword = false;
                }
            }

            if(txtUsername.Text == "" || pwbPassword.Password == "")
            {
                if (tbEmptyError.Visibility == Visibility.Collapsed)
                {
                    tbEmptyError.Visibility = Visibility.Visible;
                }
            }

            if (!userExists)
            {
                if (tbUsernameError.Visibility == Visibility.Collapsed)
                {
                    tbUsernameError.Visibility = Visibility.Visible;
                }
            }

            if (!validPassword)
            {
                if (tbPasswordError.Visibility == Visibility.Collapsed)
                {
                    tbPasswordError.Visibility = Visibility.Visible;
                }
            }


            if (loginValid)
            {
                this.Frame.Navigate(typeof(NavigationRootView));
            }
        }

        private void ResizeWindow()
        {
            mainWindow.ResizeWindowForLogin();
        }

        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(RegistrationView));
        }

        
    }
}


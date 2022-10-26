using Desktop_Application.Interfaces;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using EmailValidation;
using Desktop_Application.Models;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata.Internal;
using ModelsLibrary;
using System.Diagnostics;
using Desktop_Application.Navigation;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Desktop_Application.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RegistrationView : Page
    {
        private MainWindow mainWindow;

        public RegistrationView()
        {
            this.InitializeComponent();
            GetMainWindow();
            ResizeWindow();
            tbBlankFieldsError.Visibility = Visibility.Collapsed;
            tbUsernameError.Visibility = Visibility.Collapsed;
            tbEmailError.Visibility = Visibility.Collapsed;
            tbPasswordError.Visibility = Visibility.Collapsed;
        }
        private void GetMainWindow()
        {
            mainWindow = (Application.Current as App)?.Window as MainWindow;
        }

        private void ResizeWindow()
        {
            mainWindow.ResizeWindowForRegistration();
        }

        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            using var database = new BudgetAppContext();
            database.Database.EnsureCreated();
            bool registrationValid = true;
            bool emptyInputs = false;
            bool unmatchingPasswords = false;
            bool invalidEmail = false;
            bool usernameTaken = false;

            //error textblocks
            tbBlankFieldsError.Visibility = Visibility.Collapsed;
            tbUsernameError.Visibility = Visibility.Collapsed;
            tbEmailError.Visibility = Visibility.Collapsed;
            tbPasswordError.Visibility = Visibility.Collapsed;


            if(txtFirstname.Text == "" || 
                txtLastname.Text == "" || 
                txtUsername.Text == "" ||
                txtEmail.Text == "" ||
                pwbPassword.Password == "" ||
                pwbConfirmPassword.Password == "")
            {
                registrationValid = false;
                emptyInputs = true;
            }

            if (database.Users.FirstOrDefault(u => u.Username == txtUsername.Text) is not null) 
            {
                //username already exists
                registrationValid = false;
                usernameTaken = true;
            }


            if (!EmailValidator.Validate(txtEmail.Text) && txtEmail.Text != "")
            {
                registrationValid = false;
                invalidEmail = true;
                
            }
            
            if(pwbPassword.Password != pwbConfirmPassword.Password)
            {
                registrationValid = false;
                unmatchingPasswords = true;
            }


            if (emptyInputs)
            {
                if (tbBlankFieldsError.Visibility == Visibility.Collapsed)
                {
                    tbBlankFieldsError.Visibility = Visibility.Visible;
                }
            }

            if (usernameTaken)
            {
                if (tbUsernameError.Visibility == Visibility.Collapsed)
                {
                    tbUsernameError.Visibility = Visibility.Visible;
                }
            }

            if (invalidEmail)
            {
                if (tbEmailError.Visibility == Visibility.Collapsed)
                {
                    tbEmailError.Visibility = Visibility.Visible;
                }
            }

            if (unmatchingPasswords)
            {
                if (tbPasswordError.Visibility == Visibility.Collapsed)
                {
                    tbPasswordError.Visibility = Visibility.Visible;
                }
            }

            if (registrationValid)
            {
                //Create a new User object
                var user = new User()
                {
                    FirstName = txtFirstname.Text,
                    LastName = txtLastname.Text,
                    EmailAddress = txtEmail.Text,
                    PercentageMod = null,
                    Username = txtUsername.Text,
                    Password = PasswordInterface.HashPassword(pwbPassword.Password),
                    UserImageLink = null
                };

                database.Users.Add(user);
                database.SaveChanges();
                this.Frame.Navigate(typeof(NavigationRootView));
            }
            
        }
    }
}

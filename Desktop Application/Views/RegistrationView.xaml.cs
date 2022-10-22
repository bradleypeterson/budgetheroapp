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


            //Check all input fields for invalid input
            if(txtFirstname.Text == "")
            {
                registrationValid = false;
            }
            else if(txtLastname.Text == "")
            {
                registrationValid = false;
            }
            else if (database.Users.FirstOrDefault(u => u.Username == txtUsername.Text) is not null) 
            {
                //username already exists
                registrationValid = false;
            }
            else if (!EmailValidator.Validate(txtEmail.Text))
            {
                registrationValid = false;
                
            }else if(pwbPassword.ToString() != pwbConfirmPassword.ToString())
            {
                registrationValid = false;
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
                    Password = PasswordInterface.HashPassword(pwbPassword.ToString()),
                    UserImageLink = null
                };

                database.Users.Add(user);
                database.SaveChanges();
                var usertest = database.Users.FirstOrDefault(u => u.Username == txtFirstname.Text.ToString());
                Debug.WriteLine("!!!!!!!!!!" + usertest.Password);
                this.Frame.Navigate(typeof(DashboardView));
            }
            
        }
    }
}

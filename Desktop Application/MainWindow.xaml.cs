using Desktop_Application.Models;
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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Desktop_Application
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            //CreateTest();
            ReadTest();
            
        }

        private void myButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void CreateTest()
        {
            using var db = new BudgetAppContext();

            // Create
            db.Add(new User { FirstName = "Jane", LastName = "Doe", EmailAddress = "jdoe@example.com", PercentageMod = null, Username = "doewoman", Password = "123456", UserImageLink = null });
            db.SaveChanges();
        }

        private void ReadTest()
        {
            using var db = new BudgetAppContext();

            var users = db.Users;

            foreach (User user in users)
            {
                Debug.WriteLine("Username: " + user.Username);
            }
        }
    }
}

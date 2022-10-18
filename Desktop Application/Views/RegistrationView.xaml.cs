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
            this.Frame.Navigate(typeof(LoginView));
        }
    }
}

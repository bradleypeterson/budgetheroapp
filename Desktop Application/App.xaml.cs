using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Desktop_Application.Navigation;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Desktop_Application
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        private Window m_window;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            m_window = new MainWindow();

            InitializeAppNavigation(args);
            m_window.Activate();
        }

        private void InitializeAppNavigation(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            //// Create a Frame to act as the navigation context
            //Frame rootFrame = new();

            //// Handle the failed navigation events
            //rootFrame.NavigationFailed += OnNavigationFailed;

            //// Place the frame in current window
            //m_window.Content = rootFrame;

            //// Navigate to the root navigation page
            //rootFrame.Navigate(typeof(NavigationRootView), args.Arguments);
        }

        private void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            Console.WriteLine("App.xaml.cs: OnNavigationFailed | Navigation failed...");
        }
    }
}

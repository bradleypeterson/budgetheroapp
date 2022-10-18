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
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using System.Windows;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Desktop_Application
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        private readonly int dashboardWindowWidth = 1920;
        private readonly int dashboardWindowHeight = 1080;
        private readonly int loginWindowWidth = 450;
        private readonly int loginWindowHeight = 630;
        private readonly int registrationWindowWidth = 450;
        private readonly int registrationWindowHeight = 800;
        
        public MainWindow(string title)
        {
            this.InitializeComponent();
            this.Title = title;
        }

        public void ResizeWindowForDashboard()
        {
            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
            ResizeDisplayToMonitorDimensions(hWnd);
            CenterWindow(hWnd);
        }

        public void ResizeWindowForLogin()
        {
            ResizeWindow(loginWindowWidth, loginWindowHeight);
        }

        public void ResizeWindowForRegistration()
        {
            ResizeWindow(registrationWindowWidth, registrationWindowHeight);
        }
        
        private void ResizeWindow(int customWidth, int customHeight)
        {
            IntPtr hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
            var windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hWnd);
            var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(windowId);

            appWindow.Resize(new Windows.Graphics.SizeInt32 { Width = customWidth, Height = customHeight });
        }

        private void CenterWindow(IntPtr hWnd)
        {
            Microsoft.UI.WindowId windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hWnd);
            Microsoft.UI.Windowing.AppWindow appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(windowId);
            if (appWindow is not null)
            {
                Microsoft.UI.Windowing.DisplayArea displayArea = Microsoft.UI.Windowing.DisplayArea.GetFromWindowId(windowId, Microsoft.UI.Windowing.DisplayAreaFallback.Nearest);
                if (displayArea is not null)
                {
                    var CenteredPosition = appWindow.Position;
                    CenteredPosition.X = ((displayArea.WorkArea.Width - appWindow.Size.Width) / 2);
                    CenteredPosition.Y = ((displayArea.WorkArea.Height - appWindow.Size.Height) / 2);
                    appWindow.Move(CenteredPosition);
                }
            }
        }

        private void ResizeDisplayToMonitorDimensions(IntPtr hWnd)
        {
            Microsoft.UI.WindowId windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hWnd);
            Microsoft.UI.Windowing.AppWindow appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(windowId);

            if (appWindow is not null)
            {
                Microsoft.UI.Windowing.DisplayArea displayArea = Microsoft.UI.Windowing.DisplayArea.GetFromWindowId(windowId, Microsoft.UI.Windowing.DisplayAreaFallback.Nearest);
                if (displayArea is not null)
                {
                    ResizeWindow(displayArea.WorkArea.Width, displayArea.WorkArea.Height);
                    PInvoke.User32.ShowWindow(hWnd, PInvoke.User32.WindowShowStyle.SW_MAXIMIZE);
                }
            }
        }

    }
}

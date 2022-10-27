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
using Desktop_Application.Services;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Desktop_Application
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {

        public IntPtr hWnd;
        public Microsoft.UI.Windowing.AppWindow appWindow;
        public Microsoft.UI.WindowId windowId;
        public bool isFirstLoad = false;
        private const double PERCENT_HEIGHT_LOGIN = .58;
        private const double PERCENT_WIDTH_LOGIN_AND_REG = .23;
        private const double PERCENT_HEIGHT_REG = .70;

        public MainWindow(string title)
        {
            hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
            windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hWnd);
            appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(windowId);
            this.InitializeComponent();
            this.Title = title; 

            // Call the API (replace TestApi with APIServiceManager method)
            Task.Run(async() => await TestApi());
        }

        public void ResetFirstLoad()
        {
            isFirstLoad = false;
        }

        public async Task TestApi()
        {
            List<User> users = await APIServiceManager.GetAsync<List<User>>($"/api/users");
            foreach (User user in users)
            {
                Debug.WriteLine(user.FirstName);
            }
        }

        public void ResizeWindowForDashboard()
        {
            if (isFirstLoad == true)
            {
                return;
            }
            else
            {

                if (appWindow is not null)
                {
                    Microsoft.UI.Windowing.DisplayArea displayArea = Microsoft.UI.Windowing.DisplayArea.GetFromWindowId(windowId, Microsoft.UI.Windowing.DisplayAreaFallback.Nearest);
                    if (displayArea is not null)
                    {
                        ResizeWindow(displayArea.WorkArea.Width, displayArea.WorkArea.Height);
                        PInvoke.User32.ShowWindow(hWnd, PInvoke.User32.WindowShowStyle.SW_MAXIMIZE);
                    }
                }

                isFirstLoad = true;
            }
        }

        public void ResizeWindowForLogin()
        {
            if (appWindow is not null)
            {
                Microsoft.UI.Windowing.DisplayArea displayArea = Microsoft.UI.Windowing.DisplayArea.GetFromWindowId(windowId, Microsoft.UI.Windowing.DisplayAreaFallback.Nearest);
                if (displayArea is not null)
                {
                    ResizeWindow((int)(displayArea.WorkArea.Width * PERCENT_WIDTH_LOGIN_AND_REG), (int)(displayArea.WorkArea.Height * PERCENT_HEIGHT_LOGIN));
                }
            }
        }

        public void ResizeWindowForRegistration()
        {
            if (appWindow is not null)
            {
                Microsoft.UI.Windowing.DisplayArea displayArea = Microsoft.UI.Windowing.DisplayArea.GetFromWindowId(windowId, Microsoft.UI.Windowing.DisplayAreaFallback.Nearest);
                if (displayArea is not null)
                {
                    ResizeWindow((int)(displayArea.WorkArea.Width * PERCENT_WIDTH_LOGIN_AND_REG), (int)(displayArea.WorkArea.Height * PERCENT_HEIGHT_REG));
                }
            }
        }
        
        private void ResizeWindow(int customWidth, int customHeight)
        {
            appWindow.Resize(new Windows.Graphics.SizeInt32 { Width = customWidth, Height = customHeight });
            CenterWindow(hWnd);
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
    }
}

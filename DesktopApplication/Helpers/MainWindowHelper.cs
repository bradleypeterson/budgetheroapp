using DesktopApplication.ViewModels;
using DesktopApplication.Views;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.Security.Cryptography.Core;

namespace DesktopApplication.Helpers;
public class MainWindowHelper
{
    private static bool isFirstTimeLoad = true;

    public static XamlRoot GetXamlRoot()
    {
        return App.MainWindow.Content.XamlRoot;
    }

    public static void ResizeWindow(Page pageType)
    {
        const double PERCENT_HEIGHT_LOGIN = .65;
        const double PERCENT_WIDTH_LOGIN_AND_REG = .23;
        const double PERCENT_HEIGHT_REG = .80;
        const double DEFAULT_HEIGHT = .75;
        const double DEFAULT_WIDTH = .75;
        
        double width;
        double height;

        switch (pageType)
        {
            case LoginPage:
                width = GetWindowWidth(PERCENT_WIDTH_LOGIN_AND_REG);
                height = GetWindowHeight(PERCENT_HEIGHT_LOGIN);
                break;
            case RegistrationPage:
                width = GetWindowWidth(PERCENT_WIDTH_LOGIN_AND_REG);
                height = GetWindowHeight(PERCENT_HEIGHT_REG);
                break;
            default:
                width = GetWindowWidth(DEFAULT_WIDTH);
                height = GetWindowHeight(DEFAULT_HEIGHT);
                break;
        }

        if (isFirstTimeLoad)
        {
            App.MainWindow.SetWindowSize(width, height);
            App.MainWindow.CenterOnScreen();
            
        }

    }

    public static void MaxWinFirstTimeLoad()
    {
        if (isFirstTimeLoad)
        {
            App.MainWindow.Maximize();
            isFirstTimeLoad = false;
        }
    }

    public static void ResetWindow()
    {
        App.MainWindow.Restore();
        isFirstTimeLoad = true;
    }

    private static double GetWindowHeight(double heightPercentage)
    {
        var displayArea = GetDisplayArea();

        return displayArea.WorkArea.Height * heightPercentage;
    }

    private static double GetWindowWidth(double widthPercentage)
    {
        var displayArea = GetDisplayArea();

        return displayArea.WorkArea.Width * widthPercentage;
    }

    private static DisplayArea GetDisplayArea()
    {
        var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);
        var windowId = Win32Interop.GetWindowIdFromWindow(hwnd);

        return DisplayArea.GetFromWindowId(windowId, DisplayAreaFallback.Nearest);
    }
}

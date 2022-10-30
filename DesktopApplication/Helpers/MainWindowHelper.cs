using DesktopApplication.Views;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml.Controls;

namespace DesktopApplication.Helpers;
public class MainWindowHelper
{
    public static void ResizeWindow(Page pageType)
    {
        const double PERCENT_HEIGHT_LOGIN = .58;
        const double PERCENT_WIDTH_LOGIN_AND_REG = .23;
        const double PERCENT_HEIGHT_REG = .70;
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
                width = 800;
                height = 600;
                break;
        }

        App.MainWindow.SetWindowSize(width, height);
        App.MainWindow.CenterOnScreen();
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

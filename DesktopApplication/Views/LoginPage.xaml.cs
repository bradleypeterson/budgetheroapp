using DesktopApplication.Contracts.Services;
using DesktopApplication.Helpers;
using DesktopApplication.Services;
using DesktopApplication.ViewModels;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
namespace DesktopApplication.Views;

public sealed partial class LoginPage : Page
{
    public LoginViewModel ViewModel
    {
        get;
    }

    public LoginPage()
    {
        ViewModel = App.GetService<LoginViewModel>();
        NavigationViewService.HideNavigationViewPane();
        InitializeComponent();
        ViewModel.OnUserNotFound += showInvalidUsernameOrPassword;
        ViewModel.OnValidLogin += removeLoginError;
    }

    private void removeLoginError(object? sender, EventArgs e)
    {
        tbUsernameOrPasswordInvalid.Visibility = Visibility.Collapsed;
    }

    private void showInvalidUsernameOrPassword(object? sender, EventArgs e)
    {
        tbUsernameOrPasswordInvalid.Visibility = Visibility.Visible;
    }

    private void Page_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        MainWindowHelper.ResizeWindow(this);
    }

    private void Page_Unloaded(object sender, RoutedEventArgs e)
    {
        ViewModel.OnUserNotFound -= showInvalidUsernameOrPassword;
        ViewModel.OnValidLogin -= removeLoginError;
    }
}

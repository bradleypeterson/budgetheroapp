using DesktopApplication.Helpers;
using DesktopApplication.Services;
using DesktopApplication.ViewModels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.UI.Core;

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
        MainWindowHelper.ResetWindow();
    }

    private void removeLoginError(object? sender, EventArgs e)
    {
        tbUsernameOrPasswordInvalid.Visibility = Visibility.Collapsed;
    }

    private void showInvalidUsernameOrPassword(object? sender, EventArgs e)
    {
        tbUsernameOrPasswordInvalid.Visibility = Visibility.Visible;
        txtUsername.Text = "";
        pwbPassword.Password = "";
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

    private void OnKeyDown(object sender, Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e)
    {
        if(e.Key == Windows.System.VirtualKey.Enter)
        {
            ViewModel.LoginCommand.Execute(e.Key);
        }
        
    }
}

using DesktopApplication.Helpers;
using DesktopApplication.ViewModels;

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
namespace DesktopApplication.Views;

public sealed partial class RegistrationPage : Page
{
    public RegistrationViewModel ViewModel
    {
        get;
    }

    public RegistrationPage()
    {
        ViewModel = App.GetService<RegistrationViewModel>();
        InitializeComponent();
        ViewModel.OnUsernameTaken += showUsernameTakenError;
        ViewModel.OnUsernameNotTaken += removeUsernameTakenError;
        ViewModel.OnInvalidEmail += showEmailInvalidError;
        ViewModel.OnValidEmail += removeEmailInvalidError;
        ViewModel.OnMismatchingPasswords += showMismatchingPasswordsError;
        ViewModel.OnMatchingPasswords += removeMismatchingPasswordsError;
    }

    private void removeMismatchingPasswordsError(object? sender, EventArgs e)
    {
        tbPasswordsMismatchedError.Visibility = Visibility.Collapsed;
    }

    private void showMismatchingPasswordsError(object? sender, EventArgs e)
    {
        tbPasswordsMismatchedError.Visibility = Visibility.Visible;
        pwbPassword.Password = "";
        pwdConfirmPassword.Password = "";
    }

    private void removeEmailInvalidError(object? sender, EventArgs e)
    {
        tbEmailInvalidError.Visibility = Visibility.Collapsed;
    }

    private void showEmailInvalidError(object? sender, EventArgs e)
    {
        tbEmailInvalidError.Visibility= Visibility.Visible;
        txtEmail.Text = "";
    }

    private void removeUsernameTakenError(object? sender, EventArgs e)
    {
        tbUsernameTakenError.Visibility = Visibility.Collapsed;
    }

    private void showUsernameTakenError(object? sender, EventArgs e)
    {
        tbUsernameTakenError.Visibility = Visibility.Visible;
        txtUsername.Text = "";
    }

    private void Page_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        MainWindowHelper.ResizeWindow(this);
    }

    private void Page_Unloaded(object sender, RoutedEventArgs e)
    {
        ViewModel.OnUsernameTaken -= showUsernameTakenError;
        ViewModel.OnUsernameNotTaken -= removeUsernameTakenError;
        ViewModel.OnInvalidEmail -= showEmailInvalidError;
        ViewModel.OnValidEmail -= removeEmailInvalidError;
        ViewModel.OnMismatchingPasswords -= showMismatchingPasswordsError;
        ViewModel.OnMatchingPasswords -= removeMismatchingPasswordsError;
    }
}

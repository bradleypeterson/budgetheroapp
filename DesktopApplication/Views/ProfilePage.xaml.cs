using DesktopApplication.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace DesktopApplication.Views;

public sealed partial class ProfilePage : Page
{
    public ProfileViewModel ViewModel
    {
        get;
    }

    public ProfilePage()
    {
        ViewModel = App.GetService<ProfileViewModel>();
        InitializeComponent();
    }
}

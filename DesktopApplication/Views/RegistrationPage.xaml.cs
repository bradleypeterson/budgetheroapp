using DesktopApplication.ViewModels;

using Microsoft.UI.Xaml.Controls;

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
    }
}

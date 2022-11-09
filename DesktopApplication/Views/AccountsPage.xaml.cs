using DesktopApplication.Helpers;
using DesktopApplication.Services;
using DesktopApplication.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace DesktopApplication.Views;

public sealed partial class AccountsPage : Page
{
    public AccountsViewModel ViewModel { get; }

    public AccountsPage()
    {
        ViewModel = App.GetService<AccountsViewModel>();
        NavigationViewService.ShowNavigationViewPane();
        InitializeComponent();
    }

    private async void Page_Loaded(object sender, RoutedEventArgs e)
    {
        MainWindowHelper.ResizeWindow(this);
        MainWindowHelper.MaxWinFirstTimeLoad();
        await ViewModel.LoadAsync();
    }
}

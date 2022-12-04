using DesktopApplication.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace DesktopApplication.Views;

public sealed partial class HouseholdPage : Page
{
    public HouseholdViewModel ViewModel { get; }

    public HouseholdPage()
    {
        ViewModel = App.GetService<HouseholdViewModel>();
        InitializeComponent();
    }

    private async void Page_Loaded(object sender, RoutedEventArgs e)
    {
        await ViewModel.LoadAsync();
    }
}

using DesktopApplication.Contracts.Data;
using DesktopApplication.Contracts.Services;
using DesktopApplication.Models;
using DesktopApplication.Services;
using DesktopApplication.ViewModels;
using Microsoft.UI.Xaml.Controls;
using Syncfusion.UI.Xaml.Charts;
using System.Collections.ObjectModel;

namespace DesktopApplication.Views;

public sealed partial class ReportsPage : Page
{

    public ReportsViewModel ViewModel { get; }

    public ReportsPage()
    {
        ViewModel = App.GetService<ReportsViewModel>();
        InitializeComponent();

    }

    //Once the page is loaded this function will call the LoadAsync which will get the data
    private async void Page_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        await ViewModel.LoadAsync();
    }


}

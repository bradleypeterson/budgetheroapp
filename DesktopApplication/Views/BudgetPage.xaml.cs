using System.Diagnostics;
using CommunityToolkit.WinUI.UI.Controls;
using DesktopApplication.ViewModels;
using DesktopApplication.ViewModels.Models;
using Microsoft.UI.Xaml.Controls;
using ModelsLibrary;

namespace DesktopApplication.Views;

public sealed partial class BudgetPage : Page
{
    private object? storedSender;
    
    public BudgetViewModel ViewModel { get; }

    public BudgetPage()
    {
        ViewModel = App.GetService<BudgetViewModel>();
        InitializeComponent();
    }

    private async void Page_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        await ViewModel.LoadAsync();
    }

    private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if(storedSender is null)
        {
            storedSender = sender;
        }
        else if(sender != storedSender)
        {
            DataGrid? grid = storedSender as DataGrid;
            
            if (grid != null)
            {
                grid.SelectedIndex = -1;
            }

            storedSender = sender;
        }
    }
}

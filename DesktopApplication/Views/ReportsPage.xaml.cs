using DesktopApplication.ViewModels;

using Microsoft.UI.Xaml.Controls;
using Syncfusion.UI.Xaml.Charts;

namespace DesktopApplication.Views;

public sealed partial class ReportsPage : Page
{
    public ReportsViewModel ViewModel
    {
        get;
    }

    public ReportsPage()
    {
        ViewModel = App.GetService<ReportsViewModel>();
        InitializeComponent();
    }
}

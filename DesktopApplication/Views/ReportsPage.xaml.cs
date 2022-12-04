using DesktopApplication.Contracts.Data;
using DesktopApplication.Contracts.Services;
using DesktopApplication.Models;
using DesktopApplication.Services;
using DesktopApplication.ViewModels;
using Microsoft.UI.Xaml.Controls;
using ModelsLibrary;
using Syncfusion.UI.Xaml.Charts;
using System.Collections.ObjectModel;
using static DesktopApplication.ViewModels.ReportsViewModel;

namespace DesktopApplication.Views;

public sealed partial class ReportsPage : Page
{

    public ReportsViewModel ViewModel { get; }

    private SfCartesianChart categoryLineChart = new SfCartesianChart()
    {
        Header = "Category Breakown Over the Past Year"
    };

    private SfCartesianChart categoryBarGraph = new SfCartesianChart()
    {
        Header = "Total Expenses Over the Past Year"
    };

    public ReportsPage()
    {
        ViewModel = App.GetService<ReportsViewModel>();
        InitializeComponent();
        //set the context for the graph data
        ContentArea.DataContext = ViewModel;
    }

    //Once the page is loaded this function will call the LoadAsync which will get the data
    private async void Page_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        await ViewModel.LoadAsync();
        createCategoryLineGraph();
        createCategoryBarGraph();
    }

    private void createCategoryLineGraph()
    {
        //Create the x-axis
        CategoryAxis xAxis = new CategoryAxis();
        xAxis.Header = "Month";
        categoryLineChart.XAxes.Add(xAxis);

        //create the y-axis
        NumericalAxis yAxis = new NumericalAxis();
        yAxis.Header = "Cost";
        categoryLineChart.YAxes.Add(yAxis);

        //Legend
        ChartLegend legend = new ChartLegend()
        {
            Placement = LegendPlacement.Right,
            CheckBoxVisibility = Visibility
        };
        categoryLineChart.Legend = legend;
        

        //gather data by category
        List<string> categories = new List<string>();

        for (int i = 0; i < ViewModel.Transactions.Count; i++)
        {

            string name = ViewModel.Transactions.ElementAt(i).CategoryName;
            if (!categories.Contains(name) && ViewModel.Transactions.ElementAt(i).DepositAmount.Equals(""))
            {
                LineSeries temp = new LineSeries()
                {
                    ItemsSource = ViewModel.gatherData(name),
                    XBindingPath = "Month",
                    YBindingPath = "TotalCost",
                    Label = name,
                    EnableTooltip = true,
                };
                categoryLineChart.Series.Add(temp);
                categories.Add(name);
            }
            
            
        }

        //Add graph to the xaml
        catLineGraphArea.Children.Add(categoryLineChart);
    }

    private void createCategoryBarGraph()
    {
        //Create the x-axis
        CategoryAxis xAxis = new CategoryAxis();
        xAxis.Header = "Month";
        categoryBarGraph.XAxes.Add(xAxis);

        //create the y-axis
        NumericalAxis yAxis = new NumericalAxis();
        yAxis.Header = "Cost";
        categoryBarGraph.YAxes.Add(yAxis);

        //gather data by category
        ColumnSeries temp = new ColumnSeries()
        {
            ItemsSource = ViewModel.gatherData(),
            XBindingPath = "Month",
            YBindingPath = "TotalCost",
            EnableTooltip = true,
            SegmentSpacing = 0.5,
        };
                
        categoryBarGraph.Series.Add(temp);
        



        //Add graph to the xaml
        catBarGraphArea.Children.Add(categoryBarGraph);
    }
    
}

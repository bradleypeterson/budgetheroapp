using System.Diagnostics;
using DesktopApplication.Contracts.Data;
using DesktopApplication.Data;
using DesktopApplication.Helpers;
using ModelsLibrary;

namespace DesktopApplication;

public sealed partial class MainWindow : WindowEx
{
    private IDataStore _datastore;

    public MainWindow()
    {
        InitializeComponent();

        AppWindow.SetIcon(Path.Combine(AppContext.BaseDirectory, "Assets/WindowIcon.ico"));
        Content = null;
        Title = "AppDisplayName".GetLocalized();

        _datastore = App.GetService<IDataStore>();

        //Task.Run(TestAdd);
    }

    /* test method for adding budgets */
    public async void TestAdd()
    {
        var newUser = _datastore.User.Get(u => u.UserId == 1, false, "Budgets");

        //var newBudget = new List<Budget>();
        var newBudget = newUser.Budgets;

        Budget budget = new Budget
        {
            BudgetName = "mine",
            BudgetType = "personal",
        };

        newBudget.Add(budget);

        newUser.Budgets = newBudget;

        await _datastore.User.Update(newUser);
    }
}

using CommunityToolkit.Mvvm.ComponentModel;

using DesktopApplication.Contracts.Services;
using DesktopApplication.ViewModels;
using DesktopApplication.Views;

using Microsoft.UI.Xaml.Controls;

namespace DesktopApplication.Services;

public class PageService : IPageService
{
    private readonly Dictionary<string, Type> _pages = new();

    public PageService()
    {
        Configure<LoginViewModel, LoginPage>();
        Configure<RegistrationViewModel, RegistrationPage>();
        Configure<AccountsViewModel, AccountsPage>();
        Configure<BudgetViewModel, BudgetPage>();
        Configure<ExpensesViewModel, ExpensesPage>();
        Configure<ProfileViewModel, ProfilePage>();
        Configure<SettingsViewModel, SettingsPage>();
        Configure<ReportsViewModel, ReportsPage>();
        Configure<HouseholdViewModel, HouseholdPage>();
    }

    public Type GetPageType(string key)
    {
        Type? pageType;
        lock (_pages)
        {
            if (!_pages.TryGetValue(key, out pageType))
            {
                throw new ArgumentException($"Page not found: {key}. Did you forget to call PageService.Configure?");
            }
        }

        return pageType;
    }

    private void Configure<VM, V>()
        where VM : ObservableObject
        where V : Page
    {
        lock (_pages)
        {
            var key = typeof(VM).FullName!;
            if (_pages.ContainsKey(key))
            {
                throw new ArgumentException($"The key {key} is already configured in PageService");
            }

            var type = typeof(V);
            if (_pages.Any(p => p.Value == type))
            {
                throw new ArgumentException($"This type is already configured with key {_pages.First(p => p.Value == type).Key}");
            }

            _pages.Add(key, type);
        }
    }
}

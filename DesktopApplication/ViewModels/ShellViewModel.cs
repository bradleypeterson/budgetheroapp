using CommunityToolkit.Mvvm.ComponentModel;
using DesktopApplication.Contracts.Data;
using DesktopApplication.Contracts.Services;
using DesktopApplication.Views;

using Microsoft.UI.Xaml.Navigation;
using ModelsLibrary;

namespace DesktopApplication.ViewModels;

public class ShellViewModel : ObservableRecipient
{
    private bool _isBackEnabled;
    private object? _selected;
    private readonly ISessionService _sessionService;
    private readonly IDataStore _dataStore;

    public INavigationService NavigationService
    {
        get;
    }

    public INavigationViewService NavigationViewService
    {
        get;
    }

    public bool IsBackEnabled
    {
        get => _isBackEnabled;
        set => SetProperty(ref _isBackEnabled, value);
    }

    public object? Selected
    {
        get => _selected;
        set => SetProperty(ref _selected, value);
    }

    public ShellViewModel(INavigationService navigationService, INavigationViewService navigationViewService)
    {
        NavigationService = navigationService;
        NavigationService.Navigated += OnNavigated;
        NavigationViewService = navigationViewService;
        _sessionService = App.GetService<ISessionService>();
        _dataStore = App.GetService<IDataStore>();
    }

    //Maybe we can use this to show the username in the logout bar? - AG
    public string GetUserName()
    {
        int uID = _sessionService.GetSessionUserId();
        User user = _dataStore.User.Get(u => u.UserId == uID);

        if (user is not null)
        {
            return user.Username;
        }
        else
        {
            return string.Empty;
        }

    }

    private void OnNavigated(object sender, NavigationEventArgs e)
    {
        IsBackEnabled = NavigationService.CanGoBack;

        if (e.SourcePageType == typeof(SettingsPage))
        {
            Selected = NavigationViewService.SettingsItem;
            return;
        }

        var selectedItem = NavigationViewService.GetSelectedItem(e.SourcePageType);
        if (selectedItem != null)
        {
            Selected = selectedItem;
        }
    }
}

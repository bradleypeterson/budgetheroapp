using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DesktopApplication.Contracts.Data;
using DesktopApplication.Contracts.Services;
using DesktopApplication.Services;
using DesktopApplication.Views;

using Microsoft.UI.Xaml.Navigation;
using ModelsLibrary;
using System.Windows.Input;

namespace DesktopApplication.ViewModels;

public class ShellViewModel : ObservableRecipient
{
    private bool _isBackEnabled;
    private bool _isActiveSession;
    private string? _activeUsername;
    private object? _selected;
    private readonly ISessionService _sessionService;
    private readonly IDataStore _dataStore;

    public INavigationService NavigationService { get; }

    public INavigationViewService NavigationViewService { get; }

    public bool IsBackEnabled
    {
        get => _isBackEnabled;
        set => SetProperty(ref _isBackEnabled, value);
    }

    public bool IsActiveSession
    {
        get => _isActiveSession;
        set => SetProperty(ref _isActiveSession, value);
    }

    public string? ActiveUsername
    {
        get => _activeUsername;
        set => SetProperty(ref _activeUsername, value);
    }

    public object? Selected
    {
        get => _selected;
        set => SetProperty(ref _selected, value);
    }

    public event EventHandler? OnLoggedOut;

    public ShellViewModel(INavigationService navigationService, INavigationViewService navigationViewService)
    {
        NavigationService = navigationService;
        NavigationService.Navigated += OnNavigated;
        NavigationViewService = navigationViewService;
        _sessionService = App.GetService<ISessionService>();
        _sessionService.OnSessionCreated += ActivateSession;
        _dataStore = App.GetService<IDataStore>();
        LogoutCommand = new RelayCommand(Logout);
    }

    public ICommand LogoutCommand { get; }

    private void ActivateSession(object? sender, EventArgs e)
    {
        _sessionService.OnSessionCreated -= ActivateSession;
        _sessionService.OnSessionDestroyed += DeactivateSession;
        ActiveUsername = _sessionService.GetSessionUsername();
        IsActiveSession = true;
    }

    private void DeactivateSession(object? sender, EventArgs e)
    {
        _sessionService.OnSessionCreated += ActivateSession;
        _sessionService.OnSessionDestroyed -= DeactivateSession;
        ActiveUsername = string.Empty;
        IsActiveSession = false;
        NavigateToLoginView();
    }

    private void Logout() => _sessionService.DestroySession();

    private void NavigateToLoginView() => NavigationService.NavigateTo(typeof(LoginViewModel).FullName!);

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

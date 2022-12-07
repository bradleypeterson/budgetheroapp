using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DesktopApplication.Contracts.Data;
using DesktopApplication.Contracts.Services;
using ModelsLibrary;

namespace DesktopApplication.ViewModels;

public class LoginViewModel : ObservableRecipient
{
    private readonly INavigationService _navigationService;
    private readonly ISessionService _sessionService;
    private readonly IDataStore _dataStore;
    private readonly IPasswordService _passwordService;
    private readonly IAPIService _apiService;

    public LoginViewModel()
    {
        _navigationService = App.GetService<INavigationService>();
        _sessionService = App.GetService<ISessionService>();
        _dataStore = App.GetService<IDataStore>();
        _passwordService = App.GetService<IPasswordService>();
        _apiService = App.GetService<IAPIService>();

        SignUpCommand = new RelayCommand(NavigateToSignUpView);
        LoginCommand = new AsyncRelayCommand(Login);
        IsFormComplete = false;
        IsSubmitting = false;
    }

    public event EventHandler? OnUserNotFound;
    public event EventHandler? OnValidLogin;
    public ICommand SignUpCommand { get; }

    public IAsyncRelayCommand LoginCommand { get; }

    private string? _username;
    public string? Username
    {
        get => _username;
        set
        {
            _username = value;
            ValidateFormCompletion();
        }
    }

    private string? _password;
    public string? Password
    {
        get => _password;
        set
        {
            _password = value;
            ValidateFormCompletion();
        }
    }
    private bool _isSubmitting;
    public bool IsSubmitting
    {
        get => _isSubmitting;
        set => SetProperty(ref _isSubmitting, value);
    }
    public bool IsFormComplete { get; set; }


    private async Task Login()
    {
        // First check the local database for an existing user.
        User? existingUser = await _dataStore.User.GetAsync(u => u.Username == _username, true);

        // If existing user is not in the local database, make an API call.
        if (existingUser is null)
        {
            IEnumerable<User>? users = await _apiService.GetAsync<IEnumerable<User>>("users");

            if (users is not null && users.Any())
            {
                existingUser = users.FirstOrDefault(u => u.Username == Username, null);

                if (existingUser is not null)
                    await _dataStore.User.AddAsync(existingUser);
            }    
        }

        if (existingUser is not null)
        {
            bool isValidPassword = _passwordService.VerifyPassword(_password!, existingUser.Password!);

            if (isValidPassword)
            {
                OnValidLogin?.Invoke(this, EventArgs.Empty);
                await Task.Delay(1500);
                _sessionService.CreateSession(existingUser);
                _navigationService.NavigateTo(typeof(AccountsViewModel).FullName!);
            }
            else
            {
                OnUserNotFound?.Invoke(this,EventArgs.Empty);
            }
        }
        else
        {
            OnUserNotFound?.Invoke(this, EventArgs.Empty);
        }
        
    }

    private void NavigateToSignUpView() => _navigationService.NavigateTo(typeof(RegistrationViewModel).FullName!);

    private void ValidateFormCompletion()
    {
        if (!string.IsNullOrEmpty(_username) && !string.IsNullOrEmpty(_password))
        {
            IsFormComplete = true;
        }
        else
        {
            IsFormComplete = false;
        }
        OnPropertyChanged(nameof(IsFormComplete));
    }
}
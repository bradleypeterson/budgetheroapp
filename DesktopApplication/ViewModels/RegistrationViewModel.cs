using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DesktopApplication.Contracts.Data;
using DesktopApplication.Contracts.Services;
using ModelsLibrary;

namespace DesktopApplication.ViewModels;

public class RegistrationViewModel : ObservableRecipient
{
    private readonly INavigationService _navigationService;
    private readonly IDataStore _dataStore;
    private readonly IPasswordService _passwordService;
    private readonly ISessionService _sessionService;

    public RegistrationViewModel()
    {
        _navigationService = App.GetService<INavigationService>();
        _dataStore = App.GetService<IDataStore>();
        _passwordService = App.GetService<IPasswordService>();
        _sessionService = App.GetService<ISessionService>();
        SignUpCommand = new AsyncRelayCommand(AddUser);
        CancelSignupCommand = new RelayCommand(NavigateBack);
    }

    public IAsyncRelayCommand SignUpCommand { get; }

    public ICommand CancelSignupCommand { get; }

    private string? _firstName;
    public string? FirstName
    {
        get => _firstName;
        set => SetProperty(ref _firstName, value);
    }

    private string? _lastName;
    public string? LastName
    {
        get => _lastName;
        set => SetProperty(ref _lastName, value);
    }

    private string? _username; 
    public string? Username
    {
        get => _username;
        set => SetProperty(ref _username, value);
    }

    private string? _email;
    public string? Email
    {
        get => _email;
        set => SetProperty(ref _email, value);
    }

    private string? _password;
    public string? Password
    {
        get => _password;
        set => SetProperty(ref _password, value);
    }

    private string? _confirmPassword;
    public string? ConfirmPassword
    {
        get => _confirmPassword;
        set => SetProperty(ref _confirmPassword, value);
    }

    public async Task AddUser()
    {
        
        await Task.Delay(1500);

        //if password isn't blank run this
        var hashedPassword = _passwordService.HashPassword(_password!);
        //


        User newUser = new()
        {
            FirstName = _firstName,
            LastName = _lastName,
            EmailAddress = _email,
            PercentageMod = 0,
            Username = _username,
            Password = hashedPassword
        };

        var result = await _dataStore.User.AddAsync(newUser);

        if (result == 1)
        {
            _sessionService.CreateSession(newUser);
            _navigationService.NavigateTo(typeof(AccountsViewModel).FullName!);
        }
    }

    private void NavigateBack() => _navigationService.GoBack();
}
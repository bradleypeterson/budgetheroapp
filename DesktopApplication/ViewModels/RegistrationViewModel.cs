using System.Globalization;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DesktopApplication.Contracts.Data;
using DesktopApplication.Contracts.Services;
using DesktopApplication.Data;
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
        IsFormComplete = false;
    }

    public event EventHandler? OnUsernameTaken;
    public event EventHandler? OnUsernameNotTaken;
    public event EventHandler? OnInvalidEmail;
    public event EventHandler? OnValidEmail;
    public event EventHandler? OnMismatchingPasswords;
    public event EventHandler? OnMatchingPasswords;


    public IAsyncRelayCommand SignUpCommand { get; }

    public ICommand CancelSignupCommand { get; }

    private string? _firstName;
    public string? FirstName
    {
        get => _firstName;
        set
        {
            SetProperty(ref _firstName, value);
            ValidateFormCompletion();
        }
    }

    private string? _lastName;
    public string? LastName
    {
        get => _lastName;
        set
        {
            SetProperty(ref _lastName, value);
            ValidateFormCompletion();
        }
    }

    private string? _username; 
    public string? Username
    {
        get => _username;
        set
        {
            SetProperty(ref _username, value);
            ValidateFormCompletion();
        }
    }

    private string? _email;
    public string? Email
    {
        get => _email;
        set 
        {
            SetProperty(ref _email, value);
            ValidateFormCompletion();

        }
    }

    private string? _password;
    public string? Password
    {
        get => _password;
        set {
            SetProperty(ref _password, value);
            ValidateFormCompletion();
        }
    }

    private string? _confirmPassword;
    public string? ConfirmPassword
    {
        get => _confirmPassword;
        set
        {
            SetProperty(ref _confirmPassword, value);
            ValidateFormCompletion();
        }
    }
    public bool IsFormComplete { get; set; }
    private bool dataValid = false;
    
    public async Task AddUser()
    {
        if (IsFormComplete)
        {
            var existingUser = await _dataStore.User.GetAsync(u => u.Username == _username);
            if (existingUser is null) {
                OnUsernameNotTaken?.Invoke(this, EventArgs.Empty);
                dataValid = true;
            } 
            else {
                OnUsernameTaken?.Invoke(this, EventArgs.Empty);
                dataValid = false;
            }

            if (IsValidEmail(_email))
            {
                OnValidEmail?.Invoke(this, EventArgs.Empty);
                dataValid = true;
            }
            else
            {
                OnInvalidEmail?.Invoke(existingUser, EventArgs.Empty);
                dataValid = false;
            }


            if(_password == _confirmPassword)
            {
                OnMatchingPasswords?.Invoke(this, EventArgs.Empty);
                dataValid = true;
            }
            else
            {
                OnMismatchingPasswords?.Invoke(this, EventArgs.Empty);
                dataValid = false;
            }


            if (dataValid)
            {
                await Task.Delay(1500);
                string hashedPassword = _passwordService.HashPassword(_password!);
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
                    CreateNewUserBudget();
                    _navigationService.NavigateTo(typeof(AccountsViewModel).FullName!);
                }
            }
                  
        }
    }

    private void ValidateFormCompletion()
    {
        if (!string.IsNullOrEmpty(_firstName) 
            && !string.IsNullOrEmpty(_lastName)
            && !string.IsNullOrEmpty(_username)
            && !string.IsNullOrEmpty(_email)
            && !string.IsNullOrEmpty(_password)
            && !string.IsNullOrEmpty(_confirmPassword))
        {
            IsFormComplete = true;
        }
        else
        {
            IsFormComplete = false;
        }
        OnPropertyChanged(nameof(IsFormComplete));
    }

    private static bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        try
        {
            // Normalize the domain
            email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                  RegexOptions.None, TimeSpan.FromMilliseconds(200));

            // Examines the domain part of the email and normalizes it.
            string DomainMapper(Match match)
            {
                // Use IdnMapping class to convert Unicode domain names.
                var idn = new IdnMapping();

                // Pull out and process domain name (throws ArgumentException on invalid)
                string domainName = idn.GetAscii(match.Groups[2].Value);

                return match.Groups[1].Value + domainName;
            }
        }
        catch (RegexMatchTimeoutException e)
        {
            return false;
        }
        catch (ArgumentException e)
        {
            return false;
        }

        try
        {
            return Regex.IsMatch(email,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }
    }

    private void CreateNewUserBudget()
    {
        int userId = _sessionService.GetSessionUserId();
        User? user = _dataStore.User.Get(u => u.UserId == userId, false, "Budgets");
        
        if (user is not null)
        {
            List<Budget> userBudgets;
            if (user.Budgets is not null)
                userBudgets = user.Budgets.ToList();
            else
                userBudgets = new List<Budget>();

            Budget budget = new()
            {
                BudgetName = "Personal Budget",
                BudgetType = "personal",
            };

            userBudgets.Add(budget);

            user.Budgets = userBudgets;

            _dataStore.User.Update(user);
        }
    }

    private void NavigateBack() => _navigationService.GoBack();
}
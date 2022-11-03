using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using DesktopApplication.Commands;
using DesktopApplication.Contracts.Services;

namespace DesktopApplication.ViewModels;

public class LoginViewModel : ObservableRecipient
{
    public ICommand NavigateCommand { get; }
    public ICommand DashboardCommand { get; }

    public LoginViewModel()
    {
        var _navigationService = App.GetService<INavigationService>();
        NavigateCommand = new NavigateCommand(_navigationService, typeof(RegistrationViewModel).FullName!);
        DashboardCommand = new NavigateCommand(_navigationService, typeof(AccountsViewModel).FullName!);
    }
}

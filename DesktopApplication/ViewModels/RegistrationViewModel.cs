using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using DesktopApplication.Commands;
using DesktopApplication.Contracts.Services;

namespace DesktopApplication.ViewModels;

public class RegistrationViewModel : ObservableRecipient
{
    public ICommand NavigateCommand { get; }

    public RegistrationViewModel()
    {
        var _navigationService = App.GetService<INavigationService>();
        NavigateCommand = new NavigateCommand(_navigationService, typeof(AccountsViewModel).FullName!);
    }
}

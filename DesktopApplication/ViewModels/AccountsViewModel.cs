using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using DesktopApplication.Commands;

namespace DesktopApplication.ViewModels;

public class AccountsViewModel : ObservableRecipient
{
    public ICommand AddAccountDialogCommand { get; }

    public AccountsViewModel()
    {
        AddAccountDialogCommand = new AddAccountCommand();
    }
}

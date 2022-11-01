using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using DesktopApplication.Commands;

namespace DesktopApplication.ViewModels;

public class AccountsViewModel : ObservableRecipient
{
    public ICommand ShowDialogCommand { get; }

    public AccountsViewModel()
    {
        ShowDialogCommand = new ShowDialogCommand();
    }
}

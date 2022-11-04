using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using DesktopApplication.Commands;

namespace DesktopApplication.ViewModels;

public class ExpensesViewModel : ObservableRecipient
{
    public ICommand AddExpenseDialogCommand { get; }
    public ICommand EditExpenseDialogCommand { get; }
    public ICommand DeleteExpenseDialogCommand { get; }

    public ExpensesViewModel()
    {
        AddExpenseDialogCommand = new AddExpenseCommand();
        EditExpenseDialogCommand = new EditExpenseCommand();
        DeleteExpenseDialogCommand = new DeleteExpenseCommand();
    }
}

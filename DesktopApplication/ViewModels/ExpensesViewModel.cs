using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using DesktopApplication.Commands;

namespace DesktopApplication.ViewModels;

public class ExpensesViewModel : ObservableRecipient
{
    public ICommand ExpenseDialogCommand { get; }

    public ExpensesViewModel()
    {
        ExpenseDialogCommand = new AddExpenseCommand();
    }
}

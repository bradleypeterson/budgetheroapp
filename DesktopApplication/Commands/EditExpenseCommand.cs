using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesktopApplication.Contracts.Services;

namespace DesktopApplication.Commands;
internal class EditExpenseCommand : CommandBase
{
    private readonly IDialogService _dialogService;

    public EditExpenseCommand()
    {
        _dialogService = App.GetService<IDialogService>();
    }

    public override void Execute(object? parameter)
    {
        _dialogService.EditExpenseDialog();
        
        //Add code to poulate the expense dialog
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesktopApplication.Contracts.Services;

namespace DesktopApplication.Commands;
internal class EditCategoryGroupCommand : CommandBase
{
    private readonly IDialogService _dialogService;

    public EditCategoryGroupCommand()
    {
        _dialogService = App.GetService<IDialogService>();
    }

    public override void Execute(object? parameter)
    {
        _dialogService.EditCategoryGroupDialog();
    }

}

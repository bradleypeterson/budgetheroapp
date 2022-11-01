using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesktopApplication.Contracts.Services;

namespace DesktopApplication.Commands;
public class ShowDialogCommand : CommandBase
{
    private readonly IDialogService _dialogService;

    public ShowDialogCommand()
    {
        _dialogService = App.GetService<IDialogService>();
    }

    public override void Execute(object? parameter)
    {
        _dialogService.ShowDialog();
    }
}

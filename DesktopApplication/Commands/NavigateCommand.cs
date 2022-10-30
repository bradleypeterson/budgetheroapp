using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesktopApplication.Contracts.Services;

namespace DesktopApplication.Commands;
public class NavigateCommand : CommandBase
{
    private readonly INavigationService _navigationService;
    private readonly string _pageKey;

    public NavigateCommand(INavigationService navigationService, string pageKey)
    {
        _navigationService = navigationService;
        _pageKey = pageKey;
    }

    public override void Execute(object? parameter)
    {
        _navigationService.NavigateTo(_pageKey);
    }
}

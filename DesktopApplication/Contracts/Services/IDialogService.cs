using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesktopApplication.CustomEventArgs;
using Microsoft.UI.Xaml.Controls;

namespace DesktopApplication.Contracts.Services;
public interface IDialogService
{
    event EventHandler<DialogServiceEventArgs> OnSaved;
    Task ShowDialogAsync<TForm>(string dialogTitle, object? model = null);
}

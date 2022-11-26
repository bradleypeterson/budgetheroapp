using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace DesktopApplication.ViewModels.Forms;
public class DeleteItemViewModel : ObservableRecipient
{
    private string? _message;
    public string? Message
    {
        get => _message;
        set => SetProperty(ref _message, value);
    }
}

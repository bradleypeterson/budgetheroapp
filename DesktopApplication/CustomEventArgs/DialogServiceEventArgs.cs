using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;

namespace DesktopApplication.CustomEventArgs;
public class DialogServiceEventArgs : EventArgs
{
    public DialogServiceEventArgs(object content)
    {
        Content = content;
    }

    public object Content { get; set; }
}

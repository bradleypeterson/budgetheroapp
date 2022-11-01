using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopApplication.Commands;
public class AddAccountCommand : CommandBase
{
    public override void Execute(object? parameter)
    {
        // Implement account creating logic here
        Debug.WriteLine("!!!! Account has been added !!!!");
    }
}

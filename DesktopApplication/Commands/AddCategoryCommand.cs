using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopApplication.Commands;
internal class AddCategoryCommand : CommandBase
{
    public override void Execute(object? parameter)
    {
        // Implement Category Group Creating logic here
        Debug.WriteLine("!!!!!!!!!! Add category command");

    }
}

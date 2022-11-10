using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopApplication.Converters;
public class PropertyConverter
{
    public static decimal ConvertToDecimal(object value)
    {
        if (value is string s && value != null)
        {
            decimal temp;
            if (decimal.TryParse(s, out temp))
            {
                return temp;
            }
            else
            {
                Debug.WriteLine("Error: You must provide a decimal value.");
            }
        }
        else if (value is decimal d && value != null) { return d;  }
        return -1;
    }
}

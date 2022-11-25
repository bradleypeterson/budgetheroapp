using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopApplication.Helpers
{
    public class FormValidator
    {
        public static bool ValidateDecimal(string value)
            => decimal.TryParse(value, out decimal _);

        public static bool ValidateSelection(int value)
            => value != -1;

        public static bool ValidateString(string value)
            => !string.IsNullOrEmpty(value);

        public static bool ValidateDate(object? value)
            => value is DateTimeOffset;
    }
}

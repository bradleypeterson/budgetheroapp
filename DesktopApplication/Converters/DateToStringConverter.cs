using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace DesktopApplication.Converters;
public class DateToStringConverter : IValueConverter
{
    public static readonly DependencyProperty FormatProperty =
        DependencyProperty.Register(nameof(Format), typeof(bool), typeof(DecimalToStringConverter), new PropertyMetadata("G"));

    public string? Format { get; set; }

    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is DateTime datetime && value is not null)
        {
            return datetime.ToString(Format);
        }
        return null!;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if (value is string dateString && value is not null)
        {
            return DateTime.Parse(dateString);
        }

        return null!;
    }
}

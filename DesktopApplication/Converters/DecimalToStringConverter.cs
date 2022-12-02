using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace DesktopApplication.Converters;
public sealed class DecimalToStringConverter : IValueConverter
{
    public static readonly DependencyProperty FormatProperty =
        DependencyProperty.Register(nameof(Format), typeof(bool), typeof(DecimalToStringConverter), new PropertyMetadata("G"));

    public string? Format { get; set; }

    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is decimal d && value != null)
        {
            if (d == 0) { return "$0.00"; }
            return d.ToString(Format);
        }
        else if (value is string s && value != null)
        {
            object obj = ConvertBack(value, targetType, parameter, language);
            return Convert(obj, targetType, parameter, language);
        }

        return null!;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if (value is string s && value != null)
        {
            decimal d;
            if (s == string.Empty) { return 0; }
            if (Decimal.TryParse(s, out d)) { return d; }
        }

        return -1;
    }
}

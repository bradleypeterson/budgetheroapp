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
            return d.ToString(Format);
        }

        return null!;
    }
    public object ConvertBack(object value, Type targetType, object parameter, string language) 
        => throw new NotImplementedException();
}

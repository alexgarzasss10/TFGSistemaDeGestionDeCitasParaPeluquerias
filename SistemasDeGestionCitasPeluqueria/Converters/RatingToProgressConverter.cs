using Microsoft.Maui.Controls;
using System.Globalization;

namespace SistemasDeGestionCitasPeluqueria.Converters;

public sealed class RatingToProgressConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var count = value is int i ? i : 0;
        var total = parameter switch { int t => t, _ => 0 };
        return total <= 0 ? 0d : Math.Clamp((double)count / total, 0, 1);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotSupportedException();
}

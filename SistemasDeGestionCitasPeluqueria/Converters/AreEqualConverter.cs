using Microsoft.Maui.Controls;
using System.Globalization;

namespace SistemasDeGestionCitasPeluqueria.Converters;

public sealed class AreEqualConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        => values is [var a, var b] && Equals(a, b);

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        => throw new NotSupportedException();
}

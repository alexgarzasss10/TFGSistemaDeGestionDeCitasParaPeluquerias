using System;
using System.Globalization;

namespace SistemasDeGestionCitasPeluqueria.Converters;

public class NullOrWhiteToBoolConverter : IValueConverter
{
    public bool Invert { get; set; }
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var has = value is string s && !string.IsNullOrWhiteSpace(s);
        return Invert ? !has : has;
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotSupportedException();
}